using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
public abstract class BaseHeal
{
    protected readonly View _view;
    protected readonly TurnCalculator _turnCalculator;
    public int powerSkill { get; set; }

    protected BaseHeal(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        powerSkill = 0;
    }

    public abstract void ApplyEffect(Unit caster, Unit target);
    public abstract bool CanTargetUnit(Unit target);
    public abstract string GetEffectDescription(Unit caster, Unit target);
}