using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------------------------------------------

public enum GridChoice
{
    SetStartPosition,
    SetTargetPosition,
    ClearObstacles,
    AddObstacles,
    None
};

//-------------------------------------------------------------------------------------------------

public class GridOptions : MonoBehaviour
{
    [SerializeField] private GameObject gridOptions;
    [SerializeField] private GameObject searchOptions;
    [SerializeField] private GameObject algorithmOptions;

    public static GridChoice gridChoice = GridChoice.None;

    [SerializeField] private Image SetStartPosition_ButtonImage;
    [SerializeField] private Image SetTargetPosition_ButtonImage;
    [SerializeField] private Image ClearObstacles_ButtonImage;
    [SerializeField] private Image AddObstacles_ButtonImage;

    //-------------------------------------------------------------------------------------------------

    public void SwitchToSearchOptions()
    {
        if (gridOptions != null)
        {
            gridOptions.SetActive(false);
        }
        if (searchOptions != null)
        {
            searchOptions.SetActive(true);
        }
        if (algorithmOptions != null)
        {
            algorithmOptions.SetActive(true);
        }

        //Clear toggle from buttons.
        gridChoice = GridChoice.None;
        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.black;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.black;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.black;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void GenerateRandomGrid()
    {
        int blockedPercentage = 30;

        //-------------------------------------------------------------------------------------------------
        //Block of 30% of the cells.
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                Cell cell = Grid.grid[x, y].GetComponent<Cell>();
                if (cell != null)
                {
                    cell.RefreshValues();

                    int randomNumber = Random.Range(0, 100);
                    if (randomNumber < blockedPercentage)
                    {
                        cell.SetAccessible(false);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------
        //Chose a random start position.
        Grid.StartX = Random.Range(0, Grid.Width);
        Grid.StartY = Random.Range(0, Grid.Height);
        Cell startCell = Grid.grid[Grid.StartX, Grid.StartY].GetComponent<Cell>();
        if (startCell != null)
        {
            startCell.SetAsStart(true);
        }

        //-------------------------------------------------------------------------------------------------
        //Chose a random target position, but ensure it isnt the same as the start position.
        int randomTargetX = 0;
        int randomTargetY = 0;
        do
        {
            randomTargetX = Random.Range(0, Grid.Width);
            randomTargetY = Random.Range(0, Grid.Height);

        } while (randomTargetX == Grid.StartX && randomTargetY == Grid.StartY);

        Grid.TargetX = randomTargetX;
        Grid.TargetY = randomTargetY;
        Cell targetCell = Grid.grid[Grid.TargetX, Grid.TargetY].GetComponent<Cell>();
        if (targetCell != null)
        {
            targetCell.SetAsTarget(true);
        }

        //Clear toggle from buttons.
        gridChoice = GridChoice.None;
        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.black;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.black;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.black;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_SetStartPosition()
    {
        gridChoice = GridChoice.SetStartPosition;

        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.blue;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.black;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.black;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_SetTargetPosition()
    {
        gridChoice = GridChoice.SetTargetPosition;

        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.black;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.blue;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.black;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_ClearObstacles()
    {
        gridChoice = GridChoice.ClearObstacles;

        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.black;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.black;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.blue;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_AddObstacles()
    {
        gridChoice = GridChoice.AddObstacles;

        if (SetStartPosition_ButtonImage != null)
        {
            SetStartPosition_ButtonImage.color = Color.black;
        }

        if (SetTargetPosition_ButtonImage != null)
        {
            SetTargetPosition_ButtonImage.color = Color.black;
        }

        if (ClearObstacles_ButtonImage != null)
        {
            ClearObstacles_ButtonImage.color = Color.black;
        }

        if (AddObstacles_ButtonImage != null)
        {
            AddObstacles_ButtonImage.color = Color.blue;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_None()
    {
        gridChoice = GridChoice.None;
    }

    //-------------------------------------------------------------------------------------------------
}
