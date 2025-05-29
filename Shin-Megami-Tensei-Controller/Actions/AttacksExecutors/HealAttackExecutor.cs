using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

public class HealAttackExecutor
{
    private BaseHeal _typeHeal;
    private BaseTypeTarget _typeTarget;
    private TurnCalculator _turnCalculator;
    private View _view;
    
    
    public HealAttackExecutor(BaseHeal typeHeal, BaseTypeTarget baseTypeTarget, TurnCalculator turnCalculator, View view)
    {
        _typeHeal = typeHeal;
        _typeTarget = baseTypeTarget;
        _turnCalculator = turnCalculator;
        _view = view;

    }
    
    
    public List<int> GetTargets(Player playerRival)
    {
        return _typeTarget.GetTargets(playerRival);
    }
    
    public void ShowAvailableTargets(Player playerRival, Unit actualUnitPlaying)
    {
        _typeTarget.ShowAvailablesTargets(playerRival, actualUnitPlaying);
    }
    
    public Unit GetRival(int indexRival, Player rivalPlayer)
    {
        Unit rivalChosen = rivalPlayer.Team.UnitsInGame[indexRival];
        //_view.WriteLine("----------------------------------------");
        return rivalChosen;
    }
    
    private void MakeAttacks(List<int> indexUnitsToAttack, Player player, Unit actualUnitPlaying)
    {
        foreach (var index in indexUnitsToAttack)
        {
            Unit target = GetRival(index, player);
            _view.WriteLine("----------------------------------------");
            _typeHeal.ApplyHeal(target);
            _typeHeal.ShowActionResults(actualUnitPlaying, target);
        }
    }
    
    private void ShowActionTurnResults()
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
    
    public void Execute( Player player, List<int> indexTargets, Unit actualUnitPlaying)
    {
        MakeAttacks(indexTargets, player, actualUnitPlaying);
        _turnCalculator.CalculateTurnAfterHeal(player);
        ShowActionTurnResults();
    }
    
    
    
    
    
    
    

}