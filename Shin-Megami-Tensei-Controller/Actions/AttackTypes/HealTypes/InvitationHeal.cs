namespace Shin_Megami_Tensei.Actions.AttackTypes.HealTypes;

using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.Battle;
using Shin_Megami_Tensei.Units;


public class InvitationHeal : BaseHeal, IInvocationHeal
{
    public InvitationHeal(View view, TurnCalculator turnCalculator) : base(view, turnCalculator)
    {
    }

    public bool CanInvokeMonster() => true;
    public bool CanReviveMonster() => true;

    public override void ApplyHeal(Unit target)
    {
        // Invitation no aplica heal tradicional, maneja invocación
        // Este método no se usará directamente
    }

    public override bool CanTargetUnit(Unit target)
    {
        // Invitation puede seleccionar cualquier monstruo de la reserva
        return true;
    }

    public override void GetHealMessage(Unit attacker, Unit target)
    {
        // Los mensajes se manejan en el ejecutor especial
    }
}