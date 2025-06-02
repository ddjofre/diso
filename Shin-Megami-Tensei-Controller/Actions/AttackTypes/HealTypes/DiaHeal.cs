using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
public class DiaHeal : BaseHeal
{
    public DiaHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    public override void ApplyEffect(Unit caster, Unit target)
    {
        int healAmount = CalculateHealAmount(target);
        target.ActualHP = Math.Min(target.ActualHP + healAmount, target.stats.HP);
    }

    public override bool CanTargetUnit(Unit target)
    {
        return target.ActualHP > 0;
    }

    public override string GetEffectDescription(Unit caster, Unit target)
    {
        int healAmount = CalculateHealAmount(target);
        return $"{caster.name} cura a {target.name}\n{target.name} recibe {healAmount} de HP";
    }

    private int CalculateHealAmount(Unit target)
    {
        return (int)Math.Floor(target.stats.HP * (powerSkill / 100.0));
    }
}