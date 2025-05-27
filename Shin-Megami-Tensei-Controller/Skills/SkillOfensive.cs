using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Skills;

public class SkillOfensive: Skill
{
    private SkillInfo _skillInfo;
    public BasicAttackExecutor BasicAttackExecutor;

    public SkillOfensive(SkillInfo skillInfo, BasicAttackExecutor basicAttackExecutor): base(skillInfo)
    {
        _skillInfo = skillInfo;
        BasicAttackExecutor = basicAttackExecutor;
    }
    
}