using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHeatmap : MonoBehaviour
{
    public GameObject CellPrefab = null;
    public int gridRows = 10;
    public int gridColumns = 10;

    public int percentageOfObstacles = 0;

    //-------------------------------------------------------------------------------------------------

    void OnEnable()
    {
        GenerateEnvironment();
    }

    //-------------------------------------------------------------------------------------------------

    void OnDisable()
    {
        DestroyEnvironment();
    }

    //-------------------------------------------------------------------------------------------------

    public void DestroyEnvironment()
    {
        //Clear the grevious static heatmap.
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void GenerateEnvironment()
    {
        TerrainGameData.FoodCollected = 0;

        Heatmap.Columns = gridColumns;
        Heatmap.Rows = gridRows;

        //Set up the static heatap.
        Heatmap.grid = new GameObject[Heatmap.Rows, Heatmap.Columns];

        //Randomly select a position for the FOOD, but ensure its not the same cell as the HOME.
        do
        {
            Heatmap.FoodRow1 = Random.Range(0, Heatmap.Rows);
            Heatmap.FoodCol1 = Random.Range(0, Heatmap.Columns);
        } while (Heatmap.FoodRow1 == Heatmap.HomeRow && Heatmap.FoodCol1 == Heatmap.HomeCol);
        do
        {
            Heatmap.FoodRow2 = Random.Range(0, Heatmap.Rows);
            Heatmap.FoodCol2 = Random.Range(0, Heatmap.Columns);
        } while (Heatmap.FoodRow2 == Heatmap.HomeRow && Heatmap.FoodCol2 == Heatmap.HomeCol);
        do
        {
            Heatmap.FoodRow3 = Random.Range(0, Heatmap.Rows);
            Heatmap.FoodCol3 = Random.Range(0, Heatmap.Columns);
        } while (Heatmap.FoodRow3 == Heatmap.HomeRow && Heatmap.FoodCol3 == Heatmap.HomeCol);


        //Make all cells accessible, and generate a visual cell on screen.
        for (int col = 0; col < Heatmap.Columns; col++)
        {
            for (int row = 0; row < Heatmap.Rows; row++)
            {
                if(CellPrefab != null)
                {
                    Vector3 startPos = new Vector3();
                    startPos.x = transform.position.x + (col - (Heatmap.Columns * 0.5f)) * 40.0f;
                    startPos.y = transform.position.y - (row - (Heatmap.Rows * 0.5f)) * 40.0f;

                    GameObject GO = GameObject.Instantiate(CellPrefab, startPos, Quaternion.identity, transform);
                    Heatmap.grid[row, col] = GO;

                    HeatmapCell cell = GO.GetComponent<HeatmapCell>();
                    if (cell != null)
                    {
                        cell.SetPosition(row, col);

                        //Lets randomly generate some obstacles, whilst setting the HOME and FOOD cells.
                        SetCellStatus(cell);
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------

    void SetCellStatus(HeatmapCell cell)
    {
        if(cell != null)
        {
            //Set the HOME cell. 
            if (cell.row == Heatmap.HomeRow && cell.col == Heatmap.HomeCol)
            {
                cell.SetStatus(HeatmapCellStatus.Home);
            }

            //Set the Food cell. 
            else if ((cell.row == Heatmap.FoodRow1 && cell.col == Heatmap.FoodCol1) ||
                     (cell.row == Heatmap.FoodRow2 && cell.col == Heatmap.FoodCol2) ||
                     (cell.row == Heatmap.FoodRow3 && cell.col == Heatmap.FoodCol3))
            {
                cell.SetStatus(HeatmapCellStatus.Food);
            }

            //There is a chance that this is an obstacle.
            else
            {
                int randomChanceOfBeingInaccessible = Random.Range(0, 100);
                if(randomChanceOfBeingInaccessible < percentageOfObstacles)
                {
                    cell.SetStatus(HeatmapCellStatus.InAccessible);
                }
            }
        }
    }
    //-------------------------------------------------------------------------------------------------
}
