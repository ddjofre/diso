using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.Invocations;
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

    protected override void ExecuteChosenAction(ActionContext context)
    {
        if (_actionChosen == 1)
        {
            _actionExecutor.ExecutePhysicalAttack(context);
        }
        
        else if (_actionChosen == 2)
        {
            _actionExecutor.ExecuteSkill(context);
            context.activePlayer.numOfTimeUsesHabilities +=1 ;
        }
        
        else if (_actionChosen == 3)
        {
            _actionExecutor.ExecuteSummonReplacingInvoker(context);
            //PerformSummon(context);
            _turnCalculator.CalculateTurnAfterSummonOrPass(context.activePlayer);
            _actionExecutor.ShowTurnResults();
        }
        
        else if (_actionChosen == 4)
        {
            _actionExecutor.ExecutePass(context);
        }
    }
    
    
}