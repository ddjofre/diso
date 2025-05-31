using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.SkillExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Skills;

// Skills/SkillSpecial.cs
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;


public class SkillSpecial : Skill
{
    private readonly SkillInfo _skillInfo;
    private readonly SabbatmaExecutor _sabbatmaExecutor;

    public SkillSpecial(SkillInfo skillInfo, SabbatmaExecutor sabbatmaExecutor) : base(skillInfo)
    {
        _skillInfo = skillInfo;
        _sabbatmaExecutor = sabbatmaExecutor;
    }

    public override void Execute(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        _sabbatmaExecutor.Execute(actualUnitPlaying, player);
        DiscountMP(actualUnitPlaying);
    }
}