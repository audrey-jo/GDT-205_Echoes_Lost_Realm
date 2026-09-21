using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAlgorithm : MonoBehaviour
{
    protected List<GameObject> Path = new List<GameObject>();
    protected bool firstPass = true;
    //-----------------------------------------------------------------------------

    public virtual void Search()
    {
        Debug.Log("Shouldn't be here");
    }

    //-----------------------------------------------------------------------------

    public virtual void SteppedSearch()
    {
    }

    //-----------------------------------------------------------------------------

    public virtual void ConstructPath()
    {

    }

    //-----------------------------------------------------------------------------

    public virtual void UpdateVisuals()
    {

    }

    //-----------------------------------------------------------------------------

    public void ClearVisuals()
    {
        Debug.Log("---Clear Visuals---");
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                Cell currentCell = Grid.grid[x,y].GetComponent<Cell>();
                if (currentCell != null)
                {
                    if (currentCell.isAccessible)
                    {
                        currentCell.SetColour(Color.white);
                    }
                }

                Node currentNode = Grid.grid[x, y].GetComponent<Node>();
                if (currentNode != null)
                {
                    currentNode.SetStartingValues();
                }
            }
        }
    }

    //-----------------------------------------------------------------------------

    protected int GetManhattanDistance(GameObject currentObject, GameObject targetObject)
    {
        Cell currentCell = currentObject.GetComponent<Cell>();
        Cell targetCell = targetObject.GetComponent<Cell>();

        if (currentCell != null && targetCell != null)
        {
            return Mathf.Abs(targetCell.xPos - currentCell.xPos) + Mathf.Abs(targetCell.yPos - currentCell.yPos);
        }
        else
        {
            return -1;
        }
    }

    //-----------------------------------------------------------------------------

    protected float GetEuclideanDistance(GameObject currentObject, GameObject targetObject)
    {
        Cell currentCell = currentObject.GetComponent<Cell>();
        Cell targetCell = targetObject.GetComponent<Cell>();

        if (currentCell != null && targetCell != null)
        {
            float xDiff = Mathf.Abs(targetCell.xPos - currentCell.xPos);
            float yDiff = Mathf.Abs(targetCell.yPos - currentCell.yPos);


            return Mathf.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
        }
        else
        {
            return -1.0f;
        }
    }

    //-----------------------------------------------------------------------------

    protected bool IsInList(List<GameObject> listToCheck, GameObject objectToCheck)
    {
        if (listToCheck.Contains(objectToCheck))
            return true;
        else
            return false;
    }

    //-----------------------------------------------------------------------------

    protected bool IsInList(Stack<GameObject> listToCheck, GameObject objectToCheck)
    {
        if (listToCheck.Contains(objectToCheck))
            return true;
        else
            return false;
    }

    //-----------------------------------------------------------------------------

    protected bool IsInList(Queue<GameObject> listToCheck, GameObject objectToCheck)
    {
        if (listToCheck.Contains(objectToCheck))
            return true;
        else
            return false;
    }

    //-----------------------------------------------------------------------------

    protected List<GameObject> GetChildObjects(Cell currentCell)
    {
        List<GameObject> children = new List<GameObject>();

        //Down direction.
        if (currentCell.yPos - 1 >= 0)
        {
            GameObject upGO = Grid.grid[currentCell.xPos, currentCell.yPos - 1];
            Cell upCell = upGO.GetComponent<Cell>();
            if (upCell != null)
            {
                //We only add this child if it is accessible.
                if (upCell.isAccessible)
                    children.Add(upGO);
            }
        }

        //Down&Left direction.
        if (currentCell.xPos - 1 >= 0 && currentCell.yPos - 1 >= 0)
        {
            GameObject upleftGO = Grid.grid[currentCell.xPos - 1, currentCell.yPos - 1];
            Cell upleftCell = upleftGO.GetComponent<Cell>();
            if (upleftCell != null)
            {
                //We only add this child if it is accessible.
                if (upleftCell.isAccessible)
                    children.Add(upleftGO);
            }
        }

        //Left direction.
        if (currentCell.xPos - 1 >= 0)
        {
            GameObject leftGO = Grid.grid[currentCell.xPos - 1, currentCell.yPos];
            Cell leftCell = leftGO.GetComponent<Cell>();
            if (leftCell != null)
            {
                //We only add this child if it is accessible.
                if (leftCell.isAccessible)
                    children.Add(leftGO);
            }
        }

        //Up&Left direction.
        if (currentCell.xPos - 1 >= 0 && currentCell.yPos + 1 < Grid.Height)
        {
            GameObject downleftGO = Grid.grid[currentCell.xPos - 1, currentCell.yPos + 1];
            Cell downleftCell = downleftGO.GetComponent<Cell>();
            if (downleftCell != null)
            {
                //We only add this child if it is accessible.
                if (downleftCell.isAccessible)
                    children.Add(downleftGO);
            }
        }

        //Up direction.
        if (currentCell.yPos + 1 < Grid.Height)
        {
            GameObject downGO = Grid.grid[currentCell.xPos, currentCell.yPos + 1];
            Cell downCell = downGO.GetComponent<Cell>();
            if (downCell != null)
            {
                //We only add this child if it is accessible.
                if (downCell.isAccessible)
                    children.Add(downGO);
            }
        }

        //Up&Right direction.
        if (currentCell.xPos + 1 < Grid.Width && currentCell.yPos + 1 < Grid.Height)
        {
            GameObject downrightGO = Grid.grid[currentCell.xPos + 1, currentCell.yPos + 1];
            Cell downrightCell = downrightGO.GetComponent<Cell>();
            if (downrightCell != null)
            {
                //We only add this child if it is accessible.
                if (downrightCell.isAccessible)
                    children.Add(downrightGO);
            }
        }

        //Right direction.
        if (currentCell.xPos + 1 < Grid.Width)
        {
            GameObject rightGO = Grid.grid[currentCell.xPos + 1, currentCell.yPos];
            Cell rightCell = rightGO.GetComponent<Cell>();
            if (rightCell != null)
            {
                //We only add this child if it is accessible.
                if (rightCell.isAccessible)
                    children.Add(rightGO);
            }
        }

        //Down&Right direction.
        if (currentCell.xPos + 1 < Grid.Width && currentCell.yPos - 1 >= 0)
        {
            GameObject uprightGO = Grid.grid[currentCell.xPos + 1, currentCell.yPos - 1];
            Cell uprightCell = uprightGO.GetComponent<Cell>();
            if (uprightCell != null)
            {
                //We only add this child if it is accessible.
                if (uprightCell.isAccessible)
                    children.Add(uprightGO);
            }
        }

        return children;
    }

    //-----------------------------------------------------------------------------
}
