using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;
namespace Shin_Megami_Tensei.Actions.AttackTypes;

public class GunAttack: BaseAttack
{
    private View _view;
    private DamageCalculator _damageCalculator;
    private TurnCalculator _turnCalculator;
    
    
    public GunAttack(View view, TurnCalculator turnCalculator): base(view, turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _damageCalculator = new DamageCalculator();
        typeAttack = TypeAttack.Gun;

    }
    
    protected override int GetDamageAttack(Unit attacker, Unit target, int _powerSkill)
    {
        DamageCalculator damageCalculator = new DamageCalculator();

        if (isAttackInHabilitie)
        {
            return damageCalculator.CalculateDamageGunHability(attacker, _powerSkill, GetAffinity(target));            
        }
        else
        {
            return damageCalculator.CalculateDamageGun(attacker, GetAffinity(target));
        }
        
        
    }
    
    protected override void GetAttackMessage(Unit attacker, Unit target)
    {
        _view.WriteLine($"{attacker.name} dispara a {target.name}");
    }
    
    
    protected override string GetAffinity(Unit target)
    {
        return target.affinity.Gun;
    }
    
   
    
}