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
    private  TurnCalculator _turnCalculator;
    public bool isAttackInHabilitie;
    public int powerSkill;
    public TypeAttack typeAttack;
    private IAttackEffectHandler _currentEffectHandler;
    

    public BaseOffensive(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
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
    
    private void GetFinalHpMessage(Unit attacker, Unit target)
    {
        var affectedUnit = _currentEffectHandler.GetAffectedUnit(attacker, target);
        _view.WriteLine($"{affectedUnit.name} termina con HP:{affectedUnit.ActualHP}/{affectedUnit.stats.HP}");
    }
    
    //cambiar los get en este nomrbe si es que no es get cuando cambies a view 
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
        
        // Get and store the effect handler
        var effectFactory = new AttackEffectFactory();
        _currentEffectHandler = effectFactory.GetEffectHandler(affinityCode);
        
        // Calculate damage
        int damage = GetDamageAttack(attacker, target, powerSkill, typeAttack);
        
        // Apply effect
        _currentEffectHandler.ApplyEffect(attacker, target, damage);
        
        /*// Show attack message if needed
        if (_currentEffectHandler.ShouldShowAttackMessage())
        {
            GetAttackMessage(attacker, target);
        }
        */
        
        ShowActionResults(attacker, target);
    }
    
    
    public void ShowActionResults(Unit actualUnitPlaying, Unit target)
    {
        GetAttackMessage(actualUnitPlaying, target);
        GetAffinityMessage(actualUnitPlaying, target);
        GetDamageMessage(actualUnitPlaying, target);
        GetFinalHpMessage(actualUnitPlaying, target);
        
        /*
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
        */
    }
    
    protected abstract void GetAttackMessage(Unit attacker, Unit target);
    
    protected abstract string GetAffinity(Unit target);


}

    