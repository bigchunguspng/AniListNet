﻿using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet;

public partial class AniClient
{

    public async Task<AniPagination<User>> GetUserFollowersAsync(int id, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var request = GqlParser.ParseSelection(new GqlSelection("Page", new GqlSelection[]
        {
            new("pageInfo", typeof(PageInfo).ToSelections()),
            new("followers", typeof(User).ToSelections(), new GqlParameter[]
            {
                new("userId", id)
            })
        }, new GqlParameter[]
        {
            new("page", options.PageIndex),
            new("perPage", options.PageSize)
        }));
        var response = await SendRequestAsync(request);
        return new AniPagination<User>(
            response["Page"]["pageInfo"].ToObject<PageInfo>(),
            response["Page"]["followers"].ToObject<User[]>()
        );
    }

    public async Task<AniPagination<User>> GetUserFollowingsAsync(int id, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var request = GqlParser.ParseSelection(new GqlSelection("Page", new GqlSelection[]
        {
            new("pageInfo", typeof(PageInfo).ToSelections()),
            new("following", typeof(User).ToSelections(), new GqlParameter[]
            {
                new("userId", id)
            })
        }, new GqlParameter[]
        {
            new("page", options.PageIndex),
            new("perPage", options.PageSize)
        }));
        var response = await SendRequestAsync(request);
        return new AniPagination<User>(
            response["Page"]["pageInfo"].ToObject<PageInfo>(),
            response["Page"]["following"].ToObject<User[]>()
        );
    }

    public async Task<AniPagination<MediaEntry>> GetUserEntriesAsync(int id, AniPaginationOptions? options = null)
    {
        options ??= new AniPaginationOptions();
        var request = GqlParser.ParseSelection(new GqlSelection("Page", new GqlSelection[]
        {
            new("pageInfo", typeof(PageInfo).ToSelections()),
            new("mediaList", typeof(MediaEntry).ToSelections(), new GqlParameter[]
            {
                new("userId", id)
            })
        }, new GqlParameter[]
        {
            new("page", options.PageIndex),
            new("perPage", options.PageSize)
        }));
        var response = await SendRequestAsync(request);
        return new AniPagination<MediaEntry>(
            response["Page"]["pageInfo"].ToObject<PageInfo>(),
            response["Page"]["mediaList"].ToObject<MediaEntry[]>()
        );
    }

}