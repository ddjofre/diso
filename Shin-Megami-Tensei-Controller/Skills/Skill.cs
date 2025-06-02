using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;
using Shin_Megami_Tensei.GameComponents;

namespace Shin_Megami_Tensei.Skills;

public abstract class Skill
{
    private SkillInfo _skillInfo;
    public Skill(SkillInfo skillInfo)
    {
        _skillInfo = skillInfo;
    }

    public void DiscountMP(Unit attacker)
    {
        attacker.ActualMP -= _skillInfo.cost;
    }

    public int CalculateHits(Player player)
    {
        string hitsStr = _skillInfo.hits;
        int numOfTimeUsesHabilities = player.numOfTimeUsesHabilities;
        
        hitsStr = hitsStr.Trim();

        if (hitsStr.Contains("-"))
        {
            string[] parts = hitsStr.Split('-');

            if (!int.TryParse(parts[0].Trim(), out int A) || !int.TryParse(parts[1].Trim(), out int B))
            {
                throw new FormatException("A y B no son numers.");
            }

            int offset = numOfTimeUsesHabilities % (B - A + 1);
            return A + offset;
        }
        
        else
        {
            if (int.TryParse(hitsStr, out int singleHit))
            {
                return singleHit;
            }
            else
            {
                throw new FormatException("El formato del hit es inválido. Debe ser un número o un rango A-B.");
            }
        }
    }



    public abstract void Execute(Unit actualUnitPlaying, Player playerRival, Player player);
    
}