/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------

public class GhostHandler : MonoBehaviour
{
    public const float kAfraidDuration   = 5.0f;
    public AITechnique currentAIApproach = AITechnique.FSM;

    [SerializeField] private List<Ghost>                    ghosts     = new List<Ghost>();
    [SerializeField] private List<FiniteStateMachine>       ghostFSMs  = new List<FiniteStateMachine>();
    [SerializeField] private List<BehaviourTree>            ghostBTs   = new List<BehaviourTree>();

    //-------------------------------------------------------------------------------------------

    public Ghost GetGhost(GhostColour colour)
    {
        return ghosts[(int)colour];
    }

    //-------------------------------------------------------------------------------------------

    public AITechnique GetCurrentAIApproach()
    {
        return currentAIApproach;
    }

    //-------------------------------------------------------------------------------------------

    public FiniteStateMachine GetGhostFSM(GhostColour colour)
    {
        return ghostFSMs[(int)colour];
    }

    //-------------------------------------------------------------------------------------------

    public BehaviourTree GetGhostBT(GhostColour colour)
    {
        return ghostBTs[(int)colour];
    }

    //-------------------------------------------------------------------------------------------

    public void SetGhostsAfraid()
    {
        for (int i = 0; i < ghosts.Count; i++)
        {
            ghosts[i].SetAfraid(kAfraidDuration);
        }
    }

    //-------------------------------------------------------------------------------------------

    private void Update()
    {
        //Press SPACEBAR to switch between approaches.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentAIApproach = currentAIApproach == AITechnique.FSM ? AITechnique.BT : AITechnique.FSM;

            //Ensure the correct approach is set up.
            if (currentAIApproach == AITechnique.FSM)
            {
                for (int i = 0; i < ghosts.Count; i++)
                {
                    ghostFSMs[i].enabled = true;
                    ghostBTs[i].enabled = false;
                }
            }
            else if (currentAIApproach == AITechnique.BT)
            {
                for (int i = 0; i < ghosts.Count; i++)
                {
                    ghostFSMs[i].enabled = false;
                    ghostBTs[i].enabled = true;
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameWorld.GamePaused = !GameWorld.GamePaused;
        }
    }

    //-------------------------------------------------------------------------------------------
}
*/