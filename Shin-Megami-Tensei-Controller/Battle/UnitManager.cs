using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle;

public class UnitManager
{
    private View _view;
    public UnitManager(View view)
    {
        _view = view;
    }
    
    public void SetUnitsInGame(Player player)
    {
        Team team = player.Team;

        team.UnitsInGame[0] = team.Samurai;
        
        for (int i = 1; i < 4; i++)
        {
            if (i <= team.Monsters.Count)
            {
                if (team.UnitsInGame[i] == null)
                {
                    team.UnitsInGame[i] = team.Monsters[i - 1];
                }
                else if (team.UnitsInGame[i] != null && !team.UnitsInGame[i].HasBeenIvoked)
                {
                    team.UnitsInGame[i] = team.Monsters[i - 1];
                }
                
            }
            
        }
        
    }
    

    public void SetUnitsInReserve(Player player)
    {
        Team team = player.Team;
        
        if (team.Monsters.Count > 3)
        {
            for (int i = 0; i + 3 < team.Monsters.Count; i++)
            {
                if (team.UnitsInReserve[i] == null)
                {
                    team.UnitsInReserve[i] = team.Monsters[3 + i];
                }
                else if (team.UnitsInReserve[i] != null && !team.UnitsInReserve[i].HasBeenReplaceInInvoke)
                {
                    team.UnitsInReserve[i] = team.Monsters[3 + i];
                }
                
            }
            
        }
        
        
    }
    
    
    private List<(int, int)> GetUnitsOrder(Team team)
    {
        List<(int, int)> order = new List<(int, int)>();

        foreach (var unit in team.UnitsInGame)
        {
            if (unit != null && unit.ActualHP != 0)
            {
                order.Add((unit.stats.Spd, Array.IndexOf(team.UnitsInGame, unit)));
            }
        }

        return order;
    }
    
    private List<int> SortUnitsBySpd(List<(int, int)> order)
    {
        order.Sort((a, b) => b.Item1.CompareTo(a.Item1));
        List<int> indices = new List<int>();
        order.ForEach(item => indices.Add(item.Item2));

        return indices;
    }

    public void SetOrderToUnits(Player player)
    {
        List<(int,int)> unitsOrder = GetUnitsOrder(player.Team);
        List<int> unitsOrderIndexes = SortUnitsBySpd(unitsOrder);
        
        player.Team.indexesOrderAttack = unitsOrderIndexes;

    }
    
    public Unit SetActualUnitPlaying(Player player)
    {
        Unit actualUnitPlaying = player.Team.UnitsInGame[player.Team.indexesOrderAttack[0]];
        int i = 0;
        while (actualUnitPlaying.ActualHP == 0)
        {
            actualUnitPlaying = player.Team.UnitsInGame[player.Team.indexesOrderAttack[i]];
            i++;
        }

        return actualUnitPlaying;

    }
    
    public void MoveDeadUnitsToReserve(Player player)
    {
        Team team = player.Team;

        for (int i = 1; i < 4; i++) // Solo posiciones 1, 2, 3 (monstruos, no el samurai en posición 0)
        {
            Unit unit = team.UnitsInGame[i];

            // Si hay una unidad en esta posición y está muerta
            if (unit != null && unit.ActualHP <= 0)
            {
                // Verificar si ya está en la reserva
                bool alreadyInReserve = false;
                foreach (Unit reserveUnit in team.UnitsInReserve)
                {
                    if (reserveUnit == unit)
                    {
                        alreadyInReserve = true;
                        break;
                    }
                }

                if (!alreadyInReserve)
                {
                    // Encontrar un slot vacío en reserva
                    for (int j = 0; j < team.UnitsInReserve.Length; j++)
                    {
                        if (team.UnitsInReserve[j] == null)
                        {
                            // Mover la unidad muerta a la reserva
                            team.UnitsInReserve[j] = unit;

                            // Eliminar la unidad del campo de juego
                            team.UnitsInGame[i] = null;

                            // Remover del orden de ataque si está presente
                            team.indexesOrderAttack.Remove(i);

                            // Salir del loop de reserva ya que encontramos un slot
                            break;
                        }
                    }
                }
                else
                {
                    // La unidad ya está en reserva, así que simplemente eliminamos del campo y del orden de ataque
                    team.UnitsInGame[i] = null;
                    team.indexesOrderAttack.Remove(i);
                }
            }
        }
    }

}