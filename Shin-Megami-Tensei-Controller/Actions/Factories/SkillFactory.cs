using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.Factories;

public class SkillFactory {
    
    private View _view;
    private TurnCalculator _turnCalculator;
    private TypeAttackFactory _typeAttackFactory;
    private TypeTargetFactory _typeTargetFactory;
    private Dictionary<string, TypeAttack> _skillMap;

    public SkillFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _typeAttackFactory = new TypeAttackFactory(_view, _turnCalculator);
        _typeTargetFactory = new TypeTargetFactory(_view, _turnCalculator);
        _skillMap = new Dictionary<string, TypeAttack>
        {
            { "Lunge", TypeAttack.Phys },
            { "Oni-Kagura", TypeAttack.Phys },
            { "Mortal Jihad", TypeAttack.Phys },
            { "Gram Slice", TypeAttack.Phys },
            { "Fatal Sword", TypeAttack.Phys },
            { "Berserker God", TypeAttack.Phys },
            { "Bouncing Claw", TypeAttack.Phys },
            { "Damascus Claw", TypeAttack.Phys },
            { "Nihil Claw", TypeAttack.Phys },
            { "Axel Claw", TypeAttack.Phys },
            { "Iron Judgement", TypeAttack.Phys },
            { "Stigma Attack", TypeAttack.Phys },

            { "Needle Shot", TypeAttack.Gun },
            { "Tathlum Shot", TypeAttack.Gun },
            { "Grand Tack", TypeAttack.Gun },
            { "Riot Gun", TypeAttack.Gun },

            { "Agi", TypeAttack.Fire },
            { "Agilao", TypeAttack.Fire },
            { "Agidyne", TypeAttack.Fire },
            { "Trisagion", TypeAttack.Fire },

            { "Bufu", TypeAttack.Ice },
            { "Bufula", TypeAttack.Ice },
            { "Bufudyne", TypeAttack.Ice },

            { "Zio", TypeAttack.Elec },
            { "Zionga", TypeAttack.Elec },
            { "Ziodyne", TypeAttack.Elec },

            { "Zan", TypeAttack.Force },
            { "Zanma", TypeAttack.Force },
            { "Zandyne", TypeAttack.Force },
            { "Deadly Wind", TypeAttack.Force },
        };
    }

    public Skill CreateSkillFromMap(string skillName, SkillInfo skillInfo)
    {
        if (_skillMap.TryGetValue(skillName, out var typeAttack))
        {
            return CreateSkill(skillInfo, typeAttack);
        }
        else
        {
            throw new KeyNotFoundException($"Skill {skillName} not found");
        }
    }

    private Skill CreateSkill(SkillInfo skillInfo, TypeAttack typeAttack)
    {
        var attack = _typeAttackFactory.CreateTypeAttack(typeAttack);
        attack.powerSkill = skillInfo.power;
        attack.isAttackInHabilitie = true;
        var targetAttack = _typeTargetFactory.CreateTypeTarget(attack);
        return new Skill(skillInfo, targetAttack);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}