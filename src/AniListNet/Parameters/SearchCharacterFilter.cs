using AniListNet.Helpers;
using AniListNet.Objects;

namespace AniListNet.Parameters;

public class SearchCharacterFilter : AbstractSearchFilter
{
    public bool? IsBirthday { get; set; }

    public CharacterSort Sort { get; set; } = CharacterSort.Relevance;

    public override IList<GqlParameter> ToParameters()
    {
        var parameters = new List<GqlParameter>();
        if (IsBirthday.HasValue)
            parameters.Add(new GqlParameter("isBirthday", IsBirthday));
        if (!string.IsNullOrEmpty(Query))
            parameters.Add(new GqlParameter("search", Query));
        parameters.Add(
            new GqlParameter(
                "sort",
                $"${HelperUtilities.GetEnumMemberValue(Sort)}" +
                (SortDescending && Sort != CharacterSort.Relevance ? "_DESC" : string.Empty)
            )
        );
        return parameters;
    }
}