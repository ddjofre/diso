using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;

public class RecarmHeal: BaseHeal
{
    
    public RecarmHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    private int CalculateHealAmount(Unit target)
    {
        int hp = Convert.ToInt32(target.stats.HP * (powerSkill / 100));
        return hp;
    }
    
    public override void ApplyHeal(Unit target)
    {
        target.ActualHP = CalculateHealAmount(target);
    }
    
    public override bool CanTargetUnit(Unit target)
    {
        return target.ActualHP == 0;
    }


    public override void GetHealMessage(Unit target, Unit attacker)
    {
        int healAmount = CalculateHealAmount(target);
        _view.WriteLine($"{attacker.name} revive a {target.name}");
        _view.WriteLine($"{target.name} recibe {healAmount} de HP");
    }
    
    
}