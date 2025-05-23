namespace Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;


public abstract class BaseAffinity
{
    public abstract string nameAffinity { get; }
    public abstract double GetAffinity();
    public abstract void CalculateTurnEffect(Player player, TurnCalculator turnCalculator);
    public abstract string GetAffinityMessage(Unit target, Unit actualUnitPlaying);
    
    //public abstract void ApplyEffect(Unit attacker, Unit target, int baseDamage);
    
}