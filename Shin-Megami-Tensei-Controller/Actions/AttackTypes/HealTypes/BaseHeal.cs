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
    
    private void GetFinalHpMessage(Unit target)
    {
        _view.WriteLine($"{target.name} termina con HP:{target.ActualHP}/{target.stats.HP}");
    }
    
    public void ShowActionResults(Unit attacker, Unit target)
    {
        GetHealMessage(attacker, target);
        //GetAffinityMessage(actualUnitPlaying, target);
        //GetDamageMessage(actualUnitPlaying, target);
        GetFinalHpMessage(target);
        
    }
    public abstract void ApplyHeal(Unit target);
    public abstract bool CanTargetUnit(Unit target);
    public abstract void GetHealMessage(Unit attacker, Unit target);
    
   
}