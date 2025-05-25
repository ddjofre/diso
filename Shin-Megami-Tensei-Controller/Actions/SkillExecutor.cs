using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.Factories;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;
using Shin_Megami_Tensei.Units.UnitComponents;

namespace Shin_Megami_Tensei.Actions;

public class SkillExecutor
{
    private View _view;
    private TurnCalculator _turnCalculator;
    private SkillFactory _skillFactory;
    private Skill _skill;

    public SkillExecutor(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;
        _skillFactory = new SkillFactory(_view, _turnCalculator );
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
        Skill skill = _skillFactory.CreateFinalSkillFromMap(skillInfo.name, skillInfo);
        _skill = skill;
        return skill;
    }


    public void ExecuteSkill(Unit actualUnitPlaying, Player playerRival, Player player)
    {
        _skill._attackExecutor.ShowAvailableTargets(playerRival, actualUnitPlaying);
        List<int> targetsIndexes = _skill._attackExecutor.GetTargets(playerRival);
                
        Unit target = _skill._attackExecutor.GetRival(targetsIndexes[0], playerRival);
        
        _skill._attackExecutor.Execute( target, actualUnitPlaying, player, playerRival, targetsIndexes);
        _skill.DiscountMP(actualUnitPlaying);
        
    }
    
    
    
    
    
    
    
}