using AniListNet.Helpers;

namespace AniListNet.Parameters;

public abstract class AbstractSearchFilter
{
    public string? Query { get; set; }
    public bool SortDescending { get; set; } = true;

    public abstract IList<GqlParameter> ToParameters();
}