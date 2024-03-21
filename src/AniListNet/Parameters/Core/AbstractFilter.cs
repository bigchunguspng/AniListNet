using AniListNet.Helpers;

namespace AniListNet.Parameters;

public abstract class AbstractFilter
{
    public virtual bool SortDescending { get; set; } = true;

    public abstract IList<GqlParameter> ToParameters();
}