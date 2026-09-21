using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private GameObject foodImage = null;
 
    public float    fDelayBetweenMoves          = 1.0f;
    public int      numberOfMovesUntilDeath     = 150;

    private bool    bHaveFood                   = false;
    private int     currentMoveCount            = 0;
    private float   fCurrentDelayBetweenMoves   = 0.0f;
    public float    fCoolDownAmount             = -0.01f;

    private eAlienMovementDirection previousDirection = eAlienMovementDirection.Left;

    //Positions on the grid.
    public int row;
    public int col;

    //--------------------------------------------------------------------------------------------

    void Start()
    {
        if (foodImage != null)
        {
            foodImage.SetActive(false);
        }

        currentMoveCount = 0;
        fCurrentDelayBetweenMoves = fDelayBetweenMoves;
        bHaveFood = false;
    }

    //--------------------------------------------------------------------------------------------

    void Update()
    {
        fCurrentDelayBetweenMoves -= Time.deltaTime;

        //Time to move.
        if(fCurrentDelayBetweenMoves <= 0.0)
        {
            //reset the delay.
            fCurrentDelayBetweenMoves = fDelayBetweenMoves;

            //Add to move count.
            currentMoveCount++;

            MakeMove();
        }
    }

    //-----------------------------------------------------------------------------------

    public void SetPosition(int iRow, int iColumn)
    {
        col = iColumn;
        row = iRow;

        float actualXPos = transform.parent.position.x + (col - (Heatmap.Columns * 0.5f)) * 40.0f;
        float actualYPos = transform.parent.position.y - (row - (Heatmap.Rows * 0.5f)) * 40.0f;

        transform.position = new Vector2(actualXPos, actualYPos);
    }

    //--------------------------------------------------------------------------------------------

    void GetNeighbouringHeatValues(ref float[] values)
    {
        int desiredCol = 0;
        int desiredRow = 0;

        //First get heat value to the LEFT.
        SetDesiredPositions(eAlienMovementDirection.Right, ref desiredRow, ref desiredCol);
        values[0] = GetCellValue(desiredRow, desiredCol);

        //Get heat value to the RIGHT.
        SetDesiredPositions(eAlienMovementDirection.Down, ref desiredRow, ref desiredCol);
        values[1] = GetCellValue(desiredRow, desiredCol);

        //Get heat value UP.
        SetDesiredPositions(eAlienMovementDirection.Left, ref desiredRow, ref desiredCol);
        values[2] = GetCellValue(desiredRow, desiredCol);

        //Get heat value DOWN.
        SetDesiredPositions(eAlienMovementDirection.Up, ref desiredRow, ref desiredCol);
        values[3] = GetCellValue(desiredRow, desiredCol);
    }

    //--------------------------------------------------------------------------------------------

    private void MakeMove()
    {
        bool bFoundAValidMove = false;
        int desiredCol = 0;
        int desiredRow = 0;
        float[] values = { 0.0f, 0.0f, 0.0f, 0.0f };

        GetNeighbouringHeatValues(ref values);

        if (bHaveFood == false)
        {
            //Search for Food.

        }
        else
        {
            //Follow heat of the grid to get home.

        }

        //Make the move.
        SetPosition(desiredRow, desiredCol);

        //An alien can only move so many times before dying.
        currentMoveCount++;
        if(currentMoveCount > numberOfMovesUntilDeath)
        {
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------------------------

    private void SetDesiredPositions(eAlienMovementDirection eDir, ref int desiredRow, ref int desiredCol)
    {
        switch (eDir)
        {
            case eAlienMovementDirection.Left:
                desiredCol = col - 1;
                desiredRow = row;
                break;

            case eAlienMovementDirection.Right:
                desiredCol = col + 1;
                desiredRow = row;
                break;

            case eAlienMovementDirection.Up:
                desiredCol = col;
                desiredRow = row - 1;
                break;

            case eAlienMovementDirection.Down:
                desiredCol = col;
                desiredRow = row + 1;
                break;
        }
    }

    //--------------------------------------------------------------------------------------------

    private void AdjustHeat(float fValue)
    {
        GameObject currentCellGO = Heatmap.grid[row, col];
        if (currentCellGO != null)
        {
            HeatmapCell cell = currentCellGO.GetComponent<HeatmapCell>();
            if (cell != null)
            {
                cell.SetValue(cell.GetValue()+fValue);
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    private float GetCellValue(int iRow, int iColumn)
    {
        if(IsAValidMove(iRow, iColumn))
        {
            GameObject currentCellGO = Heatmap.grid[iRow, iColumn];
            if (currentCellGO != null)
            {
                HeatmapCell cell = currentCellGO.GetComponent<HeatmapCell>();
                if (cell != null)
                {
                    return cell.GetValue();
                }
            }
        }

        return float.NaN;
    }

    //--------------------------------------------------------------------------------------------

    private void CheckForFood()
    {
        if (bHaveFood == false)
        {
            GameObject currentCellGO = Heatmap.grid[row, col];
            if (currentCellGO != null)
            {
                HeatmapCell cell = currentCellGO.GetComponent<HeatmapCell>();
                if (cell != null)
                {
                    if (cell.eCellStatus == HeatmapCellStatus.Food)
                    {
                        bHaveFood = true;

                        if (foodImage != null)
                        {
                            foodImage.SetActive(true);

                            //Allow another set of moves when food has been collected.
                            currentMoveCount = 0;
                        }
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    private void CheckForHome()
    {
        if (bHaveFood == true)
        {
            GameObject currentCellGO = Heatmap.grid[row, col];
            if (currentCellGO != null)
            {
                HeatmapCell cell = currentCellGO.GetComponent<HeatmapCell>();
                if (cell != null)
                {
                    if (cell.eCellStatus == HeatmapCellStatus.Home)
                    {
                        //Add to the food count.
                        TerrainGameData.FoodCollected++;

                        //Just kill this entity for now.
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    private bool IsAValidMove(int iRow, int iColumn)
    {
        //First check that we are on the grid.
        if(iColumn < 0 || iColumn >= Heatmap.Columns) { return false; }
        if(iRow < 0 || iRow >= Heatmap.Rows) { return false; }

        //We are on the grid, so check if the cell is accessible.
        GameObject currentCellGO = Heatmap.grid[iRow, iColumn];
        if (currentCellGO != null)
        {
            HeatmapCell cell = currentCellGO.GetComponent<HeatmapCell>();
            if (cell != null)
            {
                if (cell.eCellStatus == HeatmapCellStatus.InAccessible)
                {
                    return false;
                }
            }
        }

        //If we get here we can move to this position.
        return true;
    }

    //--------------------------------------------------------------------------------------------
}
