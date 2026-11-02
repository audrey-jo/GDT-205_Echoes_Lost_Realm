/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private Canvas     startCanvas     = null;
    [SerializeField] private GameObject gameEntity      = null;

    [SerializeField] private Text       winLossMessage  = null;

    //-------------------------------------------------------------------------------------

    public void StartGame()
    {
        gameEntity.SetActive(true);
        startCanvas.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------

    public void RestartGame()
    {
        SceneManager.LoadScene("Chapter6_DecisionMaking");
    }

    //-------------------------------------------------------------------------------------

    public void EndGame(bool winLoss)
    {
        RestartGame();
    }

    //-------------------------------------------------------------------------------------
}
*/