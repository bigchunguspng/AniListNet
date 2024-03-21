using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet.Parameters;

public class SearchStudioFilter : AbstractSearchFilter
{
    public StudioSort Sort { get; set; } = StudioSort.Relevance;

    public override IList<GqlParameter> ToParameters()
    {
        var parameters = new List<GqlParameter>();
        if (!string.IsNullOrEmpty(Query))
            parameters.Add(new GqlParameter("search", Query));
        parameters.Add(
            new GqlParameter(
                "sort",
                $"${HelperUtilities.GetEnumMemberValue(Sort)}" +
                (SortDescending && Sort != StudioSort.Relevance ? "_DESC" : string.Empty)
            )
        );
        return parameters;
    }
}