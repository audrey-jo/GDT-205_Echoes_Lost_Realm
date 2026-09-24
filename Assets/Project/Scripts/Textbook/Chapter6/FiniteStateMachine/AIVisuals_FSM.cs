/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIVisuals_FSM : MonoBehaviour
{
    [SerializeField] private Ghost ghost = null;

    [SerializeField] private Image exitHomeState    = null;
    [SerializeField] private Image chaseState       = null;
    [SerializeField] private Image evadeState       = null;
    [SerializeField] private Image returnHomeState  = null;

    [SerializeField] private Color activeColour     = Color.black;
    [SerializeField] private Color inactiveColour   = Color.white;

    private string previousAIString = "";

    //-------------------------------------------------------------------------------------

    private void OnEnable()
    {
        DeactivateAll();
    }

    //-------------------------------------------------------------------------------------

    void DeactivateAll()
    {
        exitHomeState.color     = inactiveColour;
        chaseState.color        = inactiveColour;
        evadeState.color        = inactiveColour;
        returnHomeState.color   = inactiveColour;
        previousAIString        = "";
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

                if (currentAIString == "ExitHome_State")
                {
                    exitHomeState.color = activeColour;
                }
                else if (currentAIString == "Chase_State")
                {
                    chaseState.color = activeColour;
                }
                else if (currentAIString == "Evade_State")
                {
                    evadeState.color = activeColour;
                }
                else if (currentAIString == "MoveToHome_State")
                {
                    returnHomeState.color = activeColour;
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------
}
*/