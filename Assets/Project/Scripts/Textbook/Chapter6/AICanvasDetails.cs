using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICanvasDetails : MonoBehaviour
{
    private GhostHandler                ghostHandler       = null;

    [SerializeField] private Text       details_red        = null;
    [SerializeField] private GameObject fsmVisuals_red     = null;
    [SerializeField] private GameObject btVisuals_red      = null;

    [SerializeField] private Text       details_pink       = null;
    [SerializeField] private GameObject fsmVisuals_pink    = null;
    [SerializeField] private GameObject btVisuals_pink     = null;

    [SerializeField] private Text       details_yellow     = null;
    [SerializeField] private GameObject fsmVisuals_yellow  = null;
    [SerializeField] private GameObject btVisuals_yellow   = null;

    [SerializeField] private Text       details_blue       = null;
    [SerializeField] private GameObject fsmVisuals_blue    = null;
    [SerializeField] private GameObject btVisuals_blue     = null;

    //-------------------------------------------------------------------------------------

    private void Start()
    {
        ghostHandler = GameObject.FindGameObjectWithTag("GhostHandler").GetComponent<GhostHandler>();
    }

    //-------------------------------------------------------------------------------------

    private void Update()
    {
        UpdateRedDetails();
        UpdatePinkDetails();
        UpdateYellowDetails();
        UpdateBlueDetails();
    }

    //-------------------------------------------------------------------------------------

    void UpdateRedDetails()
    {
        if(details_red)
        {
            if (ghostHandler.GetCurrentAIApproach() == AITechnique.FSM)
            {
                fsmVisuals_red.SetActive(true);
                btVisuals_red.SetActive(false);
                details_red.text = "AI: FSM" + "\n" + "State: ";
            }
            else if (ghostHandler.GetCurrentAIApproach() == AITechnique.BT)
            {
                fsmVisuals_red.SetActive(false);
                btVisuals_red.SetActive(true);
                details_red.text = "AI: BT" + "\n" + "Node: ";
            }

            details_red.text += ghostHandler.GetGhost(GhostColour.Red).GetCurrentAIString();
            details_red.text += "\n" + "Position: " + ghostHandler.GetGhost(GhostColour.Red).GetBoardPosition();
            details_red.text += "\n" + "Target: " + ghostHandler.GetGhost(GhostColour.Red).GetTargetBoardPosition();
        }
    }

    //-------------------------------------------------------------------------------------

    void UpdatePinkDetails()
    {
        if (details_pink)
        {
            if (ghostHandler.GetCurrentAIApproach() == AITechnique.FSM)
            {
                fsmVisuals_pink.SetActive(true);
                btVisuals_pink.SetActive(false);
                details_pink.text = "AI: FSM" + "\n" + "State: ";
            }
            else if (ghostHandler.GetCurrentAIApproach() == AITechnique.BT)
            {
                fsmVisuals_pink.SetActive(false);
                btVisuals_pink.SetActive(true);
                details_pink.text = "AI: BT" + "\n" + "Node: ";
            }

            details_pink.text += ghostHandler.GetGhost(GhostColour.Pink).GetCurrentAIString();
            details_pink.text += "\n" + "Position: " + ghostHandler.GetGhost(GhostColour.Pink).GetBoardPosition();
            details_pink.text += "\n" + "Target: " + ghostHandler.GetGhost(GhostColour.Pink).GetTargetBoardPosition();
        }
    }

    //-------------------------------------------------------------------------------------

    void UpdateYellowDetails()
    {
        if (details_yellow)
        {
            if (ghostHandler.GetCurrentAIApproach() == AITechnique.FSM)
            {
                fsmVisuals_yellow.SetActive(true);
                btVisuals_yellow.SetActive(false);
                details_yellow.text = "AI: FSM" + "\n" + "State: ";
            }
            else if (ghostHandler.GetCurrentAIApproach() == AITechnique.BT)
            {
                fsmVisuals_yellow.SetActive(false);
                btVisuals_yellow.SetActive(true);
                details_yellow.text = "AI: BT" + "\n" + "Node: ";
            }

            details_yellow.text += ghostHandler.GetGhost(GhostColour.Yellow).GetCurrentAIString();
            details_yellow.text += "\n" + "Position: " + ghostHandler.GetGhost(GhostColour.Yellow).GetBoardPosition();
            details_yellow.text += "\n" + "Target: " + ghostHandler.GetGhost(GhostColour.Yellow).GetTargetBoardPosition();
        }
    }

    //-------------------------------------------------------------------------------------

    void UpdateBlueDetails()
    {
        if (details_blue)
        {
            if (ghostHandler.GetCurrentAIApproach() == AITechnique.FSM)
            {
                fsmVisuals_blue.SetActive(true);
                btVisuals_blue.SetActive(false);
                details_blue.text = "AI: FSM" + "\n" + "State: ";
            }
            else if (ghostHandler.GetCurrentAIApproach() == AITechnique.BT)
            {
                fsmVisuals_blue.SetActive(false);
                btVisuals_blue.SetActive(true);
                details_blue.text = "AI: BT" + "\n" + "Node: ";
            }

            details_blue.text += ghostHandler.GetGhost(GhostColour.Blue).GetCurrentAIString();
            details_blue.text += "\n" + "Position: " + ghostHandler.GetGhost(GhostColour.Blue).GetBoardPosition();
            details_blue.text += "\n" + "Target: " + ghostHandler.GetGhost(GhostColour.Blue).GetTargetBoardPosition();
        }
    }

    //-------------------------------------------------------------------------------------
}
