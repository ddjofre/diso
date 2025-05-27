using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;
using Shin_Megami_Tensei.Enumerates;

namespace Shin_Megami_Tensei.Battle;

public class TurnCalculator
{
    
    public int BlinkingTurnsConsumed = 0;
    public int FullTurnsConsumed = 0;
    public int BlinkingTurnsObtained = 0;
    
    
    private void SetLimit(int turns)
    {
        if (turns < 0)
        {
            turns = 0;
        }
    }

    public void CalculateInitialTurns(Player player)
    {
        foreach (var unit in player.Team.UnitsInGame)
        {
            if (unit!= null && unit.ActualHP > 0 )
            {
                player.FullTurns += 1;
            }
        }
    }
    
    
    public void CalculateTurnsAfterAttack(Player player, Unit target, TypeAttack attackType)
    {
        
        Affinity affinities = target.affinity;
        
        string affinityValue = attackType switch
        {
            TypeAttack.Phys => affinities.Phys,
            TypeAttack.Gun => affinities.Gun,
            TypeAttack.Fire => affinities.Fire,
            TypeAttack.Ice => affinities.Ice,
            TypeAttack.Elec => affinities.Elec,
            TypeAttack.Force => affinities.Force,
            _ => "-"
        };

        
        if (affinityValue == "-" || affinityValue == "Rs")
        {
            if (player.BlinkingTurns == 0)
            {
                player.FullTurns -= 1;
                FullTurnsConsumed += 1;
            }
            else
            {
                player.BlinkingTurns -= 1;
                BlinkingTurnsConsumed += 1;
            }
        }
        else if (affinityValue == "Wk")
        {
            if (player.FullTurns == 0)
            {
                player.BlinkingTurns -= 1;
                BlinkingTurnsConsumed += 1;
            }
            else
            {
                player.FullTurns -= 1;
                player.BlinkingTurns += 1;
                FullTurnsConsumed += 1;
                BlinkingTurnsObtained += 1;
            }
        }
        else if (affinityValue == "Miss")
        {
            if (player.BlinkingTurns == 0)
            {
                player.FullTurns -= 1;
                FullTurnsConsumed += 1;
            }
            else
            {
                player.BlinkingTurns -= 1;
                BlinkingTurnsConsumed += 1;
            }
        }
        else if (affinityValue == "Nu")
        {
            if (player.BlinkingTurns == 0 && player.FullTurns <= 1)
            {
                player.FullTurns -= 1;
                FullTurnsConsumed += 1;
            }
            
            else if (player.BlinkingTurns < 2)
            {
                int missingTurns = 2 - player.BlinkingTurns;
                player.FullTurns -= missingTurns;
                FullTurnsConsumed += missingTurns;
                BlinkingTurnsConsumed += player.BlinkingTurns;
                player.BlinkingTurns = 0;
            }
            else
            {
                player.BlinkingTurns -= 2;
                BlinkingTurnsConsumed += 2;
            }
        }
        else if (affinityValue == "Rp" || affinityValue == "Dr")
        {
            BlinkingTurnsConsumed = player.BlinkingTurns;
            FullTurnsConsumed = player.FullTurns;

            player.BlinkingTurns = 0;
            player.FullTurns = 0;
        }

        SetLimit(player.FullTurns);
        SetLimit(player.BlinkingTurns);
    }
    
    public void CalculateTurnAfterSummonOrPass(Player player)
    {
        
        if (player.BlinkingTurns == 0)
        {
            FullTurnsConsumed += 1;
            BlinkingTurnsObtained += 1;
            
            player.FullTurns -= 1;
            player.BlinkingTurns += 1;
            
        }
        else
        {
            player.BlinkingTurns -= 1;
            BlinkingTurnsConsumed += 1;
        }
    }
    
    public void CalculateTurnAfterHeal(Player player)
    {
        // Non-offensive skills consume turns like this
        if (player.BlinkingTurns == 0)
        {
            player.FullTurns -= 1;
            FullTurnsConsumed += 1;
        }
        else
        {
            player.BlinkingTurns -= 1;
            BlinkingTurnsConsumed += 1;
        }
    }
    
    public void ResetCalculator()
    {
        BlinkingTurnsConsumed = 0;
        FullTurnsConsumed = 0;
        BlinkingTurnsObtained = 0;
    }
    
    
    
}