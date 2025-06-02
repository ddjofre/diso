using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackEffects;

public interface IAttackEffectHandler
{
    void ApplyEffect(Unit attacker, Unit target, int damage);
    bool ShouldShowAttackMessage();
    string GetDamageMessage(Unit attacker, Unit target);
    Unit GetAffectedUnit(Unit attacker, Unit target);
}
