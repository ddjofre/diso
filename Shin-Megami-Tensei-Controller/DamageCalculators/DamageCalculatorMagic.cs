using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.DamageCalculators;

public class DamageCalculatorMagic: BaseDamageCalculator
{
    public override int CalculateDamage(Unit attacker, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(attacker.stats.Mag * 54 * 0.0114 * affinity));
        attacker.damageRound = damage;
        
        return damage;
    }

    public override int CalculateDamageAbility(Unit attacker, int powerSkill, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(Math.Sqrt(attacker.stats.Mag * powerSkill) * affinity));
        attacker.damageRound = damage;
        return damage;
    }
}