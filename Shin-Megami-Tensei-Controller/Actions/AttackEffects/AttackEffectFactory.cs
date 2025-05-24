namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class AttackEffectFactory
{
    private static readonly Dictionary<string, IAttackEffectHandler> _handlers = new()
    {
        { "Wk", new NormalAttackEffect() },
        { "Rs", new NormalAttackEffect() },
        { "Nu", new NullAttackEffect() },
        { "Rp", new RepelAttackEffect() },
        { "Dr", new DrainAttackEffect() },
        { "-", new NormalAttackEffect() }
    };
    
    public IAttackEffectHandler GetEffectHandler(string affinityCode)
    {
        return _handlers.GetValueOrDefault(affinityCode, _handlers["-"]);
    }
}