using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Skills;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions.SkillExecutors;

public abstract class BaseSkillExecutor
{
    protected View _view;
    protected TurnCalculator _turnCalculator;
    private SkillFactory _skillFactory;
    public SkillOfensive SkillOfensive;

    public BaseSkillExecutor(View view, TurnCalculator turnCalculator)
    {
        this._view = view;
        this._turnCalculator = turnCalculator;
        this._skillFactory = new SkillFactory(view, turnCalculator);
    }


    public SkillOfensive CreateSkill(SkillInfo skillInfo)
    {
        SkillOfensive skillOfensive = _skillFactory.CreateSkillFromMap(skillInfo.name, skillInfo);
        SkillOfensive = skillOfensive;
        return skillOfensive;
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

    public abstract void ExecuteSkill(Unit actualUnitPlaying, Player playerRival, Player player);
    
}