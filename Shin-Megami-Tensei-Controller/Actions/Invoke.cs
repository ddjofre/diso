using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.GameSetUp.JSONManager;
namespace Shin_Megami_Tensei.Actions;

public class Invoke
{
    private View _view;
    private DamageCalculator _damageCalculator;
    private TurnCalculator _turnCalculator;
    private UnitManager _unitManager;
    
    public Invoke(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _damageCalculator = new DamageCalculator();
        _unitManager = new UnitManager(_view);
        
    }
    
    
    public void ShowAvailablePositionsForInvokeMonster(Team team)
    {
        _view.WriteLine("Seleccione una posición para invocar");
        Unit[] unitsInGame= team.UnitsInGame;
        
        for (int i = 1; i < 4; i++)
        {
            if (unitsInGame[i].ActualHP == 0)
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
    
    public void OrderUnitsInReserveByCanonicalMonsterList(Team team)
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
    
    public List<int> ShowUnitsInReserve(Team team)
{
    _view.WriteLine("Seleccione un monstruo para invocar");
    
    // Lista para mapear números mostrados -> índices reales
    List<int> validIndexes = new List<int>();
    int displayNumber = 1;
    
    for (int i = 0; i < team.UnitsInReserve.Length; i++)
    {
        var unit = team.UnitsInReserve[i];
        if (unit != null && unit.ActualHP > 0) // Solo monstruos vivos
        {
            _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
            validIndexes.Add(i); // Guardar el índice real
            displayNumber++;
        }
    }
   
    _view.WriteLine($"{displayNumber}-Cancelar");
    return validIndexes; // Devolver el mapeo
}

// Nuevo método para obtener el índice real basado en el input del usuario
public int GetRealIndexFromUserInput(int userInput, List<int> validIndexes)
{
    if (userInput >= 1 && userInput <= validIndexes.Count)
    {
        return validIndexes[userInput - 1]; // Convertir número mostrado a índice real
    }
    return -1; // Cancelar o input inválido
}

// Método MakeInvoke actualizado para usar el índice real
public void MakeInvoke(Player player, int realIndexMonsterToInvoke, int IndexMonsterToReplace)
{
    Team team = player.Team;
    Unit monsterToReplace = team.UnitsInGame[IndexMonsterToReplace];
    Unit monsterToInvoke = team.UnitsInReserve[realIndexMonsterToInvoke]; // Usar índice real

    // Verificar si es un puesto vacío ANTES de hacer el reemplazo
    bool isEmptyPosition = (monsterToReplace.ActualHP == 0);

    monsterToInvoke.HasBeenIvoked = true;
    monsterToReplace.HasBeenReplaceInInvoke = true;
    
    team.UnitsInGame[IndexMonsterToReplace] = monsterToInvoke;
    team.UnitsInReserve[realIndexMonsterToInvoke] = monsterToReplace; // Usar índice real
    
    ChangeParameterHasBeenInvokeInMonsterList(team, monsterToInvoke);
    ChangeParameterHasBeenReplaceInMonsterList(team, monsterToReplace);
    
    OrderUnitsInReserveByCanonicalMonsterList(team);
    
    // Si era un puesto vacío, agregar el índice al final del orden
    if (isEmptyPosition && !team.indexesOrderAttack.Contains(IndexMonsterToReplace))
    {
        team.indexesOrderAttack.Add(IndexMonsterToReplace);
    }
    
    _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
}

    public int GetActualUnitIndex(Team team, Unit actualUnitPlaying)
    {
        return Array.IndexOf(team.UnitsInGame, actualUnitPlaying);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}