using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchOptions : MonoBehaviour
{
    [SerializeField] private GameObject IntroCanvas;
    [SerializeField] private GameObject GridCanvas;

    [SerializeField] private GameObject gridOptions;
    [SerializeField] private GameObject searchOptions;
    [SerializeField] private GameObject algorithmOptions;

    //---------------------------------------------------------------------------

    public void BackToMainMenu()
    {
        //Clear the grid.
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                Cell cell = Grid.grid[x, y].GetComponent<Cell>();
                if (cell != null)
                {
                    cell.RefreshValues();
                }
            }
        }
        //Set the starting cell to be the bottom left cell.
        Cell startingCell = Grid.grid[0, 0].GetComponent<Cell>();
        if (startingCell != null)
        {
            startingCell.SetAsStart(true);
        }

        //Set the target cell to be the top right cell.
        Cell targetCell = Grid.grid[Grid.Width - 1, Grid.Height - 1].GetComponent<Cell>();
        if (targetCell != null)
        {
            targetCell.SetAsTarget(true);
        }

        //Put the scene back to its stating configuration.
        if (gridOptions != null)
        {
            gridOptions.SetActive(true);
        }

        if (searchOptions != null)
        {
            searchOptions.SetActive(false);
        }

        if (algorithmOptions != null)
        {
            algorithmOptions.SetActive(false);
        }

        GridOptions.gridChoice = GridChoice.None;

        //Disable this canvas and activate the menu one.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(true);
        }

        if (GridCanvas != null)
        {
            GridCanvas.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------
}
