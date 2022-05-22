﻿using Newtonsoft.Json;

namespace AniListNet.Objects;

public class User
{

    [JsonProperty("id")] public int Id { get; private init; }
    [JsonProperty("name")] public string Name { get; private init; }
    [JsonProperty("about")] public string? About { get; private init; }
    [JsonProperty("avatar")] public Image Avatar { get; private init; }
    [JsonProperty("bannerImage")] public Uri? BannerImageUrl { get; private init; }

}