using Shin_Megami_Tensei.GameSetUp.JSONManager;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.GameComponents.PlayerComponents;

public class Team
{
    public List<Unit> team;
    public Unit Samurai;
    public List<Unit> Monsters;
    public Unit[] UnitsInGame;
    public Unit[] UnitsInReserve;
    public List<int> indexesOrderAttack;
    public bool NeedsOrderRecalculation;

    
    public Team(List<Unit> team)
    {
        this.team = team;
        this.Samurai = this.team[^1];
        this.Monsters = team.GetRange(0, team.Count - 1);
        this.UnitsInGame = new Unit[4];
        this.UnitsInReserve = new Unit[4];
        this.indexesOrderAttack = new List<int>();
        this.NeedsOrderRecalculation = false;
        
    }
}