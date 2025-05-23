using Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.DamageCalculators;

public abstract class BaseDamageCalculator
{
    protected double CalculateEffectAffinity(string affinityCombat)
    {
        AffinityFactory affinityFactory = new AffinityFactory();
        BaseAffinity affinity = affinityFactory.GetAffinity(affinityCombat);
        return affinity.GetAffinity();
    }

    public abstract int CalculateDamage(Unit attacker, string affinityCombat);
    
    public abstract int CalculateDamageAbility(Unit attacker,int powerSkill, string affinityCombat);


}