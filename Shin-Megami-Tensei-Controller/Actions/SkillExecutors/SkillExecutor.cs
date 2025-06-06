﻿using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttacksExecutors;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Skills;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.SkillExecutors;


public class SkillExecutor
{
    
    private View _view;
    private TurnCalculator _turnCalculator;

    public SkillExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
    }

    public int ShowAvailableSkills(Unit actualUnitPlaying)
    {
        _view.WriteLine($"Seleccione una habilidad para que {actualUnitPlaying.name} use");

        int numVecesQueSeHizoPrint = 0;
            
        for (int i = 0; i < actualUnitPlaying.skillInfo.Count; i++)
        {
            if (actualUnitPlaying.skillInfo[i].cost <= actualUnitPlaying.ActualMP)
            {
                _view.WriteLine($"{i + 1}-{actualUnitPlaying.skillInfo[i].name} MP:{actualUnitPlaying.skillInfo[i].cost}");
                numVecesQueSeHizoPrint++;
            }
        }
            
        _view.WriteLine($"{numVecesQueSeHizoPrint + 1}-Cancelar");
        return numVecesQueSeHizoPrint;
    }
    
    public Skill CreateSkill(SkillInfo skillInfo)
    {
        if (new[] { "Phys", "Elec", "Fire", "Force", "Gun", "Ice" }.Contains(skillInfo.type))
        {
            var skillOffensiveFactory = new SkillOffensiveFactory(_view, _turnCalculator);
            return skillOffensiveFactory.CreateSkillFromMap(skillInfo.name, skillInfo);
        }
        
        else if (skillInfo.type.Equals("Heal"))
        {
            var healSkillFactory = new HealSkillFactory(_view, _turnCalculator);
            return healSkillFactory.CreateHealSkill(skillInfo);
        }
        
        else if (skillInfo.type.Equals("Special"))
        {
            if (skillInfo.name == "Sabbatma")
            {
                var sabbatmaExecutor = new SabbatmaExecutor(_view, _turnCalculator);
                return new SkillSpecial(skillInfo, sabbatmaExecutor);
            }
            throw new NotImplementedException($"Special skill {skillInfo.name} not implemented");
        }
        else
        {
            throw new NotImplementedException($"Skill type {skillInfo.type} not implemented");
        }
    }
    
}