using Shin_Megami_Tensei.Actions.AttackTargetType;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions;

public class FinalSkill
{
    private SkillInfo _skillInfo;
    public AttackExecutor _attackExecutor;

    public FinalSkill(SkillInfo skillInfo, AttackExecutor attackExecutor)
    {
        _skillInfo = skillInfo;
        _attackExecutor = attackExecutor;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
    }
    
}