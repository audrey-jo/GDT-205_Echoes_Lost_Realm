using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainIntroOptions : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Canvas gameCanvas;

    //--------------------------------------------------------------------------------------------------

    public void BeginGame()
    {
        //Enable game canvas.
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(true);
        }

        //Disable self.
        if (introCanvas != null)
        {
            introCanvas.gameObject.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------------------------

}
