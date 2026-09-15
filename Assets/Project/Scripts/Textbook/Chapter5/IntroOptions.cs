using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroOptions : MonoBehaviour
{
    [SerializeField] private GameObject IntroCanvas;
    [SerializeField] private GameObject GridCanvas;

    private int width = 22;
    private int height = 13;

    //------------------------------------------------------------------------------------------------------

    public void Begin()
    {
        //Set the dimensions of the grid.
        Grid.Width  = width;
        Grid.Height = height;

        //Switch canvases.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(false);
        }

        if (GridCanvas != null)
        {
            GridCanvas.SetActive(true);
        }
    }

    //------------------------------------------------------------------------------------------------------

}
