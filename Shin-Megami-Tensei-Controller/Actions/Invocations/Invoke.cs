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
    
    public virtual List<int> ShowUnitsInReserve(Team team) {
        
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
    
    public virtual void MakeInvoke(Player player, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Team team = player.Team;
        Unit monsterToReplace = team.UnitsInGame[indexMonsterToReplace];
        Unit monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke];
    
        Console.WriteLine($"=== MakeInvoke for Player {player.PlayerId} ===");
        Console.WriteLine($"Invoking: {monsterToInvoke.name} from Reserve[{indexMonsterToInvoke}]");
        Console.WriteLine($"Replacing: {(monsterToReplace?.name ?? "null")} at Game[{indexMonsterToReplace}]");
    
        bool isEmptyPosition = (monsterToReplace?.ActualHP == 0 || monsterToReplace == null);
        Console.WriteLine($"Is empty position: {isEmptyPosition}");

        monsterToInvoke.HasBeenIvoked = true;
        if (monsterToReplace != null)
        {
            monsterToReplace.HasBeenReplaceInInvoke = true;
        }
    
        // INTERCAMBIO
        team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
        team.UnitsInReserve[indexMonsterToInvoke] = monsterToReplace;
    
        Console.WriteLine($"After swap:");
        Console.WriteLine($"  Game[{indexMonsterToReplace}] = {team.UnitsInGame[indexMonsterToReplace]?.name ?? "null"}");
        Console.WriteLine($"  Reserve[{indexMonsterToInvoke}] = {team.UnitsInReserve[indexMonsterToInvoke]?.name ?? "null"}");
    
        ChangeParameterHasBeenInvokeInMonsterList(team, monsterToInvoke);
        if (monsterToReplace != null)
        {
            ChangeParameterHasBeenReplaceInMonsterList(team, monsterToReplace);
        }
    
        OrderUnitsInReserveByOrderMonsterList(team);
    
        // Si era un puesto vacío, agregar el índice al final del orden
        if (isEmptyPosition && !team.indexesOrderAttack.Contains(indexMonsterToReplace))
        {
            team.indexesOrderAttack.Add(indexMonsterToReplace);
            Console.WriteLine($"Added index {indexMonsterToReplace} to attack order");
        }
    
        _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
        Console.WriteLine("=== End MakeInvoke ===\n");
    }

    public int GetActualUnitIndex(Team team, Unit actualUnitPlaying)
    {
        return Array.IndexOf(team.UnitsInGame, actualUnitPlaying);
    }

    public virtual bool MeetsConditionToBeenInvoke(Unit unit)
    {
        return unit.ActualHP > 0;
    }
    
    
    
    // Agregar este método a UnitManager o Battle
public void CheckForDuplicates(Player player, string context)
{
    Console.WriteLine($"=== DUPLICATE CHECK: {context} - Player {player.PlayerId} ===");
    
    Dictionary<Unit, List<string>> unitLocations = new Dictionary<Unit, List<string>>();
    
    // Revisar UnitsInGame
    for (int i = 0; i < player.Team.UnitsInGame.Length; i++)
    {
        Unit unit = player.Team.UnitsInGame[i];
        if (unit != null)
        {
            if (!unitLocations.ContainsKey(unit))
                unitLocations[unit] = new List<string>();
            unitLocations[unit].Add($"Game[{i}]");
        }
    }
    
    // Revisar UnitsInReserve
    for (int i = 0; i < player.Team.UnitsInReserve.Length; i++)
    {
        Unit unit = player.Team.UnitsInReserve[i];
        if (unit != null)
        {
            if (!unitLocations.ContainsKey(unit))
                unitLocations[unit] = new List<string>();
            unitLocations[unit].Add($"Reserve[{i}]");
        }
    }
    
    // Revisar lista principal Monsters
    for (int i = 0; i < player.Team.Monsters.Count; i++)
    {
        Unit unit = player.Team.Monsters[i];
        if (!unitLocations.ContainsKey(unit))
            unitLocations[unit] = new List<string>();
        unitLocations[unit].Add($"Monsters[{i}]");
    }
    
    // Reportar duplicados
    bool foundDuplicates = false;
    foreach (var kvp in unitLocations)
    {
        if (kvp.Value.Count > 1)
        {
            Console.WriteLine($"DUPLICATE: {kvp.Key.name} (HP: {kvp.Key.ActualHP}) found in: {string.Join(", ", kvp.Value)}");
            foundDuplicates = true;
        }
    }
    
    if (!foundDuplicates)
    {
        Console.WriteLine("No duplicates found.");
    }
    
    Console.WriteLine("=== END DUPLICATE CHECK ===\n");
}
    
    
    
 
    
    
    
}