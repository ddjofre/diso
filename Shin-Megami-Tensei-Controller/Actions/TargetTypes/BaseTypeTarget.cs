using Shin_Megami_Tensei.GameComponents;

namespace Shin_Megami_Tensei.Actions.TargetTypes;

public abstract class BaseTypeTarget
{
    public abstract List<int> GetTargets(Player playerRival);
    
}