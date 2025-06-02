using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
namespace Shin_Megami_Tensei.Actions.Affinities;

public abstract class BaseAffinity
{
    public abstract string nameAffinity { get; }
    public abstract double GetAffinity();
    public abstract void CalculateTurnEffect(Player player, TurnCalculator turnCalculator);
    public abstract string GetAffinityMessage(Unit target, Unit actualUnitPlaying);
    
    
}