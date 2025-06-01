using Shin_Megami_Tensei.GameComponents.PlayerComponents;

namespace Shin_Megami_Tensei.GameComponents;

public class Player
{
    public int PlayerId;
    public Team Team;
    public int FullTurns;
    public int BlinkingTurns;
    public int numOfTimeUsesHabilities;
    public Player(int playerId, Team team)
    {
        this.PlayerId = playerId;
        this.Team = team;
        this.BlinkingTurns = 0;
        this.numOfTimeUsesHabilities= 0;

    }
    
    
    
    
    
}