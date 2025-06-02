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

    public override bool MeetsConditionToBeenInvoke(Unit unit)
    {
        return true;
    }

    public override List<int> ShowUnitsInReserve(Team team)
    {
        OrderUnitsInReserveByOrderMonsterList(team);
        _view.WriteLine("Seleccione un monstruo para invocar");

        List<int> validIndexes = new List<int>();
        int displayNumber = 1;

        for (int i = 0; i < team.UnitsInReserve.Length; i++)
        {
            var unit = team.UnitsInReserve[i];
            if (unit != null)
            {
                _view.WriteLine($"{displayNumber}-{unit.name} HP:{unit.ActualHP}/{unit.stats.HP} MP:{unit.ActualMP}/{unit.stats.MP}");
                validIndexes.Add(i);
                displayNumber++;
            }
        }

        _view.WriteLine($"{displayNumber}-Cancelar");
        return validIndexes;
    }

    public override void MakeInvoke(Player player, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Team team = player.Team;
        (Unit monsterToInvoke, Unit monsterToReplace, bool isFromReserve) = GetInvokeUnits(team, indexMonsterToInvoke, indexMonsterToReplace);

        bool isEmptyPosition = IsEmptyPosition(monsterToReplace);
        bool wasMonsterDead = IsMonsterDead(monsterToInvoke);

        if (wasMonsterDead)
        {
            ReviveMonster(monsterToInvoke);
        }

        UpdateUnitsForInvitation(team, indexMonsterToInvoke, indexMonsterToReplace, monsterToInvoke, monsterToReplace, isFromReserve);
        UpdateMonsterStatus(team, monsterToInvoke, monsterToReplace);

        if (isFromReserve)
        {
            OrderUnitsInReserveByOrderMonsterList(team);
        }

        if (isEmptyPosition)
        {
            UpdateAttackOrder(team, indexMonsterToReplace);
        }

        _view.WriteLine($"{monsterToInvoke.name} ha sido invocado");
    }
    

    private (Unit monsterToInvoke, Unit monsterToReplace, bool isFromReserve) GetInvokeUnits(Team team, int indexMonsterToInvoke, int indexMonsterToReplace)
    {
        Unit monsterToReplace = team.UnitsInGame[indexMonsterToReplace];
        Unit monsterToInvoke;
        bool isFromReserve;

        if (indexMonsterToInvoke >= 0)
        {
            monsterToInvoke = team.UnitsInReserve[indexMonsterToInvoke];
            isFromReserve = true;
        }
        else
        {
            int realIndex = Math.Abs(indexMonsterToInvoke) - 1;
            monsterToInvoke = team.UnitsInGame[realIndex];
            isFromReserve = false;
        }

        return (monsterToInvoke, monsterToReplace, isFromReserve);
    }

    private bool IsMonsterDead(Unit unit)
    {
        return unit.ActualHP == 0;
    }

    private void ReviveMonster(Unit unit)
    {
        unit.ActualHP = unit.stats.HP;
        unit.HasBeenRecarm = true;
    }

    private void UpdateUnitsForInvitation(Team team, int indexMonsterToInvoke, int indexMonsterToReplace, Unit monsterToInvoke, Unit monsterToReplace, bool isFromReserve)
    {
        if (isFromReserve)
        {
            team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
            team.UnitsInReserve[indexMonsterToInvoke] = monsterToReplace;
        }
        else
        {
            int realSourceIndex = Math.Abs(indexMonsterToInvoke) - 1;
            team.UnitsInGame[indexMonsterToReplace] = monsterToInvoke;
            team.UnitsInGame[realSourceIndex] = monsterToReplace;
        }
    }
}
