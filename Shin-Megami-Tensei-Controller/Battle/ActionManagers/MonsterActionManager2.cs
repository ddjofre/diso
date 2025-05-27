using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle.ActionManagers;

public class MonsterActionManager2 : BaseActionManager2
{
    public MonsterActionManager2(View view) : base(view) { }

    protected override void ShowPossibleActions(Unit actualUnitPlaying)
    {
        _view.WriteLine("1: Atacar");
        _view.WriteLine("2: Usar Habilidad");
        _view.WriteLine("3: Invocar");
        _view.WriteLine("4: Pasar Turno");
    }

    protected override void ExecuteChosenAction(ActionContext context)
    {
        if (_actionChosen == 1)
        {
            _actionExecutor.ExecutePhysicalAttack(context);
        }
        
        else if (_actionChosen == 2)
        {
            _actionExecutor.ExecuteSkill(context);
        }
        
        else if (_actionChosen == 3)
        {
            PerformSummon(context);
            _turnCalculator.CalculateTurnAfterSummonOrPass(context.activePlayer);
            _actionExecutor.ShowTurnResults();
        }
        
        else if (_actionChosen == 4)
        {
            _actionExecutor.ExecutePass(context);
        }
    }


    protected override void PerformSummon(ActionContext context)
    {
        var invoke = new Invoke(_view, _turnCalculator);
        int numPrints = invoke.ShowUnitsInReserve(context.activePlayer.Team);
        int unitToInvoke = invoke.GetUserInput();
        
        if (numPrints == 1)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        _view.WriteLine("----------------------------------------");
        int unitToRemoveFromBoard = invoke.GetActualUnitIndex(context.activePlayer.Team, context.actualUnitPlaying);
        invoke.MakeInvoke(context.activePlayer.Team, unitToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
    }
    
}