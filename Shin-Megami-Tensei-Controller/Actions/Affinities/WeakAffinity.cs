using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Affinities;

public class WeakAffinity: BaseAffinity
{
    
    public override string nameAffinity => "Wk";
    
    public override double GetAffinity() => 1.5;
    

    public override void CalculateTurnEffect(Player player, TurnCalculator turnCalculator)
    {
        if (player.FullTurns == 0)
        {
            player.BlinkingTurns -= 1;
            turnCalculator.BlinkingTurnsConsumed += 1;
        }
        else
        {
            player.FullTurns -= 1;
            player.BlinkingTurns += 1;
            turnCalculator.FullTurnsConsumed += 1;
            turnCalculator.BlinkingTurnsObtained += 1;
        }
    }

    public override string GetAffinityMessage(Unit target, Unit actualUnitPlaying)
    {
        return $"{target.name} es débil contra el ataque de {actualUnitPlaying.name}";
    }
    
}
    