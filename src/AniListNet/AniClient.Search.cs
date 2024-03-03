using AniListNet.Helpers;
using AniListNet.Objects;
using AniListNet.Parameters;

namespace AniListNet;

public partial class AniClient
{
    public async Task<AniPagination<T>> SearchAsync<T>(AbstractSearchFilter filter, string key, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var selections = new GqlSelection("Page")
        {
            Parameters = options.ToParameters(),
            Selections = new GqlSelection[]
            {
                new("pageInfo", GqlParser.ParseToSelections<PageInfo>()),
                new(key, GqlParser.ParseToSelections<T>(), filter.ToParameters())
            }
        };
        var response = await PostRequestAsync(selections);
        var pageInfo = GqlParser.ParseFromJson<PageInfo>(response["Page"]["pageInfo"]);
        var pageData = GqlParser.ParseFromJson<T[]>(response["Page"][key]);
        return new AniPagination<T>(pageInfo, pageData);
    }

    public Task<AniPagination<Media>> SearchMediaAsync(SearchMediaFilter filter, AniPaginationOptions? options = null)
    {
        return SearchAsync<Media>(filter, "media", options);
    }

    public Task<AniPagination<Character>> SearchCharacterAsync(SearchCharacterFilter filter, AniPaginationOptions? options = null)
    {
        return SearchAsync<Character>(filter, "characters", options);
    }

    public Task<AniPagination<Staff>> SearchStaffAsync(SearchStaffFilter filter, AniPaginationOptions? options = null)
    {
        return SearchAsync<Staff>(filter, "staff", options);
    }

    public Task<AniPagination<Studio>> SearchStudioAsync(SearchStudioFilter filter, AniPaginationOptions? options = null)
    {
        return SearchAsync<Studio>(filter, "studios", options);
    }

    public Task<AniPagination<User>> SearchUserAsync(SearchUserFilter filter, AniPaginationOptions? options = null)
    {
        return SearchAsync<User>(filter, "users", options);
    }

    /* below are methods that are the simplified versions of the above */

    public Task<AniPagination<Media>> SearchMediaAsync(string query, AniPaginationOptions? options = null)
    {
        return SearchMediaAsync(new SearchMediaFilter { Query = query }, options);
    }

    public Task<AniPagination<Character>> SearchCharacterAsync(string query, AniPaginationOptions? options = null)
    {
        return SearchCharacterAsync(new SearchCharacterFilter { Query = query }, options);
    }

    public Task<AniPagination<Staff>> SearchStaffAsync(string query, AniPaginationOptions? options = null)
    {
        return SearchStaffAsync(new SearchStaffFilter { Query = query }, options);
    }

    public Task<AniPagination<Studio>> SearchStudioAsync(string query, AniPaginationOptions? options = null)
    {
        return SearchStudioAsync(new SearchStudioFilter { Query = query }, options);
    }

    public Task<AniPagination<User>> SearchUserAsync(string query, AniPaginationOptions? options = null)
    {
        return SearchUserAsync(new SearchUserFilter { Query = query }, options);
    }
}