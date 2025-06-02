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
    public TypeTarget targetType;
    
    public SingleTypeTarget(View view, TypeTarget typeTarget): base(typeTarget){
        
        _view = view; 
        targetType = typeTarget;
    }
    
    private List<(int displayNumber, int unitIndex)> BuildTargetIndex(Player playerRival)
    {
        var mapping = new List<(int, int)>();
        int displayNumber = 1;
        int unitIndex = 0;

        foreach (var unit in playerRival.Team.UnitsInGame)
        {
            if (unit != null && MeetCondition(unit))
            {
                mapping.Add((displayNumber, unitIndex));
                displayNumber++;
            }
            unitIndex++;
        }

        return mapping;
    }

    private int GetUnitIndexFromChoice(List<(int displayNumber, int unitIndex)> mapping, int userInput)
    {
        return mapping.LastOrDefault(pair => pair.displayNumber == userInput).unitIndex;
    }
    
    private bool IsCancelChoice(int userInput, int maxDisplayNumber)
    {
        return userInput == maxDisplayNumber;
    }
    
    protected int ChooseTarget(Player playerRival)
    {
        var selectableTargets = BuildTargetIndex(playerRival);
        
        int userInput = Convert.ToInt32(_view.ReadLine());

        if (IsCancelChoice(userInput, selectableTargets.Count + 1))
        {
            return -1;
        }
        
        return GetUnitIndexFromChoice(selectableTargets, userInput);
    }


    
    public override bool MeetCondition(Unit target)
    {
        return target.ActualHP != 0;
    }

    public override void ShowAvailablesTargets(Player playerRival, Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione un objetivo para {actualUnitPlaying.name}");

        int displayNumber = 1;
        foreach (var unit in playerRival.Team.UnitsInGame)
        {
            if (unit != null && MeetCondition(unit))
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                displayNumber += 1;
            }
        }
        _view.WriteLine($"{displayNumber}-Cancelar");
    }
    
    
    //esto quiza tenga que encapsularlo
    public override List<int> GetTargets(Player playerRival)
    {
        List<int> targets = new List<int>();
        targets.Add(ChooseTarget(playerRival));
        return targets;
    }

    
    
    
    
    
    
}