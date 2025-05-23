using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.GameSetUp.JSONManager;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.GameSetUp.TeamManager;

public class TeamCreator
{
    private JsonHandler _jsonHandler;

    public TeamCreator()
    {
        _jsonHandler = new JsonHandler();
    }
    
    public Team CreateTeam(TeamParser teamParser)
    {
        List<Unit> team = new List<Unit>();
        List<Unit> monsters = _jsonHandler.JsonDeserializerUnits("data/monsters.json");
        List<Unit> samurais = _jsonHandler.JsonDeserializerUnits("data/samurai.json");
        List<SkillInfo> skills = _jsonHandler.JsonDeserializerSkill("data/skills.json");
        
        foreach (var monster in teamParser._monsters)
        {
            foreach (var character in monsters)
            {
                if (monster.Equals(character.name))
                {
                    character.ActualHP = character.stats.HP;
                    character.ActualMP = character.stats.MP;
                    character.type = TypeUnits.monster;
                    team.Add(character);
                }
            }
            
        }


        foreach (var character in samurais)
        {
            foreach (var samurai in teamParser._samurais)
            {
                if (samurai.Key.Equals(character.name))
                {
                    character.ActualHP = character.stats.HP;
                    character.ActualMP = character.stats.MP;
                    character.type = TypeUnits.samurai;
                    character.skills = samurai.Value;
                    team.Add(character);
                }
            }
        }

        
        AddSkills(skills, team);
        
        return new Team(team);
        
    }


    public void AddSkills(List<SkillInfo> skills, List<Unit> team)
    {
        
        foreach (var unit in team)
        {
            if (unit.skills != null && unit.skills.Length > 0)
            {
                foreach (var habilitie in unit.skills)
                {
                    foreach (var skill in skills)
                    {
                        if (skill.name == habilitie)
                        {
                            unit.skillInfo.Add(skill);         
                        } 
                    }
                    
                }
            }
            
        }
        
        
    }
}