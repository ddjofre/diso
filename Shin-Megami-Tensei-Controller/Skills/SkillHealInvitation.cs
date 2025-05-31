namespace Shin_Megami_Tensei.Skills;

// Skills/SkillHealInvitation.cs
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

public class SkillHealInvitation : Skill
{
    private readonly SkillInfo _skillInfo;
    private readonly InvitationExecutor _invitationExecutor;

    public SkillHealInvitation(SkillInfo skillInfo, InvitationExecutor invitationExecutor) : base(skillInfo)
    {
        _skillInfo = skillInfo;
        _invitationExecutor = invitationExecutor;
    }

    public override void Execute(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        _invitationExecutor.Execute(actualUnitPlaying, player);
        DiscountMP(actualUnitPlaying);
    }
}