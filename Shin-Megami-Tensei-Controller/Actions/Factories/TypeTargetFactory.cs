using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Actions.AttackTypes;
using Shin_Megami_Tensei.Actions.TargetTypes;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Enumerates;

namespace Shin_Megami_Tensei.Actions.Factories;


public class TypeTargetFactory
{
        
    private View _view;

    public TypeTargetFactory(View view)
    {
        _view = view;
    }
    
    public BaseTypeTarget CreateTypeTarget(TypeTarget typeTarget)
    {
        if (typeTarget == TypeTarget.Single)
        {
            return new SingleTypeTarget(_view, TypeTarget.Single);
        }
        
        else
        {
            throw new NotImplementedException();
        }
        
    }

    
}