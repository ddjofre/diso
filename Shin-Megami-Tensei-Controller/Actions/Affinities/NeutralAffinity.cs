using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Affinities;

public class NeutralAffinity: BaseAffinity
{
    
    public override string nameAffinity => "-";

    public override double GetAffinity() => 1;
    

    public override void CalculateTurnEffect(Player player, TurnCalculator turnCalculator)
    {
        if (player.BlinkingTurns == 0)
        {
            player.FullTurns -= 1;
            turnCalculator.FullTurnsConsumed += 1;
        }
        else
        {
            player.BlinkingTurns -= 1;
            turnCalculator.BlinkingTurnsConsumed += 1;
        }
    }

    public override string GetAffinityMessage(Unit target, Unit actualUnitPlaying)
    {
        return null;
    }

}