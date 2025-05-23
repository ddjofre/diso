using Shin_Megami_Tensei.Actions.AttackTargetType;

namespace Shin_Megami_Tensei.Units.UnitComponents;

public class Skill
{
    private SkillInfo _skillInfo;
    public BaseTypeTargetAttack typeTargetAttack;

    public Skill(SkillInfo skillInfo, BaseTypeTargetAttack typeTargetAttack)
    {
        _skillInfo = skillInfo;
        this.typeTargetAttack = typeTargetAttack;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
    }
    
}