namespace Shin_Megami_Tensei.Actions.Affinities;

public class AffinityFactory
{
    private static readonly Dictionary<string, BaseAffinity> _affinities = new()
    {
        { "Wk", new WeakAffinity() },
        { "Rs", new ResistAffinity() },
        { "Nu", new NullAffinity() },
        { "Rp", new RepelAffinity() },
        { "Dr", new DrainAffinity() },
        { "-", new NeutralAffinity()}
    };
    
    public BaseAffinity GetAffinity(string nameAffinity)
    {
        return _affinities.GetValueOrDefault(nameAffinity, _affinities["-"]);
    }
    
}