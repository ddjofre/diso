using Shin_Megami_Tensei.Actions.AttackTargetType;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;

namespace Shin_Megami_Tensei.Actions.Factories;


public class TypeTargetFactory
{
    
    private View _view;
    private TurnCalculator _turnCalculator;

    public TypeTargetFactory(View view, TurnCalculator turnCalculator)
    {
        _view = view;
        _turnCalculator = turnCalculator;

    }
    
    
    public BaseTypeTargetAttack CreateTypeTarget(BaseAttack typeAttack){
        
        return new SingleTargetAttack(_view, typeAttack, _turnCalculator);
    }

}