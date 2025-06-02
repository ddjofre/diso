using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Battle;

public class Battle
{
    private View _view;
    private BattleInfoDisplay _battleInfoDisplay;
    private int playerInTurn;
    private Unit actualUnitPlaying;

    private UnitManager _unitManager;
    private ActionManagerFactory _actionManagerFactory; 
    
    private Unit rivalChoosed;

    public Battle(View view)
    {
        _view = view;
        _battleInfoDisplay = new BattleInfoDisplay(_view);
        _unitManager = new UnitManager(_view);
        _actionManagerFactory = new ActionManagerFactory(_view); 
    }
    
    
    
    public void ShowInitialInfo(Player actualPlayer,int turno)
    {
        _battleInfoDisplay.ShowInfoRound(actualPlayer, turno);
    }
    
    public void CalculateInitialTurns(Player actualPlayer)
    {
        TurnCalculator turnCalculator = new TurnCalculator();
        turnCalculator.CalculateInitialTurns(actualPlayer);
    }
    
    public void SetInitialsParametersToBattle(Player actualPlayer, Player rivalPlayer, int turn)
    {
        _unitManager.SetUnitsInGame(actualPlayer);
        _unitManager.SetUnitsInReserve(actualPlayer);
        _unitManager.SetOrderToUnits(actualPlayer);
            
        _unitManager.SetUnitsInGame(rivalPlayer);
        _unitManager.SetUnitsInReserve(rivalPlayer);
        _unitManager.SetOrderToUnits(rivalPlayer);
        
        CalculateInitialTurns(actualPlayer);
        
        actualUnitPlaying = _unitManager.SetActualUnitPlaying(actualPlayer);
        
        ShowInitialInfo(actualPlayer, turn);
    }
    
    public void ActualizeInitialsParametersToBattle(Player actualPlayer, Player rivalPlayer)
    {
        _unitManager.SetUnitsInGame(actualPlayer);
        _unitManager.SetUnitsInGame(rivalPlayer);
        
        actualUnitPlaying = _unitManager.SetActualUnitPlaying(actualPlayer);
        
        _unitManager.MoveDeadUnitsToReserve(actualPlayer);
        _unitManager.MoveDeadUnitsToReserve(rivalPlayer);
    }
    
    public void ActualizeOrderToUnits(Player actualPlayer)
    {
        List<int> orderIndexUnits = actualPlayer.Team.indexesOrderAttack;
        if (orderIndexUnits.Count > 1)
        {
            int primero = orderIndexUnits[0];
            orderIndexUnits.RemoveAt(0);
            orderIndexUnits.Add(primero);
        }
        
        actualPlayer.Team.indexesOrderAttack = orderIndexUnits;
    }
    
    private void ShowPlayersInformation(Player actualPlayer, Player rivalPlayer)
    {
        if (actualPlayer.PlayerId.Equals(1))
        {
            _battleInfoDisplay.ShowPlayerInformation(actualPlayer);
            _battleInfoDisplay.ShowPlayerInformation(rivalPlayer);
        }
        else
        {
            _battleInfoDisplay.ShowPlayerInformation(rivalPlayer);
            _battleInfoDisplay.ShowPlayerInformation(actualPlayer);
        }
    }
    
    public void MakeTurn(Player actualPlayer, Player rivalPlayer)
    {
        ActualizeInitialsParametersToBattle(actualPlayer, rivalPlayer);
        
        ShowPlayersInformation(actualPlayer, rivalPlayer);
        
        _battleInfoDisplay.ShowTurnsPLayer(actualPlayer);
        _battleInfoDisplay.ShowOrderUnitsPlay(actualPlayer);
        
        var context = new ActionContext(actualPlayer, rivalPlayer, actualUnitPlaying);
        var actionManager = _actionManagerFactory.GetActionManager(actualUnitPlaying);
        
        actionManager.ChooseAction(actualUnitPlaying);
        actionManager.MakeAction(context);
        
        _unitManager.MoveDeadUnitsToReserve(actualPlayer);
        _unitManager.MoveDeadUnitsToReserve(rivalPlayer);
        
        ActualizeOrderToUnits(actualPlayer);
        
    }
    
    public bool CheckIfHasTurns(Player playerPlaying)
    {
        if (playerPlaying.FullTurns == 0 && playerPlaying.BlinkingTurns == 0)
        {
            return false;
        }
        
        return true;
    }
    
    public void MakeTurns(Player playerPlaying, Player playerTarget)
    {
        while (CheckIfHasTurns(playerPlaying) && !CheckForSurrender() && !CheckLifeRival(playerTarget))
        {
            MakeTurn(playerPlaying, playerTarget);
        }
    }
    

    public bool CheckLifeRival(Player playerTarget)
    {
        int finalHP = 0;
        foreach (var unit in playerTarget.Team.UnitsInGame)
        {
            if (unit != null)
            {
                finalHP += unit.ActualHP;
            }
            
        }
        
        if (finalHP.Equals(0))
        {
            return true;
        }
        return false;
    }

    public bool CheckForWinner(Player playerPlaying, Player playerTarget)
    {
        int finalHP = 0;
        foreach (var unit in playerTarget.Team.UnitsInGame)
        {
            if (unit != null)
            {
                finalHP += unit.ActualHP;
            }
            
        }
        
        if (finalHP.Equals(0))
        {
            
            _battleInfoDisplay.AnnounceWinner(playerPlaying.PlayerId, playerPlaying.Team.Samurai);
            return true;
        }
        
        else if(CheckForSurrender())
        {
            _battleInfoDisplay.Surrender(actualUnitPlaying, playerPlaying.PlayerId);
            _battleInfoDisplay.AnnounceWinner(playerTarget.PlayerId, playerTarget.Team.Samurai);
            return CheckForSurrender();
        }
        return false;
        
    }
    
    public bool CheckForSurrender()
    {
        if (actualUnitPlaying.DoesSurrender)
        {
            return true;
        }

        return false;
    }
    
    
    
    










}