using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;

public class IceOffensive: BaseOffensive
{
    private View _view;
    private DamageCalculator _damageCalculator;
    private TurnCalculator _turnCalculator;
    
    
    public IceOffensive(View view, TurnCalculator turnCalculator): base(view, turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _damageCalculator = new DamageCalculator();
        typeAttack = TypeAttack.Ice;

    }
    
    protected override void GetAttackMessage(Unit attacker, Unit target)
    {
        _view.WriteLine($"{attacker.name} lanza hielo a {target.name}");
    }
    
   
    protected override string GetAffinity(Unit target)
    {
        return target.affinity.Ice;
    }
    
    
    
}