using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet.Parameters;

public class MediaReviewFilter : AbstractFilter
{
    public MediaType? Type { get; set; }

    public MediaReviewSort Sort { get; set; } = MediaReviewSort.Rating;

    public override IList<GqlParameter> ToParameters()
    {
        var parameters = new List<GqlParameter>
        {
            new("sort", $"${HelperUtilities.GetEnumMemberValue(Sort)}" + (SortDescending ? "_DESC" : string.Empty))
        };
        if (Type.HasValue)
            parameters.Add(new GqlParameter("mediaType", Type.Value));
        return parameters;
    }
}