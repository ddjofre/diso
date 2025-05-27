using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.AttacksExecutors;

namespace Shin_Megami_Tensei.Units.UnitComponents;

public class Skill
{
    private SkillInfo _skillInfo;
    public BasicAttackExecutor BasicAttackExecutor;

    public Skill(SkillInfo skillInfo, BasicAttackExecutor basicAttackExecutor)
    {
        _skillInfo = skillInfo;
        BasicAttackExecutor = basicAttackExecutor;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
    }
    
}