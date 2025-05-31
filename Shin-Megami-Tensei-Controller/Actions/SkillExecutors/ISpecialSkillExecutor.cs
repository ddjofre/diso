using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.SkillExecutors;

public interface ISpecialExecutor
{
    void Execute(Unit actualUnitPlaying, Player player);
}