using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Skills;


public class SkillHeal : Skill
{
    private SkillInfo _skillInfo;
    public HealAttackExecutor healAttackExecutor;

    public SkillHeal(SkillInfo skillInfo, HealAttackExecutor healAttackExecutor) : base(skillInfo)
    {
        _skillInfo = skillInfo;
        this.healAttackExecutor = healAttackExecutor;
    }

    public override void Execute(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        healAttackExecutor.ShowAvailableTargets(player, actualUnitPlaying);
        
        List<int> targetsIndexes = healAttackExecutor.GetTargets(player);
        
        healAttackExecutor.Execute(player, targetsIndexes, actualUnitPlaying);
        
        DiscountMP(actualUnitPlaying);
    }
    
    
}