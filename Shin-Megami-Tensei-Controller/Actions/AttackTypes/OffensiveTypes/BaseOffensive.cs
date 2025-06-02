using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei.Actions.AttackEffects;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.DamageCalculators;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;
namespace Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;

public abstract class BaseOffensive
{
    private  View _view;
    public bool isAttackInHabilitie;
    public int powerSkill;
    public TypeAttack typeAttack;
    private IAttackEffectHandler _currentEffectHandler;
    

    public BaseOffensive(View view)
    {
        _view = view;
        isAttackInHabilitie = false;
        powerSkill = 0;

    }
    
    private void GetAffinityMessage(Unit actualUnitPlaying, Unit target)
    {
        AffinityFactory affinityFactory = new AffinityFactory();
        BaseAffinity affinity = affinityFactory.GetAffinity(GetAffinity(target));
        var message = affinity.GetAffinityMessage(target, actualUnitPlaying);

        if (!string.IsNullOrEmpty(message))
        {
            _view.WriteLine(message);
        }
    }
    
    private int GetDamageAttack(Unit attacker, Unit target, int _powerSkill, TypeAttack typeAttack)
    {
        DamageCalculatorFactory factory = new DamageCalculatorFactory();
        BaseDamageCalculator damageCalculator = factory.GetDamageCalculator(typeAttack);

        if (isAttackInHabilitie)
        {
            return damageCalculator.CalculateDamageAbility(attacker, _powerSkill, GetAffinity(target));            
        }
        
        return damageCalculator.CalculateDamage(attacker, GetAffinity(target));
        
    }
    
    public void GetFinalHpMessage(Unit attacker, Unit target)
    {
        var affectedUnit = _currentEffectHandler.GetAffectedUnit(attacker, target);
        _view.WriteLine($"{affectedUnit.name} termina con HP:{affectedUnit.ActualHP}/{affectedUnit.stats.HP}");
    }
    
    private void GetDamageMessage(Unit attacker, Unit target)
    {
        var message = _currentEffectHandler.GetDamageMessage(attacker, target);
        if (!string.IsNullOrEmpty(message))
        {
            _view.WriteLine(message);
        }
    }
    
    public void MakeAttack(Unit attacker, Unit target, TypeAttack typeAttack)
    {
        string affinityCode = GetAffinity(target);
        var effectFactory = new AttackEffectFactory();
        _currentEffectHandler = effectFactory.GetEffectHandler(affinityCode);
        
        int damage = GetDamageAttack(attacker, target, powerSkill, typeAttack);
        _currentEffectHandler.ApplyEffect(attacker, target, damage);
        
        ShowActionResults(attacker, target);
    }
    
    public void ShowActionResults(Unit actualUnitPlaying, Unit target)
    {
        GetAttackMessage(actualUnitPlaying, target);
        GetAffinityMessage(actualUnitPlaying, target);
        GetDamageMessage(actualUnitPlaying, target);
    }
    
    protected abstract void GetAttackMessage(Unit attacker, Unit target);
    
    protected abstract string GetAffinity(Unit target);


}

    