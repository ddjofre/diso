using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;

public abstract class BaseHeal
{
    protected View _view;
    protected TurnCalculator _turnCalculator;
    public int powerSkill;
    
    protected BaseHeal(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        powerSkill = 0;
    }
    
    public abstract void ApplyHeal(Unit target);
    public abstract bool CanTargetUnit(Unit target);
    public abstract void GetHealMessage(Unit target, Unit attacker);
    
    
}