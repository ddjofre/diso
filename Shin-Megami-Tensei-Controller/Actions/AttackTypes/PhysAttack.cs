using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes;

public class PhysAttack: BaseAttack
{
    private View _view;
    
    public PhysAttack(View view, TurnCalculator turnCalculator): base(view, turnCalculator)
    {
        _view = view;
        typeAttack = TypeAttack.Phys;

    }
    

    
    protected override void GetAttackMessage(Unit attacker, Unit target)
    {
        _view.WriteLine($"{attacker.name} ataca a {target.name}");
    }
    

    protected override string GetAffinity(Unit target)
    {
        return target.affinity.Phys;
    }

    
}