using Shin_Megami_Tensei.GameComponents.PlayerComponents;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

// Actions/SpecialExecutors/SabbatmaExecutor.cs
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;


public class SabbatmaExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly Invoke _invoke;

    public SabbatmaExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _invoke = new Invoke(view, turnCalculator);
    }

    public void Execute(Unit actualUnitPlaying, Player player)
    {
        // Mostrar solo monstruos vivos de la reserva
        int numOptions = ShowAliveMonstersInReserve(player.Team);
        
        if (numOptions == 1) 
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        int monsterChoice = _invoke.GetUserInput();
        
        if (monsterChoice == numOptions)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        _view.WriteLine("----------------------------------------");
        
        // Mostrar posiciones disponibles
        _invoke.ShowAvailablePositionsForInvokeMonster(player.Team);
        int positionChoice = _invoke.GetUserInput();
        
        if (positionChoice == 4) // Cancelar
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        _view.WriteLine("----------------------------------------");
        
        // Obtener el índice real del monstruo en UnitsInReserve
        int realIndex = GetRealIndexOfAliveMonster(player.Team, monsterChoice - 1);
        
        // Realizar la invocación (esto maneja los parámetros HasBeenInvoked y HasBeenReplaceInInvoke)
        _invoke.MakeInvoke(player, realIndex + 1, positionChoice);
        
        _view.WriteLine("----------------------------------------");
        _turnCalculator.CalculateTurnAfterHeal(player);
        ShowTurnResults();
    }

    private int ShowAliveMonstersInReserve(Team team)
    {
        _view.WriteLine("Seleccione un monstruo para invocar");
        int contador = 1;
        
        foreach (var unit in team.UnitsInReserve)
        {
            if (unit != null && unit.ActualHP > 0)
            {
                _view.WriteLine($"{contador}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                contador++;
            }
        }
        
        _view.WriteLine($"{contador}-Cancelar");
        return contador;
    }

    private int GetRealIndexOfAliveMonster(Team team, int visibleIndex)
    {
        int currentVisibleIndex = 0;
        
        for (int i = 0; i < team.UnitsInReserve.Length; i++)
        {
            if (team.UnitsInReserve[i] != null && team.UnitsInReserve[i].ActualHP > 0)
            {
                if (currentVisibleIndex == visibleIndex)
                {
                    return i;
                }
                currentVisibleIndex++;
            }
        }
        
        return -1;
    }

    private void ShowTurnResults()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
}