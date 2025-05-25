using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions;

public class AttackExecutor
{
    private BaseAttack _typeAttack;
    private BaseTypeTarget _typeTarget;
    private TurnCalculator _turnCalculator;
    private View _view;
    
    
    public AttackExecutor(BaseAttack baseAttack, BaseTypeTarget baseTypeTarget, TurnCalculator turnCalculator, View view)
    {
        _typeAttack = baseAttack;
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
    
    private void MakeAttacks(List<int> indexUnitsToAttack, Unit actualUnitPlaying, Player rivalPlayer)
    {
        foreach (var index in indexUnitsToAttack)
        {
            Unit rival = GetRival(index, rivalPlayer);
            _view.WriteLine("----------------------------------------");
            _typeAttack.MakeAttack(actualUnitPlaying, rival, _typeAttack.typeAttack);
            
        }
    }
    
    
    private void ShowActionTurnResults()
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
    
    
    public void Execute(Unit target, Unit actualUnitPlying, Player player, Player rivalPlayer, List<int> indexTargets)
    {
        //List<int> indexTargets = _typeTarget.GetTargets(rivalPlayer);
        MakeAttacks(indexTargets, actualUnitPlying, rivalPlayer);
        _turnCalculator.CalculateTurnsAfterAttack(player, target, _typeAttack.typeAttack);
        ShowActionTurnResults();
    }
    
    
}