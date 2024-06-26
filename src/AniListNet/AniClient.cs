﻿using System.Net.Http.Headers;
using System.Text;
using AniListNet.Helpers;
using Newtonsoft.Json.Linq;

namespace AniListNet;

public partial class AniClient
{
    private readonly HttpClient _client = new();
    private readonly Uri _url = new("https://graphql.anilist.co");

    public bool IsAuthenticated { get; private set; }

    public event EventHandler<AniRateEventArgs>? RateChanged;

    public async Task<bool> TryAuthenticateAsync(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            _ = await GetAuthenticatedUserAsync();
            IsAuthenticated = true;
        }
        catch
        {
            _client.DefaultRequestHeaders.Authorization = null;
            IsAuthenticated = false;
        }
        return IsAuthenticated;
    }

    public async Task<JToken> PostRequestAsync(GqlSelection selection, bool isMutation = false)
    {
        var body = JObject.FromObject(new { query = (isMutation ? "mutation" : string.Empty) + selection });
        var bodyText = body["query"].ToObject<string>();
        var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(_url, content);
        var responseText = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(responseText);
        if (!response.IsSuccessStatusCode)
            throw new AniException
            (
                json["errors"].First["message"].ToString(),
                bodyText,
                responseText
            );
        response.Headers.TryGetValues("X-RateLimit-Limit", out var rateLimitValues);
        response.Headers.TryGetValues("X-RateLimit-Remaining", out var rateRemainingValues);
        var rateLimitString = rateLimitValues?.FirstOrDefault();
        var rateRemainingString = rateRemainingValues?.FirstOrDefault();
        var rateLimitValidated = int.TryParse(rateLimitString, out var rateLimit);
        var rateRemainingValidated = int.TryParse(rateRemainingString, out var rateRemaining);
        if (rateLimitValidated && rateRemainingValidated)
            RateChanged?.Invoke(this, new AniRateEventArgs(rateLimit, rateRemaining));
        return json["data"];
    }

    public async Task<JToken> GetSingleDataAsync(params GqlSelection[] path)
    {
        var selection = path[^1];
        for (var index = path.Length - 2; index >= 0; index--)
        {
            var newSelection = path[index];
            newSelection.Selections ??= new List<GqlSelection>();
            newSelection.Selections.Add(selection);
            selection = newSelection;
        }
        var token = await PostRequestAsync(selection);
        foreach (var item in path)
            token = token[item.Name];
        return token;
    }
}