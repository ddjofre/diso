using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Invocations;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

public class SabbatmaExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly Invoke _invoke;
    private readonly ActionExecutor _actionsExecutor;

    public SabbatmaExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _invoke = new Invoke(view);
    }

    public void Execute(Unit actualUnitPlaying, Player player)
    {
        // Usar exactamente el mismo flujo que SamuraiActionManager2.PerformSummon()
        List<int> validIndexes = _invoke.ShowUnitsInReserve(player.Team);
        int userInput = _invoke.GetUserInput();
        
        if (validIndexes.Count == 0)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        _view.WriteLine("----------------------------------------");
        
        int realIndexToInvoke = _invoke.GetRealIndexFromUserInput(userInput, validIndexes);
        
        if (realIndexToInvoke == -1)
        {
            throw new OperationCanceledException();
        }

        _invoke.ShowAvailablePositionsForInvokeMonster(player.Team);
        int unitToRemoveFromBoard = _invoke.GetUserInput();
        _view.WriteLine("----------------------------------------");
    
        _invoke.MakeInvoke(player, realIndexToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
        
        _turnCalculator.CalculateTurnAfterHeal(player);
        ShowTurnResults();
    }

    private void ShowTurnResults()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
}