using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle.ActionManagers;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Factories;

public class ActionManagerFactory
{
    private readonly View _view;
    private readonly Dictionary<TypeUnits, BaseActionManager2> _managers;

    public ActionManagerFactory(View view)
    {
        _view = view;
        _managers = new Dictionary<TypeUnits, BaseActionManager2>
        {
            { TypeUnits.samurai, new SamuraiActionManager2(_view) },
            { TypeUnits.monster, new MonsterActionManager2(_view) }
        };
    }

    public BaseActionManager2 GetActionManager(Unit unit)
    {
        if (_managers.TryGetValue(unit.type, out var manager))
        {
            return manager;
        }

        throw new NotImplementedException($"No action manager found for unit type: {unit.type}");
    }
}