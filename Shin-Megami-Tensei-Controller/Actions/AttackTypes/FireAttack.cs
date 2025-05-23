using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;
namespace Shin_Megami_Tensei.Actions.AttackTypes;

public class FireAttack: BaseAttack
{
    private View _view;
    private DamageCalculator _damageCalculator;
    private TurnCalculator _turnCalculator;
    
    
    public FireAttack(View view, TurnCalculator turnCalculator): base(view, turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _damageCalculator = new DamageCalculator();
        typeAttack = TypeAttack.Fire;

    }
    
    
    protected override int GetDamageAttack(Unit attacker, Unit target, int _powerSkill)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        if (isAttackInHabilitie)
        {
            return damageCalculator.CalculateDamageMagicHability(attacker, _powerSkill, GetAffinity(target));            
        }
        else
        {
            return damageCalculator.CalculateDamageMagic(attacker, GetAffinity(target));
        }
        
        
    }
    
    protected override void GetAttackMessage(Unit attacker, Unit target)
    {
        _view.WriteLine($"{attacker.name} lanza fuego a {target.name}");
    }
    
    
    protected override string GetAffinity(Unit target)
    {
        return target.affinity.Fire;
    }
    
    
    
}