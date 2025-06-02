using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Skills;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.Factories;

public class HealSkillFactory
{
    private readonly View _view;
    private readonly TurnCalculator _turnCalculator;
    private readonly Dictionary<string, Func<BaseHeal>> _healMap;

    public HealSkillFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        
        _healMap = new Dictionary<string, Func<BaseHeal>>
        {
            { "Dia", () => new DiaHeal(_view, _turnCalculator) },
            { "Diarama", () => new DiaHeal(_view, _turnCalculator) },
            { "Diarahan", () => new DiaHeal(_view, _turnCalculator) },
            { "Recarm", () => new RecarmHeal(_view, _turnCalculator) },
            { "Samarecarm", () => new RecarmHeal(_view, _turnCalculator) },
            { "Invitation", () => new InvitationHeal(_view, _turnCalculator) }
        };
    }

    public Skill CreateHealSkill(SkillInfo skillInfo)
    {
        if (skillInfo.name == "Invitation")
        {
            var invitationExecutor = new InvitationExecutor(_view, _turnCalculator);
            return new SkillHealInvitation(skillInfo, invitationExecutor);
        }

        else if (_healMap.TryGetValue(skillInfo.name, out var healFactory))
        {
            var healType = healFactory();
            healType.powerSkill = skillInfo.power;
            var executor = new HealExecutor(_view, _turnCalculator, healType);
            return new SkillHeal(skillInfo, executor);
        }
        else
        {
            throw new KeyNotFoundException($"Skill {skillInfo.name} not found");
        }
    }
}