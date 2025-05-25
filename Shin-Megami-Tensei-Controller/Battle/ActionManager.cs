using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.AttackTargetType;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Battle;


public class ActionManager
{
    private View _view;
    private TurnCalculator _turnCalculator;
    public int actionChoosed;
    private SkillFactory _skillFactory;

    public ActionManager(View view)
    {
        _view = view;
        _turnCalculator = new TurnCalculator();
        _skillFactory = new SkillFactory(_view, _turnCalculator);


    }
    
    private void ShowPossibleActions(Unit actualUnitPlaying)
    {
        if (actualUnitPlaying.type.Equals(TypeUnits.samurai))
        {
            _view.WriteLine("1: Atacar");
            _view.WriteLine("2: Disparar");
            _view.WriteLine("3: Usar Habilidad");
            _view.WriteLine("4: Invocar");
            _view.WriteLine("5: Pasar Turno");
            _view.WriteLine("6: Rendirse");
        }
        
        else
        {
            _view.WriteLine("1: Atacar");
            _view.WriteLine("2: Usar Habilidad");
            _view.WriteLine("3: Invocar");
            _view.WriteLine("4: Pasar Turno");
            
        }
    }
    
    public int ChooseAction(Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione una acción para {actualUnitPlaying.name}");
        ShowPossibleActions(actualUnitPlaying);
        
        int actionChosen = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");

        actionChoosed = actionChosen;
        
        return actionChosen;
    }
    
    private AttackExecutor GetActionFromChoice(int actionChoice)
    {
        if (actionChoice == 1)
        {
            BaseAttack attackPhys = new PhysAttack(_view, _turnCalculator);
            BaseTypeTarget singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
            AttackExecutor attackExecutor = new AttackExecutor(attackPhys, singleTypeTarget, _turnCalculator, _view);
            return attackExecutor;
            //return new SingleTargetAttack(_view, attackPhys, _turnCalculator);
        }

        else if (actionChoice == 2)
        {
            
            BaseAttack attackGun = new GunAttack(_view, _turnCalculator);
            BaseTypeTarget singleTypeTarget = new SingleTypeTarget(_view, TypeTarget.Single);
            AttackExecutor attackExecutor = new AttackExecutor(attackGun, singleTypeTarget, _turnCalculator, _view);
            return attackExecutor;
        }
        
        else
        {
            throw new NotImplementedException();
        }
    }
    
    
    private void MakeActionSamurai(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        if (actionChoosed == 1 || actionChoosed == 2)
        {
            HandleAttackOrShoot(player, playerRival, actualUnitPlaying);
        }
        else if (actionChoosed == 3)
        {
            MakeUseOfSkills(actualUnitPlaying, player, playerRival);
        }

        else if(actionChoosed == 4)
        {
            HandleSummon(player, playerRival, actualUnitPlaying);
        }
        
        else if(actionChoosed == 5)
        {
            HandlePassTurn(player);
        }
        
        else if (actionChoosed == 6)
        {
            actualUnitPlaying.DoesSurrender = true;
        }

        _turnCalculator.ResetCalculator();
        
    }
    
    private void MakeActionMonster(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        if (actionChoosed == 1)
        {
            HandleAttackOrShoot(player, playerRival, actualUnitPlaying);
        }
        else if (actionChoosed == 2)
        {
            MakeUseOfSkills(actualUnitPlaying, player, playerRival);
        }

        else if(actionChoosed == 3)
        {
            HandleSummon(player, playerRival, actualUnitPlaying);
        }
        
        else if (actionChoosed == 4)
        {
            HandlePassTurn(player);
        }

        _turnCalculator.ResetCalculator();
        
    }
    
    public void MakeAction(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        if (actualUnitPlaying.type.Equals(TypeUnits.samurai))
        {
            MakeActionSamurai(player, playerRival, actualUnitPlaying);
        }
        else
        {
            MakeActionMonster(player, playerRival, actualUnitPlaying);
        }
        _turnCalculator.ResetCalculator();
    }
    
