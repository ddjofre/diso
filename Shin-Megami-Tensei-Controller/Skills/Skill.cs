using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;
using Shin_Megami_Tensei.GameComponents;

namespace Shin_Megami_Tensei.Skills;

public abstract class Skill
{
    private SkillInfo _skillInfo;
    //public BasicAttackExecutor BasicAttackExecutor;

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
        Console.WriteLine($"{hitsStr}");
        int numOfTimeUsesHabilities = player.numOfTimeUsesHabilities;

        if (string.IsNullOrWhiteSpace(hitsStr))
        {
            throw new ArgumentException("El valor de hitsStr no puede estar vacío o en blanco.");
        }

        hitsStr = hitsStr.Trim();

        if (hitsStr.Contains("-"))
        {
            // Es un rango A-B
            string[] parts = hitsStr.Split('-');
            if (parts.Length != 2)
            {
                throw new FormatException("El formato del rango de hits es inválido. Debe ser A-B.");
            }

            if (!int.TryParse(parts[0].Trim(), out int A) || !int.TryParse(parts[1].Trim(), out int B))
            {
                throw new FormatException("El formato del rango de hits es inválido. A y B deben ser números.");
            }

            int offset = numOfTimeUsesHabilities % (B - A + 1);
            return A + offset;
        }
        else
        {
            // Es un solo número
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