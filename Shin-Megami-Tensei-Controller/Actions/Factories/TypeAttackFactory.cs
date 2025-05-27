using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;
using Shin_Megami_Tensei.Battle;

namespace Shin_Megami_Tensei.Actions.Factories;

public class TypeAttackFactory
{
    private View _view;
    private TurnCalculator _turnCalculator;

    public TypeAttackFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;

    }
    
    
    
    public BaseOffensive CreateTypeAttack(TypeAttack typeAttack)
    {
        if (typeAttack == TypeAttack.Phys)
        {
            return new PhysOffensive(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Gun)
        {
            return new GunOffensive(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Fire)
        {
            return new FireOffensive(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Ice)
        {
            return new IceOffensive(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Elec)
        {
            return new ElecOffensive(_view, _turnCalculator);
        }
        else if (typeAttack == TypeAttack.Force)
        {
            return new ForceOffensive(_view, _turnCalculator);
        }
        else
        {
            throw new NotImplementedException();
        }
        
    }

    public BaseHeal CreateTypeHealAttack(TypeHeal typeHeal)
    {
        if (typeHeal.Equals(TypeHeal.Dia))
        {
            return new DiaHeal(_view, _turnCalculator);
        }
        else if(typeHeal.Equals(TypeHeal.Recarm))
        {
            return new RecarmHeal(_view, _turnCalculator);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

}