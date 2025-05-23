using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.GameSetUp.TeamManager;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.GameSetUp;

public class GameSetter
{
    private View _view;
    

    public GameSetter(View view)
    {
        _view = view;
    }
    
    private Team CreateTeam(string[] teamSelected, int player)
    {
        TeamParser teamParser = new TeamParser(teamSelected);
        
        if (player == 1)
            teamParser.CreateTeamPlayerOne();
        else
            teamParser.CreateTeamPlayerTwo();

        TeamCreator teamCreator = new TeamCreator();
        Team team = teamCreator.CreateTeam(teamParser);

        return team;
    }

    public Player CreatePlayer(string[] teamSelected, int idPlayer)
    {
        Team teamPlayer = CreateTeam(teamSelected, idPlayer);
        Player player = new Player(idPlayer, teamPlayer);
        
    return player;
    }
    
    private bool ValidateTeams(Player player)
    {
        bool isValidPlayer = new TeamValidator(player.Team).Validate();
        return isValidPlayer;
        
    }
    
    public bool AreBothTeamsValid(Player player1, Player player2)
    {
        if (ValidateTeams(player1) && ValidateTeams(player2))
        {
            return true;
        }

        _view.WriteLine("Archivo de equipos inválido");
        return false;
        
    }
    
    
    
    
    
    
    

    
    
    

    

 
    
    
    
    
    
    
    
}