using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Skills;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.Factories;

public class SkillHealFactory
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly TypeAttackFactory _typeAttackFactory;
    private readonly Dictionary<string, (TypeHeal, TypeTarget)> _skillMap;

    public SkillHealFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _typeAttackFactory = new TypeAttackFactory(_view, _turnCalculator);
        _skillMap = new Dictionary<string, (TypeHeal, TypeTarget)>
        {
            { "Dia", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Diarama", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Diarahan", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Recarm", (TypeHeal.Recarm, TypeTarget.Ally) },
            { "Samarecarm", (TypeHeal.Recarm, TypeTarget.Ally) },
            { "Invitation", (TypeHeal.Recarm, TypeTarget.Ally) }
        };
    }

    public SkillHeal CreateSkillFromMap(string skillName, SkillInfo skillInfo)
    {
        if (_skillMap.TryGetValue(skillName, out var types))
        {
            return CreateSkill(skillInfo, types.Item1, types.Item2);
        }
        else
        {
            throw new KeyNotFoundException($"Heal Skill {skillName} not found");
        }
    }

    private SkillHeal CreateSkill(SkillInfo skillInfo, TypeHeal typeHeal, TypeTarget typeTarget)
    {
        var healAttack = _typeAttackFactory.CreateTypeHealAttack(typeHeal);
        healAttack.powerSkill = skillInfo.power;

        var target = new AllyTargetType(_view, typeTarget, healAttack);
        var attackExecutor = new HealAttackExecutor(healAttack, target, _turnCalculator, _view);
        
        return new SkillHeal(skillInfo, attackExecutor);
    }
}