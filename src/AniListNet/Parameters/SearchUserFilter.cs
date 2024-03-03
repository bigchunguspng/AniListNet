﻿using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet.Parameters;

public class SearchUserFilter : AbstractSearchFilter
{
    public bool? IsModerator { get; set; }

    public UserSort Sort { get; set; } = UserSort.Relevance;

    public override IList<GqlParameter> ToParameters()
    {
        var parameters = new List<GqlParameter>();
        if (IsModerator.HasValue)
            parameters.Add(new GqlParameter("isModerator", IsModerator));
        if (!string.IsNullOrEmpty(Query))
            parameters.Add(new GqlParameter("search", Query));
        parameters.Add(
            new GqlParameter(
                "sort",
                $"${HelperUtilities.GetEnumMemberValue(Sort)}" +
                (SortDescending && Sort != UserSort.Relevance ? "_DESC" : string.Empty)
            )
        );
        return parameters;
    }
}