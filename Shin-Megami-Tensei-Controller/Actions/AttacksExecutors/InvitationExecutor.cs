using Shin_Megami_Tensei.Actions.Invocations;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

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
        _invoke = new Invoke(view);
    }

    public void Execute(Unit actualUnitPlaying, Player player)
    {
        // Mostrar tanto monstruos muertos de inGame como monstruos de reserva (vivos y muertos)
        var inGameUnits = new List<(Unit unit, int index)>();
        var reserveUnits = new List<(Unit unit, int index)>();
        int totalOptions = ShowMonstersForInvitation(player.Team, inGameUnits, reserveUnits);
        
        int userChoice = _invoke.GetUserInput();
        
        if (inGameUnits.Count == 0 && reserveUnits.Count == 0) // No hay unidades disponibles
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        if (userChoice == totalOptions) // Cancelar
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        _view.WriteLine("----------------------------------------");
        
        // Determinar qué unidad se seleccionó
        Unit unitToInvoke;
        int originalIndex;
        bool isFromReserve;
        
        if (userChoice <= inGameUnits.Count)
        {
            // Seleccionó de inGame
            var selected = inGameUnits[userChoice - 1];
            unitToInvoke = selected.unit;
            originalIndex = selected.index;
            isFromReserve = false;
        }
        else
        {
            // Seleccionó de reserva
            var selected = reserveUnits[userChoice - inGameUnits.Count - 1];
            unitToInvoke = selected.unit;
            originalIndex = selected.index;
            isFromReserve = true;
        }
        
        bool wasUnitDead = unitToInvoke.ActualHP == 0;
        
        // Mostrar posiciones disponibles (siguiendo el flujo del samurai)
        var positionMapping = ShowAvailablePositionsForInvokeMonster(player.Team);
        int positionChoice = _invoke.GetUserInput();
        
        if (positionChoice == positionMapping.Count + 1) // Cancelar
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }

        _view.WriteLine("----------------------------------------");
        
        // Obtener el índice real usando el mapeo
        int realPositionIndex = positionMapping[positionChoice - 1];
        
        // Realizar la invocación según el origen de la unidad
        if (isFromReserve)
        {
            // Invocar desde reserva (usar índice real de reserva)
            _invoke.MakeInvoke(player, originalIndex, realPositionIndex);
        }
        else
        {
            // Invocar desde inGame (monstruo muerto) - también permite elegir posición
            InvokeFromInGame(player, originalIndex, realPositionIndex, unitToInvoke);
        }
        
        // Ahora revivirla DESPUÉS de la invocación para que la unidad en UnitsInGame tenga el HP correcto
        Unit invokedUnit = player.Team.UnitsInGame[realPositionIndex];
        if (wasUnitDead)
        {
            invokedUnit.ActualHP = invokedUnit.stats.HP;
            //4invokedUnit.HasBeenRecarm = true;
            _view.WriteLine($"{actualUnitPlaying.name} revive a {invokedUnit.name}");
            _view.WriteLine($"{invokedUnit.name} recibe {invokedUnit.stats.HP} de HP");
            _view.WriteLine($"{invokedUnit.name} termina con HP:{invokedUnit.ActualHP}/{invokedUnit.stats.HP}");
        }
        
        
        _view.WriteLine("----------------------------------------");
        _turnCalculator.CalculateTurnAfterHeal(player);
        ShowTurnResults();
        _view.WriteLine("----------------------------------------");
    }

    private int ShowMonstersForInvitation(Team team, List<(Unit unit, int index)> inGameUnits, List<(Unit unit, int index)> reserveUnits)
    {
        _view.WriteLine("Seleccione un monstruo para invocar");
        int contador = 1;
        
        // Mostrar monstruos muertos de inGame (posiciones 1, 2, 3)
        for (int i = 1; i < 4; i++)
        {
            var unit = team.UnitsInGame[i];
            if (unit != null && unit.ActualHP == 0)
            {
                _view.WriteLine($"{contador}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                inGameUnits.Add((unit, i));
                contador++;
            }
        }
        
        // Mostrar monstruos de reserva (vivos y muertos)
        foreach (var unit in team.UnitsInReserve)
        {
            if (unit != null)
            {
                _view.WriteLine($"{contador}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                int reserveIndex = Array.IndexOf(team.UnitsInReserve, unit);
                reserveUnits.Add((unit, reserveIndex));
                contador++;
            }
        }
        
        _view.WriteLine($"{contador}-Cancelar");
        return contador;
    }

    private void InvokeFromInGame(Player player, int sourceIndex, int targetIndex, Unit unitToInvoke)
    {
        Team team = player.Team;
        Unit unitToReplace = team.UnitsInGame[targetIndex];
        
        // Marcar flags apropiados
        unitToInvoke.HasBeenIvoked = true;
        unitToReplace.HasBeenReplaceInInvoke = true;
        
        // Si estamos moviendo dentro del mismo campo de juego
        if (sourceIndex != targetIndex)
        {
            // Intercambiar posiciones
            team.UnitsInGame[targetIndex] = unitToInvoke;
            team.UnitsInGame[sourceIndex] = unitToReplace;
        }
        
        // Actualizar flags en las listas principales
        _invoke.ChangeParameterHasBeenInvokeInMonsterList(team, unitToInvoke);
        _invoke.ChangeParameterHasBeenReplaceInMonsterList(team, unitToReplace);
        
        // Si la unidad reemplazada tenía HP > 0 y no está en el orden, agregarla
        bool isEmptyPosition = (unitToReplace.ActualHP == 0);
        if (isEmptyPosition && !team.indexesOrderAttack.Contains(targetIndex))
        {
            team.indexesOrderAttack.Add(targetIndex);
        }
        
        _view.WriteLine($"{unitToInvoke.name} ha sido invocado"); 
    }

    private List<int> ShowAvailablePositionsForInvokeMonster(Team team)
    {
        _view.WriteLine("Seleccione una posición para invocar");
        Unit[] unitsInGame = team.UnitsInGame;
        List<int> positionMapping = new List<int>();
        int displayNumber = 1;
        
        for (int i = 1; i < 4; i++)
        {
            if (unitsInGame[i] == null || unitsInGame[i].ActualHP == 0)
            {
                _view.WriteLine($"{displayNumber}-Vacío (Puesto {i + 1})");
            }
            else
            {
                _view.WriteLine($"{displayNumber}-{unitsInGame[i].name} HP:{unitsInGame[i].ActualHP}/{unitsInGame[i].stats.HP} MP:{unitsInGame[i].ActualMP}/{unitsInGame[i].stats.MP} (Puesto {i + 1})");
            }
            positionMapping.Add(i);
            displayNumber++;
        }
        
        _view.WriteLine($"{displayNumber}-Cancelar");
        return positionMapping;
    }

    private void ShowTurnResults()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
    }
}