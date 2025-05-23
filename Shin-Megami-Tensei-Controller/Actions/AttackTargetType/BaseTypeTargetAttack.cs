using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTargetType;

public abstract class BaseTypeTargetAttack
{
    public abstract void ShowAvailableTargets(Player playerRival, Unit actualUnitPlaying);

    public abstract int ChooseTarget(Player playerRival);

    public abstract Unit GetRival(Player rival, int indexRival);
    
    public abstract void Execute(Unit actualUnitPlaying, Unit target, Player player);
}