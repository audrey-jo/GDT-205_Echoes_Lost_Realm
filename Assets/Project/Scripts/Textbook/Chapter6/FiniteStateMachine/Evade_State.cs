using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade_State : Base_State
{
    private Vector2Int evadePos;

    //-------------------------------------------------------------------------------------

    public Evade_State(Ghost g, Vector2Int boardEvadePosition) : base(g)
    {
        evadePos = boardEvadePosition;
    }

    //-------------------------------------------------------------------------------------

    public override void OnEnter()
    {
        if (ghost)
        { 
            ghost.ghostVisuals.SetSpriteSet(SpriteSet.Evading);
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnUpdate()
    {
        //This is super simple - Just set the position we want to go to.
        if (ghost)
        {
            ghost.SetCurrentAIString("Evade_State");

            ghost.SetTargetBoardPosition(evadePos);

            ghost.Move();
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnExit()
    {

    }

    //-------------------------------------------------------------------------------------

    public override GhostState CheckTransitions()
    {
        //EVADE can move into CHASE & MOVETOHOME
        if (ghost)
        {
            if (!ghost.IsPowerPillActive())
                return GhostState.Chase;
            else if (ghost.HasBeenEaten())
                return GhostState.ReturnToHome;
            else
                return GhostState.Evade;
        }
        else
        {
            return GhostState.Evade;
        }
    }

    //-------------------------------------------------------------------------------------
}
