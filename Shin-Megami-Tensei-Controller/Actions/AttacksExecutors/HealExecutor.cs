using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.AttacksExecutors;

public class HealExecutor
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly BaseHeal _healType;
    private readonly AllyTargetSelector _targetSelector;

    public HealExecutor(View view, TurnCalculator turnCalculator, BaseHeal healType)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _healType = healType;
        _targetSelector = new AllyTargetSelector(view, healType);
    }

    public void Execute(Unit caster, Player player)
    {
        try
        {
            var target = _targetSelector.SelectTarget(player, caster);
            
            _view.WriteLine("----------------------------------------");
            _healType.ApplyEffect(caster, target);
            ShowHealMessages(caster, target);
            _turnCalculator.CalculateTurnAfterHeal(player);
            ShowTurnResults();
        }
        catch (OperationCanceledException)
        {
            _view.WriteLine("----------------------------------------");
            throw;
        }
    }

    private void ShowHealMessages(Unit caster, Unit target)
    {
        var description = _healType.GetEffectDescription(caster, target);
        _view.WriteLine(description);
        _view.WriteLine($"{target.name} termina con HP:{target.ActualHP}/{target.stats.HP}");
    }

    private void ShowTurnResults()
    {
        _view.WriteLine("----------------------------------------");
        _view.WriteLine($"Se han consumido {_turnCalculator.FullTurnsConsumed} Full Turn(s) y {_turnCalculator.BlinkingTurnsConsumed} Blinking Turn(s)");
        _view.WriteLine($"Se han obtenido {_turnCalculator.BlinkingTurnsObtained} Blinking Turn(s)");
        _view.WriteLine("----------------------------------------");
    }
}