namespace Shin_Megami_Tensei.Actions.TargetTypes;

using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;



public class SingleTypeTarget: BaseTypeTarget
{
    private readonly View _view;
    private readonly BaseAttack _typeAttack;
    private readonly TurnCalculator _turnCalculator;
    
    public SingleTypeTarget(View view, BaseAttack typeAttack, TurnCalculator turnCalculator): base(){
        _view = view;
        _typeAttack = typeAttack;
        _turnCalculator = turnCalculator;
    }
    
    private List<(int displayNumber, int unitIndex)> BuildTargetIndex(Player playerRival)
    {
        var mapping = new List<(int, int)>();
        int displayNumber = 1;
        int unitIndex = 0;

        foreach (var unit in playerRival.Team.UnitsInGame)
        {
            if (unit != null && unit.ActualHP != 0)
            {
                mapping.Add((displayNumber, unitIndex));
                displayNumber++;
            }
            unitIndex++;
        }

        return mapping;
    }
    
    private bool IsCancelChoice(int userInput, int maxDisplayNumber)
    {
        return userInput == maxDisplayNumber;
    }

    private int GetUnitIndexFromChoice(List<(int displayNumber, int unitIndex)> mapping, int userInput)
    {
        return mapping.LastOrDefault(pair => pair.displayNumber == userInput).unitIndex;
    }
    
    
    public void ShowAvailableTargets(Player playerRival, Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione un objetivo para {actualUnitPlaying.name}");

        int displayNumber = 1;
        foreach (var unit in playerRival.Team.UnitsInGame)
        {
            if (unit != null && unit.ActualHP != 0)
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                displayNumber += 1;
            }
        }
        _view.WriteLine($"{displayNumber}-Cancelar");
    }
    
    
    public int ChooseTarget(Player playerRival)
    {
        var selectableTargets = BuildTargetIndex(playerRival);
        
        int userInput = Convert.ToInt32(_view.ReadLine());

        if (IsCancelChoice(userInput, selectableTargets.Count + 1))
        {
            return -1;
        }

        return GetUnitIndexFromChoice(selectableTargets, userInput);
    }

    
    
    public override List<int> GetTargets(Player playerRival)
    {
        List<int> targets = new List<int>();
        targets.Add(ChooseTarget(playerRival));
        return targets;
    }


    public Unit GetRival(Player rival, int indexRival)
    {
        Unit rivalChosen = rival.Team.UnitsInGame[indexRival];
        _view.WriteLine("----------------------------------------");
        return rivalChosen;
    }
    
    
    
    
    
    /*
    public void Execute(Unit actualUnitPlaying, Unit target, Player player)
    {
        _typeAttack.MakeAttack(actualUnitPlaying,target);
        _turnCalculator.CalculateTurnsAfterAttack(player, target, _typeAttack.typeAttack);
        _typeAttack.ShowActionResults(actualUnitPlaying, target);
    }
    */
    
    
    
    
    
    
    
}