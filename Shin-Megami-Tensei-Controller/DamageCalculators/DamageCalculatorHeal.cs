using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.DamageCalculators;



public class DamageCalculatorHeal: BaseDamageCalculator
{
    
    public override int CalculateDamage(Unit attacker, string affinityCombat)
    {
        throw new NotImplementedException();
    }


    public override int CalculateDamageAbility(Unit attacker, int powerSkill, string affinityCombat)
    {
        int heal = powerSkill * attacker.stats.HP;
        return heal;
        
    }
    

}