using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttackTargetType;

public class SingleTargetAttack: BaseTypeTargetAttack
{
    private readonly View _view;
    private readonly BaseAttack _typeAttack;
    private readonly TurnCalculator _turnCalculator;
    
    public SingleTargetAttack(View view, BaseAttack typeAttack, TurnCalculator turnCalculator): base()
    {
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
    
    public override void ShowAvailableTargets(Player playerRival, Unit actualUnitPlaying)
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
    
    public override int ChooseTarget(Player playerRival)
    {
        var selectableTargets = BuildTargetIndex(playerRival);
        
        int userInput = Convert.ToInt32(_view.ReadLine());

        if (IsCancelChoice(userInput, selectableTargets.Count + 1))
        {
            return -1;
        }

        return GetUnitIndexFromChoice(selectableTargets, userInput);
    }
    
    public override Unit GetRival(Player rival, int indexRival)
    {
        Unit rivalChosen = rival.Team.UnitsInGame[indexRival];
        _view.WriteLine("----------------------------------------");
        return rivalChosen;
    }
    
    public override void Execute(Unit actualUnitPlaying, Unit target, Player player)
    {
        _typeAttack.MakeAttack(actualUnitPlaying,target,_typeAttack.typeAttack );
        _turnCalculator.CalculateTurnsAfterAttack(player, target, _typeAttack.typeAttack);
        _typeAttack.ShowActionResults(actualUnitPlaying, target);
    }
    
    
}