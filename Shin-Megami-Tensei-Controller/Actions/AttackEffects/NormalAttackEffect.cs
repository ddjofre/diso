using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class NormalAttackEffect : IAttackEffectHandler
{
    public void ApplyEffect(Unit attacker, Unit target, int damage)
    {
        target.ActualHP -= damage;
        if (target.ActualHP < 0) target.ActualHP = 0;
    }
    
    public bool ShouldShowAttackMessage()
    {
        return true;
    }

    public string GetDamageMessage(Unit attacker, Unit target)
    {
        return $"{target.name} recibe {attacker.damageRound} de daño";
    }
    
    public Unit GetAffectedUnit(Unit attacker, Unit target)
    {
        return target;
    }
}