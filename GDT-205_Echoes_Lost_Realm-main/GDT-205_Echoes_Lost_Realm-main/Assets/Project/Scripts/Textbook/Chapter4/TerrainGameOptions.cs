using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainGameOptions : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GenerateHeatmap heatmapGenerator;
    [SerializeField] private Text foodCountText;

    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private Transform alienParent;

    //--------------------------------------------------------------------------------------------

    private void Update()
    {
        if(foodCountText != null)
        {
            foodCountText.text = TerrainGameData.FoodCollected.ToString();
        }
    }

    //--------------------------------------------------------------------------------------------

    public void RegenGrid()
    {
        //Destroy the current environment, and generate a new one.
        if(heatmapGenerator != null)
        {
            heatmapGenerator.DestroyEnvironment();
            heatmapGenerator.GenerateEnvironment();
        }
    }

    //--------------------------------------------------------------------------------------------

    public void ReturnToMenu()
    {
        //Destroy the environment.
        if (heatmapGenerator != null)
        {
            heatmapGenerator.DestroyEnvironment();
        }

        //Enable the intro canvas.
        if(introCanvas != null)
        {
            introCanvas.gameObject.SetActive(true);
        }

        //Disable the game canvas.
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SpawnAnAlien()
    {
        if(alienPrefab != null && alienParent != null)
        {
            GameObject newGO = Instantiate(alienPrefab, alienParent);
            if (newGO != null)
            {
                Alien alien = newGO.GetComponent<Alien>();
                if (alien != null)
                {
                    alien.SetPosition(Heatmap.HomeRow, Heatmap.HomeCol);
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------
}
