using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

public class BasicAttackExecutor
{
    private BaseOffensive _typeOffensive;
    private BaseTypeTarget _typeTarget;
    private TurnCalculator _turnCalculator;
    private View _view;
    
    
    public BasicAttackExecutor(BaseOffensive baseOffensive, BaseTypeTarget baseTypeTarget, TurnCalculator turnCalculator, View view)
    {
        _typeOffensive = baseOffensive;
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
        _view.WriteLine("----------------------------------------");
        return rivalChosen;
    }
    
    private void MakeAttacks(List<int> indexUnitsToAttack, Unit actualUnitPlaying, Player rivalPlayer)
    {
        foreach (var index in indexUnitsToAttack)
        {
            Unit rival = rivalPlayer.Team.UnitsInGame[index];
            //_view.WriteLine("----------------------------------------");
            _typeOffensive.MakeAttack(actualUnitPlaying, rival, _typeOffensive.typeAttack);
            
        }
    }
    
    public void ShowActionTurnResults(Player player, Unit target)
    {
        _view.WriteLine("----------------------------------------");
        _turnCalculator.CalculateTurnsAfterAttack(player, target, _typeOffensive.typeAttack);
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }

    public void ShowFinalHpMessage(Unit attacker, Unit target)
    {
        _typeOffensive.GetFinalHpMessage(attacker, target);
        //_view.WriteLine("----------------------------------------");
    }
    
    public void Execute(Unit actualUnitPlying, Player rivalPlayer, List<int> indexTargets)
    {
        
        MakeAttacks(indexTargets, actualUnitPlying, rivalPlayer);
        
        //_turnCalculator.CalculateTurnsAfterAttack(player, target, _typeOffensive.typeAttack);
        //ShowActionTurnResults();
    }
    
    public void MakeAttackMultiHits(List<int> indexUnitsToAttack, Unit actualUnitPlaying, Player rivalPlayer)
    {
        foreach (var index in indexUnitsToAttack)
        {
            Unit rival = GetRival(index, rivalPlayer);
            _typeOffensive.MakeAttack(actualUnitPlaying, rival, _typeOffensive.typeAttack);
            
        }
    }
    
    
}