using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Actions.SkillExecutors;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions;
/*
public class HealSkillExecutor: SkillExecutor
{
    private BaseHeal _healType;
    private BaseTypeTarget _targetType;
    private TurnCalculator _turnCalculator;
    private View _view;
    

    public HealSkillExecutor(BaseHeal healType, BaseTypeTarget targetType,
        TurnCalculator turnCalculator, View view): base(view, turnCalculator)
    {
        _healType = healType;
        _targetType = targetType;
        _turnCalculator = turnCalculator;
        _view = view;
    }
    
    
    //override
    public override void ExecuteSkill(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        _skill._attackExecutor.ShowAvailableTargets(playerRival, actualUnitPlaying);
        List<int> targetsIndexes = _skill._attackExecutor.GetTargets(playerRival);
                
        Unit target = _skill._attackExecutor.GetRival(targetsIndexes[0], playerRival);
        
        _skill._attackExecutor.Execute( target, actualUnitPlaying, player, playerRival, targetsIndexes);
        _skill.DiscountMP(actualUnitPlaying);
        
    }

    
    
    
    
    
    
    
    
    
    
    

}

*/