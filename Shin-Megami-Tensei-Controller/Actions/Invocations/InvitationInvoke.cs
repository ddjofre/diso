using Shin_Megami_Tensei_View;
using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.GameComponents.PlayerComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions.Invocations;

public class InvitationInvoke : Invoke
{
    private View _view;

    public InvitationInvoke(View view) : base(view)
    {
        _view = view;
    }

    // Override para permitir tanto unidades vivas como muertas
    public override bool MeetsConditionToBeenInvoke(Unit unit)
    {
        // Invitation puede invocar tanto unidades vivas como muertas
        return true;
    }

    public override void MakeInvoke(Player player, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Team team = player.Team;
        Unit monsterToReplace = team.UnitsInGame[indexMonsterToReplace];
        Unit monsterToInvoke;
        bool isFromReserve;
        
        // Determinar si viene de la reserva o del tablero
        if (indexMonsterToInvoke >= 0)
        {
            // Viene de la reserva
            monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke];
            isFromReserve = true;
        }
        else
        {
            // Viene del tablero (índice negativo)
            int realIndex = Math.Abs(indexMonsterToInvoke) - 1;
            monsterToInvoke = team.UnitsInGame[realIndex];
            isFromReserve = false;
        }
        
        bool isEmptyPosition = (monsterToReplace.ActualHP == 0);
        bool wasMonsterDead = monsterToInvoke.ActualHP == 0;

        // Si el monstruo estaba muerto, revivirlo con HP completo
        if (wasMonsterDead)
        {
            monsterToInvoke.ActualHP = monsterToInvoke.stats.HP;
            monsterToInvoke.HasBeenRecarm = true;
        }

        monsterToInvoke.HasBeenIvoked = true;
        monsterToReplace.HasBeenReplaceInInvoke = true;
        
        if (isFromReserve)
        {
            // Intercambio normal reserva <-> tablero
            team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
            team.UnitsInReserve[indexMonsterToInvoke] = monsterToReplace;
        }
        else
        {
            // Viene del tablero - mover a la posición seleccionada
            int realSourceIndex = Math.Abs(indexMonsterToInvoke) - 1;
            team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
            team.UnitsInGame[realSourceIndex] = monsterToReplace;
        }
        
        ChangeParameterHasBeenInvokeInMonsterList(team, monsterToInvoke);
        ChangeParameterHasBeenReplaceInMonsterList(team, monsterToReplace);
        
        if (isFromReserve)
        {
            OrderUnitsInReserveByOrderMonsterList(team);
        }
        
        // Si era un puesto vacío, agregar el índice al final del orden
        if (isEmptyPosition && !team.indexesOrderAttack.Contains(indexMonsterToReplace))
        {
            team.indexesOrderAttack.Add(indexMonsterToReplace);
        }
        
        _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
    }
}