using Shin_Megami_Tensei.Actions;

namespace Shin_Megami_Tensei.Units.UnitComponents;

public class Skill
{
    private SkillInfo _skillInfo;
    public AttackExecutor _attackExecutor;

    public Skill(SkillInfo skillInfo, AttackExecutor attackExecutor)
    {
        _skillInfo = skillInfo;
        _attackExecutor = attackExecutor;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
    }
    
}