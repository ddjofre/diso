using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Invocations;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

public class InvitationExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly InvitationInvoke _invitationInvoke;

    public InvitationExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _invitationInvoke = new InvitationInvoke(view);
    }

    public void Execute(Unit actualUnitPlaying, Player player)
    {
        // Mostrar monstruos disponibles para invocar (tanto vivos como muertos)
        List<int> validIndexes = _invitationInvoke.ShowUnitsInReserve(player.Team);
        int userInput = _invitationInvoke.GetUserInput();
        
        if (validIndexes.Count == 0)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        _view.WriteLine("----------------------------------------");
        
        int realIndexToInvoke = _invitationInvoke.GetRealIndexFromUserInput(userInput, validIndexes);
        
        if (realIndexToInvoke == -1)
        {
            throw new OperationCanceledException();
        }

        // Mostrar posiciones disponibles para invocar
        _invitationInvoke.ShowAvailablePositionsForInvokeMonster(player.Team);
        int unitToRemoveFromBoard = _invitationInvoke.GetUserInput();
        _view.WriteLine("----------------------------------------");

        // Verificar si la unidad seleccionada está muerta para determinar el mensaje
        Unit unitToInvoke;
        bool isFromReserve = realIndexToInvoke >= 0;
        
        if (isFromReserve)
        {
            unitToInvoke = player.Team.UnitsInReserve[realIndexToInvoke];
        }
        else
        {
            int realSourceIndex = Math.Abs(realIndexToInvoke) - 1;
            unitToInvoke = player.Team.UnitsInGame[realSourceIndex];
        }
        
        bool wasUnitDead = unitToInvoke.ActualHP == 0;

        // Realizar la invocación
        _invitationInvoke.MakeInvoke(player, realIndexToInvoke, unitToRemoveFromBoard);
        
        // Mostrar mensaje apropiado según si la unidad estaba muerta o viva
        if (wasUnitDead)
        {
            _view.WriteLine($"{actualUnitPlaying.name} revive a {unitToInvoke.name}");
            _view.WriteLine($"{unitToInvoke.name} recibe {unitToInvoke.ActualHP} de HP");
            _view.WriteLine($"{unitToInvoke.name} termina con HP:{unitToInvoke.ActualHP}/{unitToInvoke.stats.HP}");
        }
        
        _view.WriteLine("----------------------------------------");
        
        // Calcular turnos como habilidad heal
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