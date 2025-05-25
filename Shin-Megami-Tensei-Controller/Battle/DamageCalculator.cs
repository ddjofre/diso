using Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle;

public class DamageCalculator
{

    public double CalculateEffectAffinityOld(string affinity)
    {
        double effectAffinity = 0;

        if (affinity.Equals("-"))
        {
            effectAffinity = 1;
        }
        else if(affinity.Equals("Wk"))
        {
            effectAffinity = 1.5;
        }
        else if (affinity.Equals("Rs"))
        {
            effectAffinity = 0.5;
        }
        
        else if (affinity.Equals("Nu"))
        {
            effectAffinity = 0;
        }
        else if (affinity.Equals("Rp"))
        {
            effectAffinity = 1;
        }
        else if (affinity.Equals("Dr"))
        {
            effectAffinity = 1;
        }

        return effectAffinity;

    }
    
    
    private double CalculateEffectAffinity(string affinityCombat)
    {
        AffinityFactory affinityFactory = new AffinityFactory();
        BaseAffinity affinity = affinityFactory.GetAffinity(affinityCombat);
        return affinity.GetAffinity();
    }
    
    
    public int CalculateDamagePhys(Unit attacker, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(attacker.stats.Str * 54 * 0.0114 * affinity));
        attacker.damageRound = damage;
        
        return damage;
    }
    public int CalculateDamageGun(Unit attacker, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(attacker.stats.Skl * 80 * 0.0114 * affinity));
        attacker.damageRound = damage;
        
        return damage;
    }
    
    public int CalculateDamageMagic(Unit attacker, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(attacker.stats.Mag * 54 * 0.0114 * affinity));
        attacker.damageRound = damage;
        
        return damage;
    }
    
    public int CalculateDamagePhysHability(Unit attacker,int powerSkill, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(Math.Sqrt(attacker.stats.Str * powerSkill) * affinity));
        attacker.damageRound = damage;
        return damage;
    }
    
    public int CalculateDamageGunHability(Unit attacker,int powerSkill, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(Math.Sqrt(attacker.stats.Skl * powerSkill) * affinity));
        attacker.damageRound = damage;
        return damage;
    }
    
    public int CalculateDamageMagicHability(Unit attacker,int powerSkill, string affinityCombat)
    {
        double affinity = CalculateEffectAffinity(affinityCombat);
        int damage = Convert.ToInt32(Math.Floor(Math.Sqrt(attacker.stats.Mag * powerSkill) * affinity));
        attacker.damageRound = damage;
        return damage;
    }
    
    
    
    
    
    
}