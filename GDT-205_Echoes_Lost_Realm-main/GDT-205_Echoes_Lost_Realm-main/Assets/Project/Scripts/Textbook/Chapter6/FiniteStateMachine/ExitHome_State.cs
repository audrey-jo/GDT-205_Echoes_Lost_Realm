/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHome_State : Base_State
{
    private Vector2Int exitPos = new Vector2Int(14, 11);

    //-------------------------------------------------------------------------------------

    public ExitHome_State(Ghost g) : base(g)
    {

    }

    //-------------------------------------------------------------------------------------

    public override void OnEnter()
    {
    }

    //-------------------------------------------------------------------------------------

    public override void OnUpdate()
    {
        //This is super simple - Just set the position we want to go to.
        if (ghost)
        {
            ghost.SetCurrentAIString("ExitHome_State");

            ghost.SetTargetBoardPosition(exitPos);

            ghost.MoveHome();
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnExit()
    {

    }

    //-------------------------------------------------------------------------------------

    public override GhostState CheckTransitions()
    {
        //EXITHOME can move in to CHASE & EVADE.
        if (ghost)
        {
            if (ghost.GetBoardPosition() == exitPos)
            {
                if (!ghost.IsPowerPillActive())
                {
                    return GhostState.Chase;
                }
                else
                {
                    return GhostState.Evade;
                }
            }
            else
            {
                return GhostState.ExitHome;
            }
        }
        else
        {
            return GhostState.ExitHome;
        }
    }

    //-------------------------------------------------------------------------------------
}*/