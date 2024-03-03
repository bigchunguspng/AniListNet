using AniListNet.Helpers;
using AniListNet.Objects;
using AniListNet.Parameters;

namespace AniListNet;

public partial class AniClient
{
    public GqlParameter[] GetUserIdParams(int userId) => new[] { GetUserIdParameter(userId) };
    public GqlParameter GetUserIdParameter(int userId) => new("userId", userId);

    public Task<AniPagination<User>> GetUserFollowersAsync(int userId, AniPaginationOptions? options = null)
    {
        var parameters = GetUserIdParams(userId);
        return GetPaginatedAsync<User>(parameters, "followers", options);
    }

    public Task<AniPagination<User>> GetUserFollowingsAsync(int userId, AniPaginationOptions? options = null)
    {
        var parameters = GetUserIdParams(userId);
        return GetPaginatedAsync<User>(parameters, "following", options);
    }

    public Task<AniPagination<MediaEntry>> GetUserEntriesAsync(int userId, MediaEntryFilter? filter = null, AniPaginationOptions? options = null)
    {
        filter ??= new MediaEntryFilter();
        var parameters = GetUserIdParams(userId).Concat(filter.ToParameters());
        return GetPaginatedAsync<MediaEntry>(parameters, "mediaList", options);
    }

    public async Task<MediaEntryCollection> GetUserEntryCollectionAsync(int userId, MediaType type, AniPaginationOptions? paginationOptions = null)
    {
        paginationOptions ??= new AniPaginationOptions();
        var selections = new GqlSelection("MediaListCollection", GqlParser.ParseToSelections<MediaEntryCollection>(), new[]
        {
            GetUserIdParameter(userId),
            new("type", type),
            new("chunk", paginationOptions.PageIndex),
            new("perChunk", paginationOptions.PageSize)
        });
        var response = await PostRequestAsync(selections);
        return GqlParser.ParseFromJson<MediaEntryCollection>(response["MediaListCollection"]);
    }

    public async Task<MediaListCollection> GetUserListCollectionAsync(int userId, MediaType type)
    {
        var selections = new GqlSelection("MediaListCollection", GqlParser.ParseToSelections<MediaListCollection>(), new[]
        {
            GetUserIdParameter(userId),
            new("type", type)
        });
        var response = await PostRequestAsync(selections);
        return GqlParser.ParseFromJson<MediaListCollection>(response["MediaListCollection"]);
    }

    public Task<AniPagination<MediaReview>> GetUserMediaReviewsAsync(int userId, MediaReviewFilter? filter = null, AniPaginationOptions? options = null)
    {
        filter ??= new MediaReviewFilter();
        var parameters = GetUserIdParams(userId).Concat(filter.ToParameters());
        return GetPaginatedAsync<MediaReview>(parameters, "reviews", options);
    }

    public Task<AniPagination<Media>> GetUserAnimeFavoritesAsync(int userId, AniPaginationOptions? options = null)
    {
        return GetUserFavoritesAsync<Media>(userId, "anime", options);
    }

    public Task<AniPagination<Media>> GetUserMangaFavoritesAsync(int userId, AniPaginationOptions? options = null)
    {
        return GetUserFavoritesAsync<Media>(userId, "manga", options);
    }

    public Task<AniPagination<Character>> GetUserCharacterFavoritesAsync(int userId, AniPaginationOptions? options = null)
    {
        return GetUserFavoritesAsync<Character>(userId, "characters", options);
    }

    public Task<AniPagination<Staff>> GetUserStaffFavoritesAsync(int userId, AniPaginationOptions? options = null)
    {
        return GetUserFavoritesAsync<Staff>(userId, "staff", options);
    }

    public Task<AniPagination<Studio>> GetUserStudioFavoritesAsync(int userId, AniPaginationOptions? options = null)
    {
        return GetUserFavoritesAsync<Studio>(userId, "studios", options);
    }

    /* below are methods made for private use */

    private async Task<AniPagination<TObject>> GetUserFavoritesAsync<TObject>(int userId, string type, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var selections = new GqlSelection("User")
        {
            Parameters = new GqlParameter[] { new("id", userId) },
            Selections = new GqlSelection[]
            {
                new("favourites")
                {
                    Selections = new GqlSelection[]
                    {
                        new(type)
                        {
                            Parameters = options.ToParameters(),
                            Selections = new GqlSelection[]
                            {
                                new("pageInfo", GqlParser.ParseToSelections<PageInfo>()),
                                new("nodes", GqlParser.ParseToSelections<TObject>())
                            }
                        }
                    }
                }
            }
        };
        var response = await PostRequestAsync(selections);
        var pageInfo = GqlParser.ParseFromJson<PageInfo> (response["User"]["favourites"][type]["pageInfo"]);
        var pageData = GqlParser.ParseFromJson<TObject[]>(response["User"]["favourites"][type]["nodes"]);
        return new AniPagination<TObject>(pageInfo, pageData);
    }
}