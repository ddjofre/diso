using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle;

public class BattleInfoDisplay
{
    private View _view;

    public BattleInfoDisplay(View view)
    {
        _view = view;
    }
    
    public void ShowInfoRound(Player playerPlaying, Unit actualUnitPlaying, int turn)
    {
        if (turn == 1)
        {
            _view.WriteLine("----------------------------------------");
            _view.WriteLine($"Ronda de {playerPlaying.Team.Samurai.name} (J{playerPlaying.PlayerId})");
            _view.WriteLine("----------------------------------------");
        }
        else
        {
            _view.WriteLine($"Ronda de {playerPlaying.Team.Samurai.name} (J{playerPlaying.PlayerId})");
            _view.WriteLine("----------------------------------------");
        }

    }
    
    public void ShowPlayerInformation(Player player)
    {
        Team team = player.Team;
        Unit samurai = team.Samurai;
        
        _view.WriteLine($"Equipo de {samurai.name} (J{player.PlayerId})");
        
        _view.WriteLine($"A-{samurai.name} HP:{samurai.ActualHP}/{samurai.stats.HP} MP:{samurai.ActualMP}/{samurai.stats.MP}");
        
        char[] positions = new char[3] { 'B', 'C', 'D' };

        for (int i = 0; i < 3; i++)
        {
            if(team.UnitsInGame[i+1] != null && team.UnitsInGame[i+1].ActualHP!=0 && team.UnitsInGame[i+1].HasBeenRecarm!= true)
            {
                Unit monster = team.UnitsInGame[i + 1];
                _view.WriteLine($"{positions[i]}-{monster.name} HP:{monster.ActualHP}/{monster.stats.HP} MP:{monster.ActualMP}/{monster.stats.MP}");
            }
            else
            {
                _view.WriteLine($"{positions[i]}-");
            }
        }
        
    }
    
    public void ShowTurnsPLayer(Player player)
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Full Turns: {player.FullTurns}");
        _view.WriteLine($"Blinking Turns: {player.BlinkingTurns}");
        _view.WriteLine("----------------------------------------");
        
    }
    
    public void ShowOrderUnitsPlay(Player player)
    {
        _view.WriteLine($"Orden:");
        List<string> nameUnits = GetNameUnitsReadyToPlay(player.Team);

        for (int i = 0; i < nameUnits.Count(); i++)
        {
            _view.WriteLine($"{i + 1}-{nameUnits[i]}");   
        }
        
        _view.WriteLine("----------------------------------------");
        
    }

    public void AnnounceWinner(int idPlayer, Unit samurai)
    {
        _view.WriteLine($"Ganador: {samurai.name} (J{idPlayer})");
    }

    public void Surrender(Unit actualUnit, int playerId)
    {
        _view.WriteLine($"{actualUnit.name} (J{playerId}) se rinde");
        _view.WriteLine("----------------------------------------");

    }
    
    private List<string> GetNameUnitsReadyToPlay(Team team)
    {
        List<string> nameUnits = new List<string>();

        int f = 0;
        foreach (var coso in team.indexesOrderAttack)
        {
            Console.WriteLine($"INDEXES N°{f}: {coso}" );
            f++;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < team.indexesOrderAttack.Count)
            {
                Console.WriteLine($"1________________actual spd de {team.UnitsInGame[team.indexesOrderAttack[i]].name}: {team.UnitsInGame[team.indexesOrderAttack[i]].stats.Spd}");
            }
            
            if (i < team.indexesOrderAttack.Count && team.UnitsInGame[team.indexesOrderAttack[i]].ActualHP != 0)
            {
                Console.WriteLine($"2________________actual spd de {team.UnitsInGame[team.indexesOrderAttack[i]].name}: {team.UnitsInGame[team.indexesOrderAttack[i]].stats.Spd}");
                nameUnits.Add(team.UnitsInGame[team.indexesOrderAttack[i]].name);
            }
        }

        return nameUnits;
    }
    
    
}