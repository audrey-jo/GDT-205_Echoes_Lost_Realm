/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Text CellText;
    [SerializeField] private Image cellImage;

    public bool isAccessible    = true;
    public bool hasBeenChecked  = false;
    public bool isStartCell     = false;
    public bool isTargetCell    = false;

    public int xPos;
    public int yPos;

    //--------------------------------------------------------------------------------------------

    void Start()
    {
        if (!isStartCell && !isTargetCell)
            RefreshValues();
    }

    //-----------------------------------------------------------------------------------

    public void SetPosition(int x, int y)
    {
        xPos = x;
        yPos = y;
    }

    //--------------------------------------------------------------------------------------------

    public void RefreshValues()
    {
        isAccessible    = true;
        hasBeenChecked  = false;
        isStartCell     = false;
        isTargetCell    = false;

        if (cellImage != null)
            cellImage.color = Color.white;

        if (CellText != null)
            CellText.text = "";
    }

    //--------------------------------------------------------------------------------------------

    public void SetAccessible(bool trueFalse)
    {
        isAccessible = trueFalse;

        if (cellImage != null)
        {
            if (isAccessible)
                cellImage.color = Color.white;
            else
            {
                isStartCell = false;
                isTargetCell = false;
                cellImage.color = Color.black;
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SetAsStart(bool trueFalse)
    {
        isStartCell = trueFalse;

        if (CellText != null)
        {
            if (isStartCell)
            {
                isAccessible = true;
                CellText.text = "S";

                if (isAccessible)
                    cellImage.color = Color.white;

                Grid.StartX = xPos;
                Grid.StartY = yPos;
            }
            else
                CellText.text = "";
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SetAsTarget(bool trueFalse)
    {
        isTargetCell = trueFalse;

        if (CellText != null)
        {
            if (isTargetCell)
            {
                isAccessible = true;
                CellText.text = "T";
                if (isAccessible)
                    cellImage.color = Color.white;

                Grid.TargetX = xPos;
                Grid.TargetY = yPos;
            }
            else
                CellText.text = "";
        }
    }

    //--------------------------------------------------------------------------------------------

    public void OnMouseHoverOver()
    {
        if (Input.GetMouseButton(0))
        {
            switch (GridOptions.gridChoice)
            {
                case GridChoice.SetStartPosition:
                    //Clear previous start cell.
                    Cell startCell = Grid.grid[Grid.StartX, Grid.StartY].GetComponent<Cell>();
                    if (startCell != null)
                    {
                        startCell.SetAsStart(false);
                    }
                    //Set this as the start cell.
                    SetAsStart(true);
                break;

                case GridChoice.SetTargetPosition:
                    //Clear previous start cell.
                    Cell targetCell = Grid.grid[Grid.TargetX, Grid.TargetY].GetComponent<Cell>();
                    if (targetCell != null)
                    {
                        targetCell.SetAsTarget(false);
                    }
                    //Set this as the target cell.
                    SetAsTarget(true);
                break;

                case GridChoice.ClearObstacles:
                    SetAccessible(true);
                break;

                case GridChoice.AddObstacles:
                    if (!isStartCell && !isTargetCell)
                        SetAccessible(false);
                break;

                case GridChoice.None:
                break;
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SetColour(Color col)
    {
        cellImage.color = col;
    }

    //--------------------------------------------------------------------------------------------
}
*/