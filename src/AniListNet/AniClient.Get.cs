using AniListNet.Helpers;
using AniListNet.Objects;
using AniListNet.Parameters;

namespace AniListNet;

public partial class AniClient
{
    /// <summary>
    /// Generic method to get an object with the given ID. Use it at your own risk.
    /// </summary>
    public async Task<T> GetAsync<T>(int id, string key)
    {
        var selections = new GqlSelection(key)
        {
            Parameters = new GqlParameter[] { new("id", id) },
            Selections = GqlParser.ParseToSelections<T>().ToArray()
        };
        var response = await PostRequestAsync(selections);
        return GqlParser.ParseFromJson<T>(response[key]);
    }

    /// <summary>
    /// Generic method to get paginated data. Use it at your own risk.
    /// </summary>
    public async Task<AniPagination<T>> GetPaginatedAsync<T>(IEnumerable<GqlParameter> parameters, string key, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var selections = new GqlSelection("Page")
        {
            Parameters = options.ToParameters(),
            Selections = new GqlSelection[]
            {
                new("pageInfo", GqlParser.ParseToSelections<PageInfo>()),
                new(key, GqlParser.ParseToSelections<T>(), parameters)
            }
        };
        var response = await PostRequestAsync(selections);
        var pageInfo = GqlParser.ParseFromJson<PageInfo>(response["Page"]["pageInfo"]);
        var pageData = GqlParser.ParseFromJson<T[]>(response["Page"][key]);
        return new AniPagination<T>(pageInfo, pageData);
    }

    public Task<AniPagination<T>> GetPaginatedAsync<T>(AbstractFilter filter, string key, AniPaginationOptions? options = null)
    {
        return GetPaginatedAsync<T>(filter.ToParameters(), key, options);
    }

    /// <summary>
    /// Gets a collection of supported genres.
    /// </summary>
    public async Task<string[]> GetGenreCollectionAsync()
    {
        var selections = new GqlSelection("GenreCollection");
        var response = await PostRequestAsync(selections);
        return GqlParser.ParseFromJson<string[]>(response["GenreCollection"]);
    }

    /// <summary>
    /// Gets a collection of supported tags.
    /// </summary>
    public async Task<MediaTag[]> GetTagCollectionAsync()
    {
        var selections = new GqlSelection("MediaTagCollection")
        {
            Selections = GqlParser.ParseToSelections<MediaTag>().ToArray()
        };
        var response = await PostRequestAsync(selections);
        return GqlParser.ParseFromJson<MediaTag[]>(response["MediaTagCollection"]);
    }

    /// <summary>
    /// Gets the media with the given ID.
    /// </summary>
    public Task<Media> GetMediaAsync(int mediaId)
    {
        return GetAsync<Media>(mediaId, "Media");
    }

    /// <summary>
    /// Gets the review with the given ID.
    /// </summary>
    public Task<MediaReview> GetMediaReviewAsync(int reviewId)
    {
        return GetAsync<MediaReview>(reviewId, "Review");
    }

    /// <summary>
    /// Gets collection of media schedules.
    /// </summary>
    public Task<AniPagination<MediaSchedule>> GetMediaSchedulesAsync(MediaSchedulesFilter? filter = null, AniPaginationOptions? paginationOptions = null)
    {
        filter ??= new MediaSchedulesFilter();
        return GetPaginatedAsync<MediaSchedule>(filter, "airingSchedules", paginationOptions);
    }

    /// <summary>
    /// Gets collection of trending media.
    /// </summary>
    public Task<AniPagination<MediaTrend>> GetTrendingMediaAsync(MediaTrendFilter? filter = null, AniPaginationOptions? paginationOptions = null)
    {
        filter ??= new MediaTrendFilter();
        return GetPaginatedAsync<MediaTrend>(filter, "mediaTrends", paginationOptions);
    }

    /// <summary>
    /// Gets the character with the given ID.
    /// </summary>
    public Task<Character> GetCharacterAsync(int characterId)
    {
        return GetAsync<Character>(characterId, "Character");
    }

    /// <summary>
    /// Gets the staff with the given ID.
    /// </summary>
    public Task<Staff> GetStaffAsync(int staffId)
    {
        return GetAsync<Staff>(staffId, "Staff");
    }

    /// <summary>
    /// Gets the studio with the given ID.
    /// </summary>
    public Task<Studio> GetStudioAsync(int studioId)
    {
        return GetAsync<Studio>(studioId, "Studio");
    }

    /// <summary>
    /// Gets the user with the given ID.
    /// </summary>
    public Task<User> GetUserAsync(int userId)
    {
        return GetAsync<User>(userId, "User");
    }
}