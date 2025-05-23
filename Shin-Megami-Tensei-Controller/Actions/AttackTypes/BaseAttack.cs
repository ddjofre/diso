using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Affinities;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.DamageCalculators;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;


namespace Shin_Megami_Tensei.Actions.AttackTypes;


public abstract class BaseAttack
{
    private  View _view;
    private  TurnCalculator _turnCalculator;
    public bool isAttackInHabilitie;
    public int powerSkill;
    public TypeAttack typeAttack;
    

    public BaseAttack(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        isAttackInHabilitie = false;
        powerSkill = 0;

    }
    
    private void GetAffinityMessages(Unit actualUnitPlaying, Unit target, string affinity)
    {
        if (affinity.Equals("Wk"))
        {
            _view.WriteLine($"{target.name} es débil contra el ataque de {actualUnitPlaying.name}");
        }
        else if (affinity.Equals("Rs"))
        {
            _view.WriteLine($"{target.name} es resistente el ataque de {actualUnitPlaying.name}");
        }
        else if (affinity.Equals("Nu"))
        {
            _view.WriteLine($"{target.name} bloquea el ataque de {actualUnitPlaying.name}");
        }
        else if (affinity.Equals("Rp"))
        {
            _view.WriteLine($"{target.name} devuelve {actualUnitPlaying.damageRound} daño a {actualUnitPlaying.name}");
        }
        else if (affinity.Equals("Dr"))
        {
            _view.WriteLine($"{target.name} absorbe {actualUnitPlaying.damageRound} daño");
        }
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
    
    private void SetLimitDamage(Unit target)
    {
        if (target.ActualHP <= 0)
        {
            target.ActualHP = 0;
        }
        
    }

    private void SetLimitAbsorbe(Unit target)
    {
        if (target.ActualHP > target.stats.HP)
        {
            target.ActualHP = target.stats.HP;
        }
    }
    
    
    protected int GetDamageAttack(Unit attacker, Unit target, int _powerSkill, TypeAttack typeAttack)
    {
        DamageCalculatorFactory factory = new DamageCalculatorFactory();
        BaseDamageCalculator damageCalculator = factory.GetDamageCalculator(typeAttack);

        if (isAttackInHabilitie)
        {
            return damageCalculator.CalculateDamageAbility(attacker, _powerSkill, GetAffinity(target));            
        }
        
        else
        {
            return damageCalculator.CalculateDamage(attacker, GetAffinity(target));
        }
        
    }
    
    public void MakeAttack(Unit attacker, Unit target, TypeAttack typeAttack)
    {
        if(GetAffinity(target).Equals("Nu"))
        {
            GetAttackMessage(attacker, target);
        }
        
        else if (GetAffinity(target).Equals("Rp"))
        {
            int damage = GetDamageAttack(attacker, target, powerSkill, typeAttack);
            attacker.ActualHP -= damage;
            SetLimitDamage(target);
            GetAttackMessage(attacker, target);
        }
        
        else if (GetAffinity(target).Equals("Dr"))
        {
            int damage = GetDamageAttack(attacker, target, powerSkill, typeAttack);
            target.ActualHP += damage;
            SetLimitAbsorbe(target);
            GetAttackMessage(attacker, target);
        }
        
        else
        {
            int damage = GetDamageAttack(attacker, target, powerSkill, typeAttack);
            target.ActualHP -= damage;
            SetLimitDamage(target);
            GetAttackMessage(attacker, target);
        }
        
    }
    
    private void GetFinalHpMessage(Unit attacker, Unit target)
    {
        if (GetAffinity(target) == "Rp")
        {
            _view.WriteLine($"{attacker.name} termina con HP:{attacker.ActualHP}/{attacker.stats.HP}");
        }
        else
        {
            _view.WriteLine($"{target.name} termina con HP:{target.ActualHP}/{target.stats.HP}");
        }
    }
    
    private  void GetDamageMessage(Unit attacker, Unit target)
    {
        
        if (GetAffinity(target) != "Nu" && GetAffinity(target) != "Rp" && GetAffinity(target) != "Dr"){
            
            _view.WriteLine($"{target.name} recibe {attacker.damageRound} de daño");   
        }
        
    }
    
    public void ShowActionResults(Unit actualUnitPlaying, Unit target)
    {
        GetAffinityMessage(actualUnitPlaying, target);
        GetDamageMessage(actualUnitPlaying, target);
        GetFinalHpMessage(actualUnitPlaying, target);
        
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
    
    protected abstract void GetAttackMessage(Unit attacker, Unit target);
    
    protected abstract string GetAffinity(Unit target);


}

    