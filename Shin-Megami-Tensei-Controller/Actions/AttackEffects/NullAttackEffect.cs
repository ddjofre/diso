using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class NullAttackEffect : IAttackEffectHandler
{
    public void ApplyEffect(Unit attacker, Unit target, int damage)
    {
    }
    
    public bool ShouldShowAttackMessage()
    {
        return true;
    }

    public string GetDamageMessage(Unit attacker, Unit target)
    {
        return null;
    }
    
    public Unit GetAffectedUnit(Unit attacker, Unit target)
    {
        return target;
    }
}