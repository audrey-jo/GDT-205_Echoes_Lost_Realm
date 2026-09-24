/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_MenuOptions : MonoBehaviour
{
    [SerializeField] private Canvas startCanvas = null;
    [SerializeField] private Canvas gameCanvas = null;

    [SerializeField] private ChessPlayer player1Human;
    [SerializeField] private ChessPlayer player2Human;
    [SerializeField] private ChessPlayerAI player2Ai;

    //--------------------------------------------------------------------------------------------

    public void Start1PlayerGame()
    {
        if(player1Human != null && player2Ai != null)
        {
            //Make player 1 human controlled.
            player1Human.SetTurn(true);
            player1Human.opponentPlayer = player2Ai;
            player1Human.gameObject.SetActive(true);

            //Make player 2 Ai controlled.
            player2Ai.SetTurn(false);
            player2Ai.opponentPlayer = player1Human;
            player2Ai.gameObject.SetActive(true);
        }

        //Activate game canvas.
        if(gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }

        //Deativate self.
        if (startCanvas != null)
        {
            startCanvas.gameObject.SetActive(false);
        }
    }
    
    //--------------------------------------------------------------------------------------------

    public void Start2PlayerGame()
    {
        if (player1Human != null && player2Human != null)
        {
            //Make player 1 human controlled.
            player1Human.SetTurn(true);
            player1Human.opponentPlayer = player2Human;
            player1Human.gameObject.SetActive(true);

            //Make player 2 Human controlled.
            player2Human.SetTurn(false);
            player2Human.opponentPlayer = player1Human;
            player2Human.gameObject.SetActive(true);
        }

        //Activate game canvas.
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }

        //Deativate self.
        if (startCanvas != null)
        {
            startCanvas.gameObject.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------------------
}
*/