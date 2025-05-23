using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Units;

public class Unit
{
    public string name { get; set; }
    public Stats stats { get; set; }
    public Affinity affinity { get; set; }
    public string[] skills { get; set; }

    public List<SkillInfo> skillInfo = new List<SkillInfo>();
    
    public TypeUnits type;
    
    public int ActualHP;
    public int ActualMP;
    public int damageRound;
    public bool DoesSurrender = false;
    public bool HasBeenIvoked = false;
    public bool HasBeenReplaceInInvoke = false;
    
}
