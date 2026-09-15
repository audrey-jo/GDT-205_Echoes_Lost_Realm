using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyIntroOptions : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Canvas gameCanvas;

    [SerializeField] private Environment environment;

    //--------------------------------------------------------------------------------------------------

    public void StartUserVsUserGame()
    {
        if(introCanvas != null)
        {
            introCanvas.gameObject.SetActive(false);
        }

        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }
    }

    //--------------------------------------------------------------------------------------------------

    public void StartAiVsUserGame()
    {
        if(environment != null)
        {
            environment.SetAIGame();
        }

        if (introCanvas != null)
        {
            introCanvas.gameObject.SetActive(false);
        }

        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }
    }

    //--------------------------------------------------------------------------------------------------
}
