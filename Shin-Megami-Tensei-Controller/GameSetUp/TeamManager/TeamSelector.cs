using Shin_Megami_Tensei_View;

namespace Shin_Megami_Tensei.GameSetUp.TeamManager;


public class TeamSelector
{
    private View _view;
    private string _teamsFolder;
    
    
    
    public TeamSelector(View view,string teamsFolder)
    {
        _view = view;
        _teamsFolder = teamsFolder;
    }
    
    
    private string[] GetTeamFiles()
    {
        string[] files = Directory.GetFiles(_teamsFolder, "*.txt");
        Array.Sort(files);
        return files;
    }

    private void ShowTeamOptions(string[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {
            _view.WriteLine($"{i}: {Path.GetFileName(files[i])}");
        }
       
    }

    private string[] GetUserOption(string[] files)
    {
        int userChoice = Convert.ToInt32((_view.ReadLine()));
        string teamsFile = files[userChoice];
        string[] userOption = File.ReadAllLines(teamsFile);
        
        return userOption;
        
    }
    
    
    public string[] ChoseTeams()
    {
        _view.WriteLine("Elige un archivo para cargar los equipos");
        string[] teamFiles = GetTeamFiles();
        ShowTeamOptions(teamFiles);
        return GetUserOption(teamFiles);
    }
}