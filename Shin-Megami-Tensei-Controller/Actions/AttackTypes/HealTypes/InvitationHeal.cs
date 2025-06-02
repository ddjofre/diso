namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;

using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

public class InvitationHeal : BaseHeal
{
    public InvitationHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    public override void ApplyEffect(Unit caster, Unit target)
    {
    }

    public override bool CanTargetUnit(Unit target)
    {
        return true;
    }

    public override string GetEffectDescription(Unit caster, Unit target)
    {
        return "";
    }
}
