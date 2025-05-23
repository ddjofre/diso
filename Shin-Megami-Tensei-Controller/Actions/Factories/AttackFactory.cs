using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;

namespace Shin_Megami_Tensei.Actions.Factories;

public class AttackFactory
{
    private View _view;
    private TurnCalculator _turnCalculator;

    public AttackFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;

    }
    
    
    
    public BaseAttack CreateTypeAttack(TypeAttack typeAttack)
    {
        if (typeAttack == TypeAttack.Phys)
        {
            return new PhysAttack(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Gun)
        {
            return new GunAttack(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Fire)
        {
            return new FireAttack(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Ice)
        {
            return new IceAttack(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Elec)
        {
            return new ElecAttack(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Force)
        {
            return new ForceAttack(_view, _turnCalculator);
        }
        else
        {
            throw new NotImplementedException();
        }
        
    }

}