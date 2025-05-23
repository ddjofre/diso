namespace Shin_Megami_Tensei.GameSetUp.TeamManager;

public class TeamParser
{
    private int _playerOneIndex;
    private int _playerTwoIndex;
    public Dictionary<string, string[]> _samurais;
    public List<string> _monsters;
    private string[] _teams;

    public TeamParser(string[] teams)
    {
        _teams = teams;
    }
    
    
    private void ParseTeams()
    {
        for (int i = 0; i < _teams.Length; i++)
        {
            if (_teams[i].Contains("Player 1 Team"))
            {
                _playerOneIndex = i + 1;
            }
            else if (_teams[i].Contains("Player 2 Team"))
            {
                _playerTwoIndex = i + 1;
            }
        }

    }
    
    string[] GetTeamArray(string[] array, int startIndex, int endIndex)
    {
        int length = endIndex - startIndex + 1;
        string[] subArray = new string[length];
        
        Array.Copy(array, startIndex, subArray, 0, length);
        
        return subArray;
    }
    
    private void ProcessSamurai(string line)
    {
        string nameSamurai = line.Split(']')[1].Split('(')[0].Trim();
        string[] habilitiesSamurai = ExtractSamuraiHabilities(line);
        _samurais[nameSamurai] = habilitiesSamurai;
    }
    
    private string[] ExtractSamuraiHabilities(string line)
    {
        if (line.Contains("("))
        {
            return line.Split('(')[1].Split(')')[0].Split(",");
        }

        return new string[] { };
    }
    
    public void ParseLine(int startIndex, int endIndex)
    {
        string[] equipoData = GetTeamArray(_teams, startIndex, endIndex);
    
        _samurais = new Dictionary<string, string[]> { };
        _monsters = new List<string> { };

        foreach (var line in equipoData)
        {
            if (line.Contains("[Samurai]"))
            {
                ProcessSamurai(line);
            }
            else
            {
                _monsters.Add(line);
            }
        }
        
        
    }

    public void CreateTeamPlayerOne()
    {
        ParseTeams();
        ParseLine(_playerOneIndex, _playerTwoIndex - 2);  
    }
    
    public void CreateTeamPlayerTwo()
    {
        ParseTeams();
        ParseLine(_playerTwoIndex, _teams.Length - 1 );
    }
    
    
    
    
}