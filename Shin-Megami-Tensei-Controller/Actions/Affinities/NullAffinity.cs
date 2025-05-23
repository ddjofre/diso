using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Affinities;

public class NullAffinity: BaseAffinity
{
    public override string nameAffinity => "Nu";

    public override double GetAffinity() => 0;
    
    public override string GetAffinityMessage(Unit target, Unit actualUnitPlaying)
    {
        return $"{target.name} bloquea el ataque de {actualUnitPlaying.name}";
    }
    
    public override void CalculateTurnEffect(Player player, TurnCalculator turnCalculator)
    {
        if (player.BlinkingTurns == 0 && player.FullTurns <= 1)
        {
            player.FullTurns -= 1;
            turnCalculator.FullTurnsConsumed += 1;
        }
            
        else if (player.BlinkingTurns < 2)
        {
            int missingTurns = 2 - player.BlinkingTurns;
            player.FullTurns -= missingTurns;
            turnCalculator.FullTurnsConsumed += missingTurns;
            turnCalculator.BlinkingTurnsConsumed += player.BlinkingTurns;
            player.BlinkingTurns = 0;
        }
        else
        {
            player.BlinkingTurns -= 2;
            turnCalculator.BlinkingTurnsConsumed += 2;
        }
    }
    
    
    
    
    
    
}