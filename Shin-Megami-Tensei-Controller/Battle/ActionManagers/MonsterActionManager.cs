using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle.ActionManagers;
public class MonsterActionManager : BaseActionManager
{
    public MonsterActionManager(View view) : base(view) { }

    protected override void ShowPossibleActions(Unit actualUnitPlaying)
    {
        _view.WriteLine("1: Atacar");
        _view.WriteLine("2: Usar Habilidad");
        _view.WriteLine("3: Invocar");
        _view.WriteLine("4: Pasar Turno");
    }

    public override void MakeAction(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        switch (_actionChosen)
        {
            case 1:
                HandlePhysAttack(player, playerRival, actualUnitPlaying);
                break;
            case 2:
                MakeUseOfSkills(actualUnitPlaying, player, playerRival);
                break;
            case 3:
                HandleSummon(player, playerRival, actualUnitPlaying);
                break;
            case 4:
                HandlePassTurn(player);
                break;
        }

        _turnCalculator.ResetCalculator();
    }

    private void HandlePhysAttack(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        AttackExecutor actionExecutor = GetPhysAttackExecutor();
        HandleAttack(player, playerRival, actualUnitPlaying, actionExecutor);
    }

    protected override void PerformSummon(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        Invoke invoke = new Invoke(_view, _turnCalculator);
        int numPrints = invoke.ShowUnitsInReserve(player.Team);
        int unitToInvoke = invoke.GetUserInput();
        
        if (numPrints == 1)
        {
            _view.WriteLine("----------------------------------------");
            RetryActionChoice(player, playerRival, actualUnitPlaying);
            return;
        }
        
        _view.WriteLine("----------------------------------------");
        int unitToRemoveFromBoard = invoke.GetActualUnitIndex(player.Team, actualUnitPlaying);
        invoke.MakeInvoke(player.Team, unitToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
    }
    
    
}