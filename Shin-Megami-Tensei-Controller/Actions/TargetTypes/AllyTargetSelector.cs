using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.TargetTypes;

public class AllyTargetSelector
{
    private readonly View _view;
    private readonly BaseHeal _healType;

    public AllyTargetSelector(View view, BaseHeal healType)
    {
        _view = view;
        _healType = healType;
    }

    public Unit SelectTarget(Player player, Unit caster)
    {
        var validTargets = GetValidTargets(player);
        
        if (!validTargets.Any())
        {
            throw new InvalidOperationException();
        }

        ShowTargetOptions(caster, validTargets);
        
        int userChoice = GetUserChoice(validTargets.Count);
        
        if (userChoice == -1)
        {
            throw new OperationCanceledException();
        }

        return validTargets[userChoice];
    }

    private List<Unit> GetValidTargets(Player player)
    {
        var targets = new List<Unit>();
        
        foreach (var unit in player.Team.UnitsInGame)
        {
            if (unit != null && _healType.CanTargetUnit(unit))
            {
                targets.Add(unit);
            }
        }
        
        if (_healType is RecarmHeal)
        {
            foreach (var unit in player.Team.UnitsInReserve)
            {
                if (unit != null && _healType.CanTargetUnit(unit))
                {
                    targets.Add(unit);
                }
            }
        }

        return targets;
    }

    private void ShowTargetOptions(Unit caster, List<Unit> validTargets)
    {
        _view.WriteLine($"Seleccione un objetivo para {caster.name}");
        
        for (int i = 0; i < validTargets.Count; i++)
        {
            var unit = validTargets[i];
            _view.WriteLine($"{i + 1}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
        }
        
        _view.WriteLine($"{validTargets.Count + 1}-Cancelar");
    }

    private int GetUserChoice(int maxOptions)
    {
        int choice = Convert.ToInt32(_view.ReadLine());
        
        if (choice == maxOptions + 1)
        {
            return -1;
        }
        
        if (choice < 1 || choice > maxOptions)
        {
            throw new ArgumentOutOfRangeException("Opción inválida");
        }
        
        return choice - 1;
    }
}