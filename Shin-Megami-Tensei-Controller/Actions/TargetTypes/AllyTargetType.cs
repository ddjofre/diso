using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;
using Shin_Megami_Tensei.Enumerates;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.TargetTypes;

public class AllyTargetType: SingleTypeTarget
{
    private readonly View _view;
    public TypeTarget targetType;
    private BaseHeal _healType;
    
    public AllyTargetType(View view, TypeTarget typeTarget, BaseHeal healType): base(view, typeTarget){
        
        _view = view; 
        targetType = typeTarget;
        _healType = healType;
    }
    public override bool MeetCondition(Unit target)
    {
        return _healType.CanTargetUnit(target);
    }
    
    
    
}