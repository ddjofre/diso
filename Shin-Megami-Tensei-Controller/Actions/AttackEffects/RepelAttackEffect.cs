using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class RepelAttackEffect : IAttackEffectHandler
{
    public void ApplyEffect(Unit attacker, Unit target, int damage)
    {
        attacker.ActualHP -= damage;
        if (attacker.ActualHP < 0) attacker.ActualHP = 0;
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
        return attacker;
    }
}