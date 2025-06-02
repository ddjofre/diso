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
    private Dictionary<TypeAttack, Func<BaseOffensive>> _attackMap;

    public TypeAttackFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;

        _attackMap = new Dictionary<TypeAttack, Func<BaseOffensive>>
        {
            { TypeAttack.Phys, () => new PhysOffensive(_view) },
            { TypeAttack.Gun, () => new GunOffensive(_view) },
            { TypeAttack.Fire, () => new FireOffensive(_view) },
            { TypeAttack.Ice, () => new IceOffensive(_view) },
            { TypeAttack.Elec, () => new ElecOffensive(_view) },
            { TypeAttack.Force, () => new ForceOffensive(_view) }
        };
    }

    public BaseOffensive CreateTypeAttack(TypeAttack typeAttack)
    {
        if (_attackMap.TryGetValue(typeAttack, out var attackFactory))
        {
            return attackFactory();
        }
        else
        {
            throw new KeyNotFoundException($"type attack not found");
        }
    }
}