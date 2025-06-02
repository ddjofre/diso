using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
public class RecarmHeal : BaseHeal
{
    public RecarmHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    public override void ApplyEffect(Unit caster, Unit target)
    {
        int reviveHP = (int)Math.Floor(target.stats.HP * (powerSkill / 100.0));
        target.ActualHP = reviveHP;
        target.HasBeenRecarm = true;
    }

    public override bool CanTargetUnit(Unit target)
    {
        return target.ActualHP == 0;
    }

    public override string GetEffectDescription(Unit caster, Unit target)
    {
        int reviveHP = (int)Math.Floor(target.stats.HP * (powerSkill / 100.0));
        return $"{caster.name} revive a {target.name}\n{target.name} recibe {reviveHP} de HP";
    }
}