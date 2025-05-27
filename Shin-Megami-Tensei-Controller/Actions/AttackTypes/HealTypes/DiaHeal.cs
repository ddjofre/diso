using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;

public class DiaHeal : BaseHeal
{
    public DiaHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    private int CalculateHealAmount(Unit target)
    {
        int healAmount = (int)Math.Floor(target.stats.HP * (powerSkill / 100.0));
        return healAmount;
    }
    
    public override void ApplyHeal(Unit target)
    {
        // Heal X% of max HP where X = powerSkill
        int healAmount = CalculateHealAmount(target);
        target.ActualHP += healAmount;
        
        // Cap at max HP
        if (target.ActualHP > target.stats.HP)
            target.ActualHP = target.stats.HP;
    }
    
    public override bool CanTargetUnit(Unit target)
    {
        return target.ActualHP > 0;
    }
    
    public override void GetHealMessage(Unit attacker, Unit target)
    {
        int healAmount = CalculateHealAmount(target);
        _view.WriteLine($"{attacker.name} cura a {target.name}");
        _view.WriteLine($"{target.name} recibe {healAmount} de HP");
    }
    
    
}