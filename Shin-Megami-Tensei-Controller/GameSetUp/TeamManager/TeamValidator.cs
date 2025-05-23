using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Unit = Shin_Megami_Tensei.Units.Unit;

namespace Shin_Megami_Tensei.GameSetUp.TeamManager;

public class TeamValidator
{
    private Team _team;

    public TeamValidator(Team team)
    {
        _team = team;
    }



    private bool DoesHaveExactlyOneSamurai()
    {
        int countSamurais = 0;

        foreach (var unit in _team.team)
        {
            if (unit.type.Equals(TypeUnits.samurai))
            {
                countSamurais++;
            }
        }
        return countSamurais == 1;
    }
    
    private bool HasRequiredNumberOfMonsters()
    {
        int countMonsters = 0;

        foreach (var unit in _team.team)
        {
            if (unit.type.Equals(TypeUnits.monster))
            {
                countMonsters++;
            }
        }

        return countMonsters <= 7;
    }
    
    private bool AreMonstersAllDifferent()
    {
        
        HashSet<Unit> uniqueMonsters = new HashSet<Unit>();
        int totalMonsters = 0;

        foreach (var unit in _team.team)
        {
            if (unit.type.Equals(TypeUnits.monster))
            {
                uniqueMonsters.Add(unit);
                totalMonsters++;
            }
        }
        

        return uniqueMonsters.Count == totalMonsters;
    }
    
    private bool HasSamuraisUniqueSkills()
    {
        bool AllSamuraisHasUniqueSkills = true;
        
        int totalSkills = 0;
        HashSet<string> uniqueSkills = new HashSet<string>();

        foreach (var unit in _team.team)
        {
            if (unit.type.Equals(TypeUnits.samurai))
            {
                foreach (var skill in unit.skills)
                {
                    uniqueSkills.Add(skill);
                    totalSkills++;
                }
            }

            if (uniqueSkills.Count != totalSkills)
            {
                AllSamuraisHasUniqueSkills = false;
            }

            totalSkills = 0;
            uniqueSkills = new HashSet<string>();
        }
        
        return AllSamuraisHasUniqueSkills;
    }
    
    private bool HasSamuraiRequiredNumberOfSkills()
    {
        bool hasSamuraiRequiredNumberOfSkills = true;

        foreach (var unit in _team.team)
        {
            if (!HasUnitEightOrLessSkills(unit))
            {
                return false;
            }
        }

        return true;
    }

    private bool HasUnitEightOrLessSkills(Unit unit)
    {
        if (unit.type.Equals(TypeUnits.samurai))
        {
            if (unit.skills.Length > 8)
            {
                return false;
            }

        }
        return true;
    }
    
    public bool Validate()
    {
        return DoesHaveExactlyOneSamurai() &&
               HasRequiredNumberOfMonsters() &&
               AreMonstersAllDifferent() &&
               HasSamuraisUniqueSkills()&& HasSamuraiRequiredNumberOfSkills();
        
        
    }
    
    
}