﻿using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public class DrainAttackEffect : IAttackEffectHandler
{
    public void ApplyEffect(Unit attacker, Unit target, int damage)
    {
        target.ActualHP += damage;
        if (target.ActualHP > target.stats.HP) 
            target.ActualHP = target.stats.HP;
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