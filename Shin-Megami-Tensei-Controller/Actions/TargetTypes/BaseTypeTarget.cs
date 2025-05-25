using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.TargetTypes;

public abstract class BaseTypeTarget
{
    public TypeTarget targetType { get; }

    protected BaseTypeTarget(TypeTarget typeTarget)
    {
        targetType = typeTarget;
    }

    public abstract void ShowAvailablesTargets(Player playerRival, Unit actualUnitPlaying);
    public abstract List<int> GetTargets(Player playerRival);
}
