using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Skills;

public class SkillHeal : Skill
{
    private readonly SkillInfo _skillInfo;
    private readonly HealExecutor _healExecutor;

    public SkillHeal(SkillInfo skillInfo, HealExecutor healExecutor) : base(skillInfo)
    {
        _skillInfo = skillInfo;
        _healExecutor = healExecutor;
    }

    public override void Execute(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        _healExecutor.Execute(actualUnitPlaying, player);
        DiscountMP(actualUnitPlaying);
    }
}