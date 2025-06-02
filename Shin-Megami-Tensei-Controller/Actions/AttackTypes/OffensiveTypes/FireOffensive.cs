using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;

public class FireOffensive: BaseOffensive
{
    private View _view;
    
    public FireOffensive(View view): base(view)
    {
        _view = view;
        typeAttack = TypeAttack.Fire;

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