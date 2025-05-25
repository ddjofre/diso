using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.Factories;

public class FinalSkillFactory
{
    private View _view;
    private TurnCalculator _turnCalculator;
    private TypeAttackFactory _typeAttackFactory;
    private FinalTypeTargetFactory _typeTargetFactory;
    private Dictionary<string, (TypeAttack, TypeTarget)> _skillMap;

    public FinalSkillFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _typeAttackFactory = new TypeAttackFactory(_view, _turnCalculator);
        _typeTargetFactory = new FinalTypeTargetFactory(_view, _turnCalculator);
        _skillMap = new Dictionary<string, (TypeAttack, TypeTarget)>
        
        {
            { "Lunge", (TypeAttack.Phys, TypeTarget.Single) },
            { "Oni-Kagura", (TypeAttack.Phys, TypeTarget.Single) },
            { "Mortal Jihad", (TypeAttack.Phys, TypeTarget.Single) },
            { "Gram Slice", (TypeAttack.Phys, TypeTarget.Single) },
            { "Fatal Sword", (TypeAttack.Phys, TypeTarget.Single) },
            { "Berserker God", (TypeAttack.Phys, TypeTarget.Single) },
            { "Bouncing Claw", (TypeAttack.Phys, TypeTarget.Single) },
            { "Damascus Claw", (TypeAttack.Phys, TypeTarget.Single) },
            { "Nihil Claw", (TypeAttack.Phys, TypeTarget.Single) },
            { "Axel Claw", (TypeAttack.Phys, TypeTarget.Single) },
            { "Iron Judgement", (TypeAttack.Phys, TypeTarget.Single) },
            { "Stigma Attack", (TypeAttack.Phys, TypeTarget.Single) },

            { "Needle Shot", (TypeAttack.Gun, TypeTarget.Single) },
            { "Tathlum Shot", (TypeAttack.Gun, TypeTarget.Single) },
            { "Grand Tack", (TypeAttack.Gun, TypeTarget.Single) },
            { "Riot Gun", (TypeAttack.Gun, TypeTarget.Single) },

            { "Agi", (TypeAttack.Fire, TypeTarget.Single) },
            { "Agilao", (TypeAttack.Fire, TypeTarget.Single) },
            { "Agidyne", (TypeAttack.Fire, TypeTarget.Single) },
            { "Trisagion", (TypeAttack.Fire, TypeTarget.Single) },

            { "Bufu", (TypeAttack.Ice, TypeTarget.Single) },
            { "Bufula", (TypeAttack.Ice, TypeTarget.Single) },
            { "Bufudyne", (TypeAttack.Ice, TypeTarget.Single) },

            { "Zio", (TypeAttack.Elec, TypeTarget.Single) },
            { "Zionga", (TypeAttack.Elec, TypeTarget.Single) },
            { "Ziodyne", (TypeAttack.Elec, TypeTarget.Single) },

            { "Zan", (TypeAttack.Force, TypeTarget.Single) },
            { "Zanma", (TypeAttack.Force, TypeTarget.Single) },
            { "Zandyne", (TypeAttack.Force, TypeTarget.Single) },
            { "Deadly Wind", (TypeAttack.Force, TypeTarget.Single) },
        };
    }
    
    

    public FinalSkill CreateFinalSkillFromMap(string skillName, SkillInfo skillInfo)
    {
        if (_skillMap.TryGetValue(skillName, out var types))
        {
            return CreateFinalSkill(skillInfo, types.Item1, types.Item2);
        }
        else
        {
            throw new KeyNotFoundException($"Skill {skillName} not found");
        }
    }

    private FinalSkill CreateFinalSkill(SkillInfo skillInfo, TypeAttack typeAttack, TypeTarget typeTarget)
    {
        var attack = _typeAttackFactory.CreateTypeAttack(typeAttack);
        attack.powerSkill = skillInfo.power;
        attack.isAttackInHabilitie = true;

        var target = _typeTargetFactory.CreateTypeTarget(typeTarget);

        var attackExecutor = new AttackExecutor(attack, target, _turnCalculator, _view);
        
        return new FinalSkill(skillInfo, attackExecutor);
    }
    
    
}