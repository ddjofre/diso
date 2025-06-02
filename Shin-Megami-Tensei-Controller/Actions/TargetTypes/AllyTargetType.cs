using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.TargetTypes;

public class AllyTargetType: SingleTypeTarget
{
    private readonly View _view;
    public TypeTarget targetType;
    private BaseHeal _healType;
    
    public AllyTargetType(View view, TypeTarget typeTarget, BaseHeal healType): base(view, typeTarget){
        
        _view = view; 
        targetType = typeTarget;
        _healType = healType;
    }
    
    
        private List<(int displayNumber, int unitIndex, bool isFromReserve)> BuildTargetIndexWithReserve(Player player)
    {
        var mapping = new List<(int, int, bool)>();
        int displayNumber = 1;

        // Agregar unidades del tablero
        for (int i = 0; i < player.Team.UnitsInGame.Length; i++)
        {
            var unit = player.Team.UnitsInGame[i];
            if (unit != null && MeetCondition(unit))
            {
                mapping.Add((displayNumber, i, false)); // false = no es de la reserva
                displayNumber++;
            }
        }

        // Agregar unidades de la reserva (vivas o muertas)
        for (int i = 0; i < player.Team.UnitsInReserve.Length; i++)
        {
            var unit = player.Team.UnitsInReserve[i];
            if (unit != null && MeetCondition(unit)) // Permite tanto vivos como muertos
            {
                mapping.Add((displayNumber, i, true)); // true = es de la reserva
                displayNumber++;
            }
        }

        return mapping;
    }

    private int GetUnitIndexFromChoiceWithReserve(List<(int displayNumber, int unitIndex, bool isFromReserve)> mapping, int userInput)
    {
        var selected = mapping.FirstOrDefault(pair => pair.displayNumber == userInput);
        
        // Si es de la reserva, devolvemos índice negativo para diferenciarlo
        if (selected.isFromReserve)
        {
            return -(selected.unitIndex + 1); // -1, -2, -3, -4 para reserva slots 0, 1, 2, 3
        }
        
        return selected.unitIndex; // Índice positivo para tablero
    }

    protected int ChooseTargetWithReserve(Player player)
    {
        var selectableTargets = BuildTargetIndexWithReserve(player);
        
        int userInput = Convert.ToInt32(_view.ReadLine());

        if (IsCancelChoice(userInput, selectableTargets.Count + 1))
        {
            return -999; // Valor especial para cancelar
        }
        
        return GetUnitIndexFromChoiceWithReserve(selectableTargets, userInput);
    }

    private bool IsCancelChoice(int userInput, int maxDisplayNumber)
    {
        return userInput == maxDisplayNumber;
    }

    public override void ShowAvailablesTargets(Player player, Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione un objetivo para {actualUnitPlaying.name}");

        int displayNumber = 1;
        
        // Mostrar unidades del tablero
        foreach (var unit in player.Team.UnitsInGame)
        {
            if (unit != null && MeetCondition(unit))
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                displayNumber++;
            }
        }

        // Mostrar unidades de la reserva
        foreach (var unit in player.Team.UnitsInReserve)
        {
            if (unit != null && MeetCondition(unit)) // Permite tanto vivos como muertos
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                displayNumber++;
            }
        }
        
        _view.WriteLine($"{displayNumber}-Cancelar");
    }

    public override List<int> GetTargets(Player player)
    {
        List<int> targets = new List<int>();
        targets.Add(ChooseTargetWithReserve(player));
        return targets;
    }
    
    
    public override bool MeetCondition(Unit target)
    {
        return _healType.CanTargetUnit(target);
    }
    
    
    
}