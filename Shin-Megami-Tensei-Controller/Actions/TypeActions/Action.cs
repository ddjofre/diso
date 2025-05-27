using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions;

public abstract class Action
{
    private BaseAffinity _affinity;
    private BaseOffensive _typeOffensive;
    private BaseTypeTarget _typeTarget;
    private TurnCalculator _turnCalculator;
    private View _view;

    public Action(BaseAffinity affinity, BaseOffensive typeOffensive, BaseTypeTarget typeTarget, TurnCalculator turnCalculator, View view)
    {
        _affinity = affinity;
        _typeTarget = typeTarget;
        _typeOffensive = typeOffensive;
        _turnCalculator = turnCalculator;
        _view = view;
        
    }
    
    
    
    public List<int> ChooseTarget(Player playerRival)
    {
        return _typeTarget.GetTargets(playerRival);
    }
    
    
    public List<Unit> GetRivals(Player rival, int indexRival)
    {
        
        Unit rivalChosen = rival.Team.UnitsInGame[indexRival];
        _view.WriteLine("----------------------------------------");
        return new List<Unit>{rivalChosen};
        
    }
    
    
    
    public void Execute(Unit actualUnitPlaying, Unit target, Player player)
    {
        //_typeAttack.MakeAttack(actualUnitPlaying,target);
        _turnCalculator.CalculateTurnsAfterAttack(player, target, _typeOffensive.typeAttack);
        _typeOffensive.ShowActionResults(actualUnitPlaying, target);
    }
    
    
    
    
    
    
    private void SetLimitDamage(Unit target)
    {
        if (target.ActualHP <= 0)
        {
            target.ActualHP = 0;
        }
        
    }
    
    
    private void SetLimitAbsorbe(Unit target)
    {
        if (target.ActualHP > target.stats.HP)
        {
            target.ActualHP = target.stats.HP;
        }
    }












}