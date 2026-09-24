/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIntroCanvas : MonoBehaviour
{
    [SerializeField] private GameObject IntroCanvas = null;
    [SerializeField] private GameObject GameCanvas = null;
    [SerializeField] private GA GAManager = null;
    [SerializeField] private UserControlledShip userControlledShip = null;

    private GameData gameData = null;

    //------------------------------------------------------------------------------------------------------

    private void Start()
    {
        gameData = FindObjectOfType<GameData>();

        ResetIntroMenu();
    }

    //------------------------------------------------------------------------------------------------------

    public void ResetIntroMenu()
    {
        if (gameData != null)
        {
            gameData.bGameActive = false;
        }

        //Enable intro canvas.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(true);
        }

        //Disable game canvas.
        if (GameCanvas != null)
        {
            GameCanvas.SetActive(false);
        }
    }

    //------------------------------------------------------------------------------------------------------

    public void StartGA()
    {
        ResetGA();
    }

    //------------------------------------------------------------------------------------------------------

    void ResetGA()
    {
        //Enable GA.
        if (GAManager != null)
        {
            GAManager.ResetGA(); 
            GAManager.gameObject.SetActive(true);
        }

        //Disable User.
        if (userControlledShip != null)
        {
            userControlledShip.gameObject.SetActive(false);
        }

        //Set game to active.
        if (gameData != null)
        {
            gameData.bGameActive = true;
        }

        //Disable intro canvas.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(false);
        }

        //Enable game canvas.
        if (GameCanvas != null)
        {
            GameCanvas.SetActive(true);
        }
    }

    //------------------------------------------------------------------------------------------------------

    public void StartUserControlledGame()
    {
        ResetUser();
    }

    //------------------------------------------------------------------------------------------------------

    void ResetUser()
    {
        //Disable GA.
        if (GAManager != null)
        {
            GAManager.gameObject.SetActive(false);
        }

        //Enable User.
        if (userControlledShip != null)
        {
            userControlledShip.ResetUserControlledShip();
            userControlledShip.gameObject.SetActive(true);
        }

        //Set game to active.
        if (gameData != null)
        {
            gameData.bGameActive = true;
        }

        //Disable intro canvas.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(false);
        }

        //Enable game canvas.
        if (GameCanvas != null)
        {
            GameCanvas.SetActive(true);
        }
    }

    //------------------------------------------------------------------------------------------------------
}
*/