using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.Factories;

public class SkillHealFactory
{
    private View _view;
    private TurnCalculator _turnCalculator;
    private TypeAttackFactory _typeAttackFactory;
    private TypeTargetFactory _typeTargetFactory;
    private Dictionary<string, (TypeHeal, TypeTarget)> _skillMap;

    public SkillHealFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _typeAttackFactory = new TypeAttackFactory(_view, _turnCalculator);
        _typeTargetFactory = new TypeTargetFactory(_view, _turnCalculator);
        _skillMap = new Dictionary<string, (TypeHeal, TypeTarget)>

        {
            { "Dia", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Diarama", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Diarahan", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Recarm", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Samarecarm", (TypeHeal.Dia, TypeTarget.Ally) },
            { "Invitation", (TypeHeal.Dia, TypeTarget.Ally) }
        };
    }
    
    
    /*
    public Skill CreateSkillFromMap(string skillName, SkillInfo skillInfo)
    {
        if (_skillMap.TryGetValue(skillName, out var types))
        {
            return CreateSkill(skillInfo, types.Item1, types.Item2);
        }
        else
        {
            throw new KeyNotFoundException($"Skill {skillName} not found");
        }
    }

    
    
    
    private Skill CreateSkill(SkillInfo skillInfo, TypeHeal typeHeal, TypeTarget typeTarget)
    {
        var healAttack = _typeAttackFactory.CreateTypeHealAttack(typeHeal);
        healAttack.powerSkill = skillInfo.power;

        var target = _typeTargetFactory.CreateTypeTarget(typeTarget);
        
        var attackExecutor = new HealAttackExecutor(healAttack, target, _turnCalculator, _view);
        
        return new Skill(skillInfo, attackExecutor);
        
    }
    
    */
    
    
    
    
    
    
    
    
}