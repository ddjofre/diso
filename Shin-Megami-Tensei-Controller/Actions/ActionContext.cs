using Shin_Megami_Tensei.GameComponents;
using Shin_Megami_Tensei.Units;

namespace Shin_Megami_Tensei.Actions;

public class ActionContext
{
    public Player activePlayer { get; }
    public Player opponentPlayer { get; }
    public Unit actualUnitPlaying { get; }

    public ActionContext(Player activePlayer, Player opponentPlayer, Unit actualUnitPlaying)
    {
        this.activePlayer = activePlayer;
        this.opponentPlayer = opponentPlayer;
        this.actualUnitPlaying = actualUnitPlaying;
    }
}