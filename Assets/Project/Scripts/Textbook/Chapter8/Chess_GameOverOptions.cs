using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chess_GameOverOptions : MonoBehaviour
{
    [SerializeField] private Text winnerText;

    //--------------------------------------------------------------------------------------------

    public void ShowGameOverCanvas()
    {
        gameObject.SetActive(true);
    }

    //--------------------------------------------------------------------------------------------

    public void SetWinner(GameState gameState, PieceColour colour)
    {
        if (winnerText != null)
        {
            if (gameState == GameState.Checkmate)
            {
                if (colour == PieceColour.White)
                {
                    winnerText.text = "WHITE wins!\nCHECKMATE";
                    winnerText.color = Color.white;
                }
                else
                {
                    winnerText.text = "BLACK wins!\nCHECKMATE";
                    winnerText.color = Color.black;
                }
            }
            else if (gameState == GameState.Stalemate)
            {
                winnerText.text = "Its a Draw!\nSTALEMATE";
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void Restart()
    {
        SceneManager.LoadScene("Chapter8_Chess");
    }

    //--------------------------------------------------------------------------------------------
}
