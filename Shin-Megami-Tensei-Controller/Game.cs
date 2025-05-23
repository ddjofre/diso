using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameSetUp;
using Shin_Megami_Tensei.GameSetUp.TeamManager;

namespace Shin_Megami_Tensei;

public class Game
{
    
    private View _view;
    private GameSetter _gameSetter;
    private TeamSelector _teamSelector;
    
    private Player _PlayerOne;
    private Player _PlayerTwo;
    public Game(View view, string teamsFolder)
    {
        _view = view;
        _teamSelector = new TeamSelector(_view, teamsFolder);
        _gameSetter = new GameSetter(_view);
        
    }
    
    public void Play()
    {
        string[] teamSelected = _teamSelector.ChoseTeams();
        _PlayerOne = _gameSetter.CreatePlayer(teamSelected, 1);
        _PlayerTwo = _gameSetter.CreatePlayer(teamSelected, 2);
        
        
        if (_gameSetter.AreBothTeamsValid(_PlayerOne, _PlayerTwo))
        {
            bool gameWinner = false;
            int turn = 1;
            Battle.Battle battle = new Battle.Battle(_view);
            
            while (gameWinner != true)
            {
                if (turn % 2 == 1)
                {
                    battle.SetInitialsParametersToBattle(_PlayerOne, _PlayerTwo, turn);
                    battle.MakeTurns(_PlayerOne, _PlayerTwo);
                    gameWinner = battle.CheckForWinner(_PlayerOne, _PlayerTwo);
                }

                else
                {
                    battle.SetInitialsParametersToBattle(_PlayerTwo, _PlayerOne, turn);
                    battle.MakeTurns(_PlayerTwo, _PlayerOne);
                    gameWinner = battle.CheckForWinner(_PlayerTwo, _PlayerOne);
                    
                }
                
                turn += 1;
            }
            
        }
        
        
    }
}