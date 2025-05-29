using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;
using Shin_Megami_Tensei.GameComponents;

namespace Shin_Megami_Tensei.Skills;

public abstract class Skill
{
    private SkillInfo _skillInfo;
    //public BasicAttackExecutor BasicAttackExecutor;

    public Skill(SkillInfo skillInfo)
    {
        _skillInfo = skillInfo;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
        
    }

    public abstract void Execute(Unit actualUnitPlaying, Player playerRival, Player player);
}