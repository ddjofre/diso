using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle.ActionManagers;

public class SamuraiActionManager : BaseActionManager
{
    public SamuraiActionManager(View view) : base(view) { }

    protected override void ShowPossibleActions(Unit actualUnitPlaying)
    {
        _view.WriteLine("1: Atacar");
        _view.WriteLine("2: Disparar");
        _view.WriteLine("3: Usar Habilidad");
        _view.WriteLine("4: Invocar");
        _view.WriteLine("5: Pasar Turno");
        _view.WriteLine("6: Rendirse");
    }

    public override void MakeAction(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        switch (_actionChosen)
        {
            case 1:
                HandlePhysAttack(player, playerRival, actualUnitPlaying);
                break;
            case 2:
                HandleGunAttack(player, playerRival, actualUnitPlaying);
                break;
            case 3:
                MakeUseOfSkills(actualUnitPlaying, player, playerRival);
                break;
            case 4:
                HandleSummon(player, playerRival, actualUnitPlaying);
                break;
            case 5:
                HandlePassTurn(player);
                break;
            case 6:
                actualUnitPlaying.DoesSurrender = true;
                break;
        }

        _turnCalculator.ResetCalculator();
    }

    private void HandlePhysAttack(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        AttackExecutor actionExecutor = GetPhysAttackExecutor();
        HandleAttack(player, playerRival, actualUnitPlaying, actionExecutor);
    }

    private void HandleGunAttack(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        AttackExecutor actionExecutor = GetGunAttackExecutor();
        HandleAttack(player, playerRival, actualUnitPlaying, actionExecutor);
    }

    private AttackExecutor GetGunAttackExecutor()
    {
        BaseAttack attackGun = new GunAttack(_view, _turnCalculator);
        BaseTypeTarget singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
        return new AttackExecutor(attackGun, singleTypeTarget, _turnCalculator, _view);
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
        
        invoke.ShowAvailablePositionsForInvokeMonster(player.Team);
        int unitToRemoveFromBoard = invoke.GetUserInput();
        _view.WriteLine("----------------------------------------");
        invoke.MakeInvoke(player.Team, unitToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
    }
}