    private void HandleAttackOrShoot(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        //SingleTargetAttack action = GetActionFromChoice(actionChoosed, actualUnitPlaying, playerRival, player);
        AttackExecutor actionExecutor = GetActionFromChoice(actionChoosed);

        actionExecutor.ShowAvailableTargets(playerRival, actualUnitPlaying);
        
        //action.ShowAvailableTargets(playerRival, actualUnitPlaying);
        
        List<int> targetsIndexes = actionExecutor.GetTargets(playerRival);

        int targetIndex = targetsIndexes[0];
        
        //int targetIndex = action.ChooseTarget(playerRival);

        if (targetIndex == -1)
        {
            _view.WriteLine("----------------------------------------");
            RetryActionChoice(player, playerRival, actualUnitPlaying);
            return;
        }
        
        Unit target = actionExecutor.GetRival(targetIndex, playerRival);
        
        actionExecutor.Execute( target, actualUnitPlaying, player, playerRival, targetsIndexes);

        //Unit target = action.GetRival(playerRival, targetIndex);

        //action.Execute(actualUnitPlaying, target, player);
    }
    
    private void HandlePassTurn(Player player)
    {
        _turnCalculator.CalculateTurnAfterSummonOrPass(player);
        ShowActualTurns();
        _view.WriteLine("----------------------------------------");
    }
    

    private void HandleSummon(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        if (actualUnitPlaying.type.Equals(TypeUnits.samurai))
        {
            HandleSamuraiSummon(player, playerRival, actualUnitPlaying);
        }
        else
        {
            HandleMonsterSummon(player, playerRival, actualUnitPlaying);
        }
        
        _turnCalculator.CalculateTurnAfterSummonOrPass(player);
        ShowActualTurns();
        _view.WriteLine("----------------------------------------");
        
    }

    private void HandleSamuraiSummon(Player player, Player playerRival, Unit actualUnitPlaying)
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

    private void HandleMonsterSummon(Player player, Player playerRival, Unit actualUnitPlaying)
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
    
    
    private void MakeUseOfSkills(Unit actualUnitPlaying, Player player, Player playerRival)
    {
        SkillExecutor skillExecutor = new SkillExecutor(_view, _turnCalculator);
        int printedSkillCount = skillExecutor.ShowAvailableSkills(actualUnitPlaying);
        
        
        //int printedSkillCount = ShowAvailableSkills(actualUnitPlaying);
        int optionUser = Convert.ToInt32(_view.ReadLine());
        _view.WriteLine("----------------------------------------");
        
        if (optionUser == printedSkillCount + 1)
        {
            RetryActionChoice(player, playerRival, actualUnitPlaying);
            return;
        }

        SkillInfo skillInfo = actualUnitPlaying.skillInfo[optionUser - 1];

        FinalSkill skill = skillExecutor.CreateSkill(skillInfo);
        
        //Skill skill = _skillFactory.CreateSkillFromMap(skillInfo.name, skillInfo);
        //skill.typeTargetAttack.ShowAvailableTargets(playerRival, actualUnitPlaying);
        
        
        //int indexRival = skill.typeTargetAttack.ChooseTarget(playerRival);
        //Unit target = skill.typeTargetAttack.GetRival(playerRival, indexRival);
        
        //skill.typeTargetAttack.Execute(actualUnitPlaying, target, player );
        //skill.DiscountMP(actualUnitPlaying);
        
        skillExecutor.ExecuteSkill(actualUnitPlaying, playerRival, player);
        
    }
    

    private int ShowAvailableSkills(Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione una habilidad para que {actualUnitPlaying.name} use");

        int numVecesQueSeHizoPrint = 0;
            
        for (int i = 0; i < actualUnitPlaying.skillInfo.Count; i++)
        {
            if (actualUnitPlaying.skillInfo[i].cost <= actualUnitPlaying.ActualMP)
            {
                _view.WriteLine($"{i + 1}-{actualUnitPlaying.skillInfo[i].name} MP:{actualUnitPlaying.skillInfo[i].cost}");
                numVecesQueSeHizoPrint++;
            }
        }
            
        _view.WriteLine($"{numVecesQueSeHizoPrint + 1}-Cancelar");
        return numVecesQueSeHizoPrint;
    }

    
    
    private void RetryActionChoice(Player player, Player playerRival, Unit actualUnitPlaying)
    {
        int actionChosen = ChooseAction(actualUnitPlaying);
        MakeAction(player, playerRival, actualUnitPlaying);
    }
    
    private void ShowActualTurns()
    {
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");

    }
    
    
}
