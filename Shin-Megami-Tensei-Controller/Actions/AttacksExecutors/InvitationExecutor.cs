using Shin_Megami_Tensei.GameComponents.PlayerComponents;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

// Actions/AttacksExecutors/InvitationExecutor.cs
// Actions/AttacksExecutors/InvitationExecutor.cs
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using System;
using System.Collections.Generic;
using System.Linq;


public class InvitationExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly Invoke _invoke;

    public InvitationExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _invoke = new Invoke(view, turnCalculator);
    }

    public void Execute(Unit actualUnitPlaying, Player player)
    {
        // Mostrar monstruos de la reserva (vivos y muertos)
        int numOptions = ShowMonstersInReserve(player.Team);
        
        if (numOptions == 1) // Solo cancelar
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        int monsterChoice = _invoke.GetUserInput();
        
        if (monsterChoice == numOptions) // Cancelar
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
        
        // Obtener el monstruo antes de invocarlo
        Unit monsterToInvoke = GetMonsterFromReserve(player.Team, monsterChoice - 1);
        bool wasMonsterDead = monsterToInvoke.ActualHP == 0;
        
        // Realizar la invocación (esto ya maneja los parámetros HasBeenInvoked y HasBeenReplaceInInvoke)
        _invoke.MakeInvoke(player, monsterChoice, positionChoice);
        
        // Si el monstruo estaba muerto, revivirlo y mostrar mensajes adicionales
        if (wasMonsterDead)
        {
            _view.WriteLine($"{actualUnitPlaying.name} revive a {monsterToInvoke.name}");
            monsterToInvoke.ActualHP = monsterToInvoke.stats.HP;
            monsterToInvoke.HasBeenRecarm = true; // Marcar que fue revivido
            _view.WriteLine($"{monsterToInvoke.name} recibe {monsterToInvoke.stats.HP} de HP");
            _view.WriteLine($"{monsterToInvoke.name} termina con HP:{monsterToInvoke.ActualHP}/{monsterToInvoke.stats.HP}");
        }
        
        _view.WriteLine("----------------------------------------");
        _turnCalculator.CalculateTurnAfterHeal(player);
        ShowTurnResults();
    }

    private int ShowMonstersInReserve(Team team)
    {
        _view.WriteLine("Seleccione un monstruo para invocar");
        int contador = 1;
        
        foreach (var unit in team.UnitsInReserve)
        {
            if (unit != null)
            {
                _view.WriteLine($"{contador}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                contador++;
            }
        }
        
        _view.WriteLine($"{contador}-Cancelar");
        return contador;
    }

    private Unit GetMonsterFromReserve(Team team, int index)
    {
        int currentIndex = 0;
        foreach (var unit in team.UnitsInReserve)
        {
            if (unit != null)
            {
                if (currentIndex == index)
                    return unit;
                currentIndex++;
            }
        }
        return null;
    }

    private void ShowTurnResults()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
    }
}