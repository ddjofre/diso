using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Skills;

public class SkillOfensive: Skill
{
    private SkillInfo _skillInfo;
    public BasicAttackExecutor basicAttackExecutor;

    public SkillOfensive(SkillInfo skillInfo, BasicAttackExecutor basicAttackExecutor): base(skillInfo)
    {
        _skillInfo = skillInfo;
        this.basicAttackExecutor = basicAttackExecutor;
    }

    public override void Execute(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        basicAttackExecutor.ShowAvailableTargets(playerRival, actualUnitPlaying);
        List<int> targetsIndexes = basicAttackExecutor.GetTargets(playerRival);
        
        if (targetsIndexes[0] == -1)
        {
            throw new OperationCanceledException();
        }
                
        Unit target = basicAttackExecutor.GetRival(targetsIndexes[0], playerRival);

        int numOfHits = CalculateHits(player);
        
        for (int i = 0; i < numOfHits; i++)
        {
            basicAttackExecutor.Execute(actualUnitPlaying, playerRival, targetsIndexes);
        }
        
        basicAttackExecutor.ShowFinalHpMessage(actualUnitPlaying, target);
        
        basicAttackExecutor.ShowActionTurnResults(player, target);
        
        DiscountMP(actualUnitPlaying);
    }
    
    
}