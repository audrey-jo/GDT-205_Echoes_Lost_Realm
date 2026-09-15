using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject CellPrefab = null;

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

    void DestroyEnvironment()
    {
        //Clear the grevious static grid.
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    //-------------------------------------------------------------------------------------------------

    void GenerateEnvironment()
    {
        //Set up the static grid.
        Grid.grid = new GameObject[Grid.Width, Grid.Height];

        //Make all cells accessible, an generate a visual cell on screen.
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                if(CellPrefab != null)
                {
                    Vector3 startPos = new Vector3();
                    startPos.x = transform.position.x + (x-(Grid.Width*0.5f)) * 40.0f;
                    startPos.y = transform.position.y + (y-(Grid.Height*0.5f)) * 40.0f;

                    GameObject GO = GameObject.Instantiate(CellPrefab, startPos, Quaternion.identity, transform);
                    Grid.grid[x, y] = GO;

                    Node node = GO.GetComponent<Node>();
                    if(node != null)
                        node.SetStartingValues();

                    Cell cell = GO.GetComponent<Cell>();
                    if (cell != null)
                        cell.SetPosition(x, y);
                }
            }
        }

        //Set the starting cell to be the bottom left cell.
        Cell startingCell = Grid.grid[0, 0].GetComponent<Cell>();
        if(startingCell != null)
        {
            startingCell.SetAsStart(true);
        }

        //Set the target cell to be the top right cell.
        Cell targetCell = Grid.grid[Grid.Width - 1, Grid.Height - 1].GetComponent<Cell>();
        if (targetCell != null)
        {
            targetCell.SetAsTarget(true);
        }
    }

    //-------------------------------------------------------------------------------------------------
}
