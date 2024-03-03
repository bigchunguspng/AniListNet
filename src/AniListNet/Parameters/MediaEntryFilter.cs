using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet.Parameters;

public class MediaEntryFilter : AbstractFilter
{
    public MediaType? Type { get; set; }
    public MediaEntryStatus? Status { get; set; }

    public MediaEntrySort Sort { get; set; } = MediaEntrySort.LastUpdated;

    public override IList<GqlParameter> ToParameters()
    {
        var parameters = new List<GqlParameter>();
        if (Type.HasValue)
            parameters.Add(new GqlParameter("type", Type));
        if (Status.HasValue)
            parameters.Add(new GqlParameter("status", Status));
        parameters.Add(new GqlParameter("sort", $"${HelperUtilities.GetEnumMemberValue(Sort)}" + (SortDescending ? "_DESC" : string.Empty)));
        return parameters;
    }
}