using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.GameSetUp.JSONManager;
using System.Text.Json;

public class JsonHandler
{
    public List<Unit> JsonDeserializerUnits(string path)
    {
        string myJson = File.ReadAllText (path) ;
        var units = JsonSerializer.Deserialize <List<Unit>>(myJson) ;
        return units;
    }
    
    public List<SkillInfo> JsonDeserializerSkill(string path)
    {
        string myJson = File.ReadAllText (path) ;
        var skills = JsonSerializer.Deserialize <List<SkillInfo>>(myJson) ;
        return skills;
    }
    
}