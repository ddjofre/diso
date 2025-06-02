using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.AttackTypes.OfensiveTypes;
using Shin_Megami_Tensei.Actions.Invocations;
using Shin_Megami_Tensei.Actions.SkillExecutors;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Skills;

namespace Shin_Megami_Tensei.Actions;

public class ActionExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;

    public ActionExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
    }

    public void ExecutePhysicalAttack(ActionContext context)
    {
        var attackExecutor = CreatePhysicalAttackExecutor();
        ExecuteAttack(context, attackExecutor);
    }

    public void ExecuteGunAttack(ActionContext context)
    {
        var attackExecutor = CreateGunAttackExecutor();
        ExecuteAttack(context, attackExecutor);
    }
    
    
    public void ExecuteSkill(ActionContext context)
    {
        var skillExecutor = new SkillExecutor(_view, _turnCalculator);
        int printedSkillCount = skillExecutor.ShowAvailableSkills(context.actualUnitPlaying);
    
        int optionUser = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");
    
        if (optionUser == printedSkillCount + 1)
        {
            throw new OperationCanceledException();
        }

        var skillInfo = context.actualUnitPlaying.skillInfo[optionUser - 1];
        Skill skill = skillExecutor.CreateSkill(skillInfo);
    
        try
        {
            skill.Execute(context.actualUnitPlaying, context.opponentPlayer, context.activePlayer);
        }
        catch (OperationCanceledException)
        {
            throw new OperationCanceledException();
        }
    }

    public void ExecutePass(ActionContext context)
    {
        _turnCalculator.CalculateTurnAfterSummonOrPass(context.activePlayer);
        ShowTurnResults();
    }

    public void ExecuteSurrender(ActionContext context)
    {
        context.actualUnitPlaying.DoesSurrender = true;
    }

    private void ExecuteAttack(ActionContext context, BasicAttackExecutor basicAttackExecutor)
    {
        basicAttackExecutor.ShowAvailableTargets(context.opponentPlayer, context.actualUnitPlaying);
        var targetsIndexes = basicAttackExecutor.GetTargets(context.opponentPlayer);

        if (targetsIndexes[0] == -1)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        var target = basicAttackExecutor.GetRival(targetsIndexes[0], context.opponentPlayer);
        basicAttackExecutor.Execute(context.actualUnitPlaying, context.opponentPlayer, targetsIndexes);
        basicAttackExecutor.ShowFinalHpMessage(context.actualUnitPlaying, target);
        basicAttackExecutor.ShowActionTurnResults(context.activePlayer, target);
    }

    public void ExecuteSummonAtChosenPosition(ActionContext context)
    {
        var invoke = new Invoke(_view);
        List<int> validIndexes = invoke.ShowUnitsInReserve(context.activePlayer.Team);
        int userInput = invoke.GetUserInput();
        
        if (validIndexes.Count == 0)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        _view.WriteLine("----------------------------------------");
        
        int realIndexToInvoke = invoke.GetRealIndexFromUserInput(userInput, validIndexes);
        
        if (realIndexToInvoke == -1)
        {
            throw new OperationCanceledException();
        }

        invoke.ShowAvailablePositionsForInvokeMonster(context.activePlayer.Team);
        int unitToRemoveFromBoard = invoke.GetUserInput();
        _view.WriteLine("----------------------------------------");
    
        invoke.MakeInvoke(context.activePlayer, realIndexToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
        
    }

    public void ExecuteSummonReplacingInvoker(ActionContext context)
    {
        var invoke = new Invoke(_view);
        List<int> validIndexes = invoke.ShowUnitsInReserve(context.activePlayer.Team);
        int userInput = invoke.GetUserInput();
        
        if (validIndexes.Count == 0)
        {
            _view.WriteLine("----------------------------------------");
            throw new OperationCanceledException();
        }
        
        _view.WriteLine("----------------------------------------");
        
        int realIndexToInvoke = invoke.GetRealIndexFromUserInput(userInput, validIndexes);
        
        if (realIndexToInvoke == -1)
        {
            throw new OperationCanceledException();
        }
    
        int unitToRemoveFromBoard = invoke.GetActualUnitIndex(context.activePlayer.Team, context.actualUnitPlaying);
        invoke.MakeInvoke(context.activePlayer, realIndexToInvoke, unitToRemoveFromBoard);
        _view.WriteLine("----------------------------------------");
        
    }

    private BasicAttackExecutor CreatePhysicalAttackExecutor()
    {
        var offensivePhys = new PhysOffensive(_view);
        var singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
        return new BasicAttackExecutor(offensivePhys, singleTypeTarget, _turnCalculator, _view);
    }

    private BasicAttackExecutor CreateGunAttackExecutor()
    {
        var offensiveGun = new GunOffensive(_view);
        var singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
        return new BasicAttackExecutor(offensiveGun, singleTypeTarget, _turnCalculator, _view);
    }
    
    public void ShowTurnResults()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
    
   
}