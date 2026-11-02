/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIVisuals_BT : MonoBehaviour
{
    [SerializeField] private Ghost ghost                        = null;

    [SerializeField] private Image root_Node                    = null;
    [SerializeField] private Image isAlive_Node                 = null;
    [SerializeField] private Image isAfraid_Node                = null;
    [SerializeField] private Image goHome_Node                  = null;
    [SerializeField] private Image exitHome_Node1               = null;
    [SerializeField] private Image chase_Node                   = null;
    [SerializeField] private Image evade_Node                   = null;
    [SerializeField] private Image moveHome_Node                = null;
    [SerializeField] private Image exitHome_Node2               = null;
    [SerializeField] private Image moveToPlayer_Node            = null;
    [SerializeField] private Image moveAheadOfPlayer_Node       = null;
    [SerializeField] private Image moveBehindPlayer_Node        = null;
    [SerializeField] private Image moveToRandomPosition_Node    = null;

    [SerializeField] private Color activeColour                 = Color.black;
    [SerializeField] private Color inactiveColour               = Color.white;

    private string previousAIString = "";

    //-------------------------------------------------------------------------------------

    private void OnEnable()
    {
        DeactivateAll();
    }

    //-------------------------------------------------------------------------------------

    void DeactivateAll()
    {
        root_Node.color                 = activeColour; //root is always active.
        isAlive_Node.color              = inactiveColour;
        isAfraid_Node.color             = inactiveColour;
        goHome_Node.color               = inactiveColour;
        exitHome_Node1.color            = inactiveColour;
        chase_Node.color                = inactiveColour;
        evade_Node.color                = inactiveColour;
        moveHome_Node.color             = inactiveColour;
        exitHome_Node2.color            = inactiveColour;
        moveToPlayer_Node.color         = inactiveColour;
        moveAheadOfPlayer_Node.color    = inactiveColour;
        moveBehindPlayer_Node.color     = inactiveColour;
        moveToRandomPosition_Node.color = inactiveColour;
        previousAIString                = "";
    }

    //-------------------------------------------------------------------------------------

    void Update()
    {
        if (ghost)
        {
            string currentAIString = ghost.GetCurrentAIString();

            if (previousAIString != currentAIString)
            {
                DeactivateAll();
                previousAIString = currentAIString;

                string combinedAIString = ghost.GetCombinedAIString();

                if (combinedAIString.Contains("Selector_IsAlive"))
                {
                    isAlive_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Selector_IsAfraid"))
                {
                    isAfraid_Node.color = activeColour;

                    if (combinedAIString.Contains("Leaf_ExitHome"))
                    {
                        exitHome_Node1.color = activeColour;
                    }
                }

                if (combinedAIString.Contains("Sequence_GoHome"))
                {
                    goHome_Node.color = activeColour;

                    if (combinedAIString.Contains("Leaf_ExitHome"))
                    {
                        exitHome_Node2.color = activeColour;
                    }
                }

                if (combinedAIString.Contains("Selector_Chase"))
                {
                    chase_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_Evade"))
                {
                    evade_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_MoveToHome"))
                {
                    moveHome_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_MoveToPlayer"))
                {
                    moveToPlayer_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_MoveAheadOfPlayer"))
                {
                    moveAheadOfPlayer_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_MoveBehindPlayer"))
                {
                    moveBehindPlayer_Node.color = activeColour;
                }

                if (combinedAIString.Contains("Leaf_MoveToRandomPosition"))
                {
                    moveToRandomPosition_Node.color = activeColour;
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------
}
*/