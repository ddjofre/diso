using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class NullAttackEffect : IAttackEffectHandler
{
    public void ApplyEffect(Unit attacker, Unit target, int damage)
    {
        // Null blocks damage - do nothing
    }
    
    public bool ShouldShowAttackMessage() => true;
    
    public string GetDamageMessage(Unit attacker, Unit target)
    {
        return null; // No damage message for null
    }
    
    public Unit GetAffectedUnit(Unit attacker, Unit target) => target;
}