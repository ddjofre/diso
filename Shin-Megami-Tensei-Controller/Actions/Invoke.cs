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
    private Battle.DamageCalculator _damageCalculator;
    private TurnCalculator _turnCalculator;
    
    public Invoke(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _damageCalculator = new DamageCalculator();
        
    }

    public int ShowUnitsInReserve(Team team)
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


    public void OrderUnitsInReserve(Team team)
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
    
    
    public void MakeInvoke(Team team, int indexMonsterToInvoke, int IndexMonsterToReplace)
    {
        
        Unit monsterToReplace = team.UnitsInGame[IndexMonsterToReplace];
        Unit monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke - 1];

        monsterToInvoke.HasBeenIvoked = true;
        monsterToReplace.HasBeenReplaceInInvoke = true;
        
        team.UnitsInGame[IndexMonsterToReplace] = monsterToInvoke;
        team.UnitsInReserve[indexMonsterToInvoke - 1] = monsterToReplace;
        
        ChangeParameterHasBeenInvokeInMonsterList(team, monsterToInvoke);
        ChangeParameterHasBeenReplaceInMonsterList(team, monsterToReplace);
        
        OrderUnitsInReserve(team);
        
        
        _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
        
        
    }

    public int GetActualUnitIndex(Team team, Unit actualUnitPlaying)
    {
        return Array.IndexOf(team.UnitsInGame, actualUnitPlaying);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}