using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------
//                           FSM Structure
//-------------------------------------------------------------
//
//                 -----------                -----------
//                 |  EXIT_  | -------------> |  CHASE  |
//                 |  HOME   | -------        -----------
//                 -----------       |            |   /\   
//                     /\            |            |   |
//                     |             |           \/   |
//              ----------------     |        -----------
//              |    MOVE_     |     |------->|  EVADE  |
//              |    TO_HOME   | <------------|         |
//              ----------------              -----------
//
//--------------------------------------------------------------

public enum GhostState
{
    Chase,
    Evade,
    ReturnToHome,
    ExitHome
};

//-------------------------------------------------------------------------------------------
[RequireComponent(typeof(Ghost))]
public class FiniteStateMachine : MonoBehaviour
{
    private Ghost               ghost               = null;
    private Player              player              = null;

    private List<Base_State>    availableStates     = new List<Base_State>();
    private GhostState          currentState        = GhostState.ExitHome;

    //Thes public variables make the ghosts act differently.
    public ChaseType            typeOfChase         = ChaseType.PlayerPosition;
    public Vector2Int           positionToEvadeTo   = new Vector2Int();

    //-------------------------------------------------------------------------------------------

    void Start()
    {
        ghost  = GetComponent<Ghost>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        availableStates.Add(new Chase_State(ghost, typeOfChase, player));
        availableStates.Add(new Evade_State(ghost, positionToEvadeTo));
        availableStates.Add(new MoveToHome_State(ghost));
        availableStates.Add(new ExitHome_State(ghost));

        //To kick things off we need to enter the start state.
        availableStates[(int)currentState].OnEnter();
    }

    //-------------------------------------------------------------------------------------------

    void Update()
    {
        if (GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            //Do we need to transition out of the current state.
            GhostState nextState = availableStates[(int)currentState].CheckTransitions();
            if (nextState != currentState)
            {
                //Exit current state.
                availableStates[(int)currentState].OnExit();

                //Enter next state.
                availableStates[(int)nextState].OnEnter();

                //Remember the change.
                currentState = nextState;
            }

            //Update the current state.
            availableStates[(int)currentState].OnUpdate();
        }
    }

    //-------------------------------------------------------------------------------------------
}
