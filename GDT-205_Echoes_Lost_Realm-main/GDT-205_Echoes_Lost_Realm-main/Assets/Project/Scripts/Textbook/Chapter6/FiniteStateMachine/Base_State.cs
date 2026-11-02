/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_State
{
    protected Ghost ghost = null;

    //-------------------------------------------------------------------------------------

    public Base_State(Ghost g)
    {
        ghost = g;
    }

    //-------------------------------------------------------------------------------------

    public virtual void OnEnter()
    { 
        Debug.Log("BASE_STATE - OnEnter() - Override required");
    }

    //-------------------------------------------------------------------------------------

    public virtual void OnUpdate()
    {
        if (ghost)
        {
            ghost.SetCurrentAIString("BASE_STATE");
        }

        Debug.Log("BASE_STATE - OnUpdate() - Override required");
    }

    //-------------------------------------------------------------------------------------

    public virtual void OnExit()
    {
        Debug.Log("BASE_STATE - OnExit() - Override required");
    }

    //-------------------------------------------------------------------------------------

    public virtual GhostState CheckTransitions()
    {
        Debug.Log("BASE_STATE - CheckTransitions() - Override required");

        return GhostState.Chase;
    }

    //-------------------------------------------------------------------------------------
}
*/