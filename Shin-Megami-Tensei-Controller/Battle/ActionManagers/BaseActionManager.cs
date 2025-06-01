using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle.ActionManagers;

public abstract class BaseActionManager
{
    protected View _view;
    protected TurnCalculator _turnCalculator;
    protected ActionExecutor _actionExecutor;
    protected int _actionChosen;

    protected BaseActionManager(View view)
    {
        _view = view;
        _turnCalculator = new TurnCalculator();
        _actionExecutor = new ActionExecutor(view, _turnCalculator);
    }
    
    public int ChooseAction(Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione una acción para {actualUnitPlaying.name}");
        ShowPossibleActions(actualUnitPlaying);
        
        _actionChosen = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");
        
        return _actionChosen;
    }
    
    public void MakeAction(ActionContext context)
    {
        try
        {
            ExecuteChosenAction(context);
        }
        
        catch (OperationCanceledException)
        {
            ChooseAction(context.actualUnitPlaying);
            MakeAction(context);
        }
        
        finally
        {
            _turnCalculator.ResetCalculator();
        }
    }

    protected abstract void ShowPossibleActions(Unit actualUnitPlaying);
    protected abstract void ExecuteChosenAction(ActionContext context);
}