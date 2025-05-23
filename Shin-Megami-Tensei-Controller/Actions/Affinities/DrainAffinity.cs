using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Affinities;

public class DrainAffinity: BaseAffinity
{
    public override string nameAffinity => "Dr";

    public override double GetAffinity() => 1;
    

    public override void CalculateTurnEffect(Player player, TurnCalculator turnCalculator)
    {
        turnCalculator.BlinkingTurnsConsumed = player.BlinkingTurns;
        turnCalculator.FullTurnsConsumed = player.FullTurns;

        player.BlinkingTurns = 0;
        player.FullTurns = 0;
    }

    
    public override string GetAffinityMessage(Unit target, Unit actualUnitPlaying)
    {
        return $"{target.name} absorbe {actualUnitPlaying.damageRound} daño";
    }
}