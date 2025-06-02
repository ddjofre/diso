using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.GameSetUp.JSONManager;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Invocations;

public class Invoke
{
    private View _view;
    public Invoke(View view)
    {
        _view = view;
    }
    
    public void ShowAvailablePositionsForInvokeMonster(Team team)
    {
        _view.WriteLine("Seleccione una posición para invocar");
        Unit[] unitsInGame= team.UnitsInGame;
        
        for (int i = 1; i < 4; i++)
        {
            if (unitsInGame[i] == null)
            {
                _view.WriteLine($"{i}-Vacío (Puesto {i + 1})");
            }
            else
            {
                _view.WriteLine($"{i}-{unitsInGame[i].name} HP:{unitsInGame[i].ActualHP}/{unitsInGame[i].stats.HP} MP:{unitsInGame[i].ActualMP}/{unitsInGame[i].stats.MP} (Puesto {i + 1})");
            }
        }
        _view.WriteLine($"4-Cancelar");
    }

    public int GetUserInput()
    {
        return Convert.ToInt32(_view.ReadLine());
    }

    public void OrderUnitsInReserveByOrderMonsterList(Team team)
    {
        JsonHandler jsonHandler = new JsonHandler();
        List<Unit> monsters = jsonHandler.JsonDeserializerUnits("data/monsters.json");
        
        var reservaOrdenada = team.UnitsInReserve
            .Where(m => m != null)
            .OrderBy(m => monsters.FindIndex(mon => mon.name == m.name))
            .ToArray();

        for (int i = 0; i < team.UnitsInReserve.Length; i++)
        {
            if (i < reservaOrdenada.Length)
                team.UnitsInReserve[i] = reservaOrdenada[i];
            else
                team.UnitsInReserve[i] = null;
        }
    }

    public void ChangeParameterHasBeenInvokeInMonsterList(Team team, Unit monster)
    {
        foreach (var unit in team.Monsters)
        {
            if (monster.name == unit.name)
            {
                unit.HasBeenIvoked = true;
            }
        }
    }

    public void ChangeParameterHasBeenReplaceInMonsterList(Team team, Unit monster)
    {
        foreach (var unit in team.Monsters)
        {
            if (monster.name == unit.name)
            {
                unit.HasBeenReplaceInInvoke = true;
            }
        }
    }

    public virtual List<int> ShowUnitsInReserve(Team team)
    {
        _view.WriteLine("Seleccione un monstruo para invocar");
        List<int> validIndexes = new List<int>();
        int displayNumber = 1;

        for (int i = 0; i < team.UnitsInReserve.Length; i++)
        {
            var unit = team.UnitsInReserve[i];
            if (unit != null && MeetsConditionToBeenInvoke(unit))
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                validIndexes.Add(i);
                displayNumber++;
            }
        }
        _view.WriteLine($"{displayNumber}-Cancelar");
        return validIndexes;
    }

    public int GetRealIndexFromUserInput(int userInput, List<int> validIndexes)
    {
        if (userInput >= 1 && userInput <= validIndexes.Count)
        {
            return validIndexes[userInput - 1];
        }
        return -1;
    }

    public int GetActualUnitIndex(Team team, Unit actualUnitPlaying)
    {
        return Array.IndexOf(team.UnitsInGame, actualUnitPlaying);
    }

    public virtual bool MeetsConditionToBeenInvoke(Unit unit)
    {
        return unit.ActualHP > 0;
    }
    
    
    protected bool IsEmptyPosition(Unit unit)
    {
        return unit == null || unit.ActualHP == 0;
    }

    private void UpdateUnitsForInvoke(Team team, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Unit monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke];
        Unit monsterToReplace = team.UnitsInGame[indexMonsterToReplace];

        team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
        team.UnitsInReserve[indexMonsterToInvoke] = monsterToReplace;
    }

    protected void UpdateMonsterStatus(Team team, Unit monsterToInvoke, Unit monsterToReplace)
    {
        monsterToInvoke.HasBeenIvoked = true;
        ChangeParameterHasBeenInvokeInMonsterList(team, monsterToInvoke);
        if (monsterToReplace != null)
        {
            monsterToReplace.HasBeenReplaceInInvoke = true;
            ChangeParameterHasBeenReplaceInMonsterList(team, monsterToReplace);
        }
    }

    protected void UpdateAttackOrder(Team team, int indexMonsterToReplace)
    {
        if (!team.indexesOrderAttack.Contains(indexMonsterToReplace))
        {
            team.indexesOrderAttack.Add(indexMonsterToReplace);
        }
    }
    
    public virtual void MakeInvoke(Player player, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Team team = player.Team;
        Unit monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke];
        Unit monsterToReplace = team.UnitsInGame[indexMonsterToReplace];

        bool emptyPosition = IsEmptyPosition(monsterToReplace);

        UpdateUnitsForInvoke(team, indexMonsterToInvoke, indexMonsterToReplace);
        UpdateMonsterStatus(team, monsterToInvoke, monsterToReplace);
        OrderUnitsInReserveByOrderMonsterList(team);

        if (emptyPosition)
        {
            UpdateAttackOrder(team, indexMonsterToReplace);
        }

        _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
    }
}
