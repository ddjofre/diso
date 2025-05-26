using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Battle.ActionManagers;

public abstract class BaseActionManager
{
    protected View _view;
    protected TurnCalculator _turnCalculator;
    protected int _actionChosen;

    protected BaseActionManager(View view)
    {
        _view = view;
        _turnCalculator = new TurnCalculator();
    }

    protected abstract void ShowPossibleActions(Unit actualUnitPlaying);
    
    public int ChooseAction(Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione una acción para {actualUnitPlaying.name}");
        ShowPossibleActions(actualUnitPlaying);
        
        int actionChosen = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");

        _actionChosen = actionChosen;
        
        return actionChosen;
    }
    
    public abstract void MakeAction(Player player, Player playerRival, Unit actualUnitPlaying);
    
    // Shared methods
    protected void HandlePassTurn(Player player)
    {
        _turnCalculator.CalculateTurnAfterSummonOrPass(player);
        ShowActualTurns();
        _view.WriteLine("----------------------------------------");
    }
    
    protected void ShowActualTurns()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
    }
    
    protected void MakeUseOfSkills(Unit actualUnitPlaying, Player player, Player playerRival)
    {
        SkillExecutor skillExecutor = new SkillExecutor(_view, _turnCalculator);
        int printedSkillCount = skillExecutor.ShowAvailableSkills(actualUnitPlaying);
        
        int optionUser = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");
        
        if (optionUser == printedSkillCount + 1)
        {
            RetryActionChoice(player, playerRival, actualUnitPlaying);
            return;
        }

        SkillInfo skillInfo = actualUnitPlaying.skillInfo[optionUser - 1];

        Skill skill = skillExecutor.CreateSkill(skillInfo);
        
        skillExecutor.ExecuteSkill(actualUnitPlaying, playerRival, player);
    }
    
    protected void RetryActionChoice(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        int actionChosen = ChooseAction(actualUnitPlaying);
        MakeAction(player, playerRival, actualUnitPlaying);
    }
    
    protected AttackExecutor GetPhysAttackExecutor()
    {
        BaseAttack attackPhys = new PhysAttack(_view, _turnCalculator);
        BaseTypeTarget singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
        return new AttackExecutor(attackPhys, singleTypeTarget, _turnCalculator, _view);
    }
    
    protected void HandleAttack(Player player, Player playerRival, Unit actualUnitPlaying, AttackExecutor actionExecutor)
    {
        actionExecutor.ShowAvailableTargets(playerRival, actualUnitPlaying);
        
        List<int> targetsIndexes = actionExecutor.GetTargets(playerRival);

        int targetIndex = targetsIndexes[0];

        if (targetIndex == -1)
        {
            _view.WriteLine("----------------------------------------");
            RetryActionChoice(player, playerRival, actualUnitPlaying);
            return;
        }
        
        Unit target = actionExecutor.GetRival(targetIndex, playerRival);
        
        actionExecutor.Execute(target, actualUnitPlaying, player, playerRival, targetsIndexes);
    }
    
    protected void HandleSummon(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        PerformSummon(player, playerRival, actualUnitPlaying);
        _turnCalculator.CalculateTurnAfterSummonOrPass(player);
        ShowActualTurns();
        _view.WriteLine("----------------------------------------");
    }
    
    protected abstract void PerformSummon(Player player, Player playerRival, Unit actualUnitPlaying);
}