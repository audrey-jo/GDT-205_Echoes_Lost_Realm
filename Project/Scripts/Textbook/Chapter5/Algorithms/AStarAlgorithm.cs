using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : BaseAlgorithm
{
    private List<GameObject> OpenList   = new List<GameObject>();
    private List<GameObject> ClosedList = new List<GameObject>();

    //-----------------------------------------------------------------------------

    public override void Search()
    {
        Debug.Log("---AStar Search---");
        
        //Todo: Add code here.



        UpdateVisuals();

        //Completed, so next search will be the first pass (used in stepped search).
        firstPass = true;
    }

    //-----------------------------------------------------------------------------

    public override void SteppedSearch()
    {
        Debug.Log("---AStar Stepped Search---");
        float g, h, f;

        if (firstPass)
        {
            //Start by clearing out any old path.
            Path.Clear();

            //Also clear out the 2 lists.
            OpenList.Clear();
            ClosedList.Clear();

            //Add the starting node to the Open List.
            OpenList.Add(Grid.grid[Grid.StartX, Grid.StartY]);

            //We don't need to reset these values next time around.
            firstPass = false;

            ClearVisuals();
        }

        Cell currentCell = null;
        GameObject targetObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Node targetNode = targetObject.GetComponent<Node>();

        //Do one pass of the algorithm.
        if (OpenList.Count > 0)
        {
            //Get the cheapest node from the open list.
            int indexOfCheapestNode = FindCheapestNodeInOpenList();
            GameObject currentObject = OpenList[indexOfCheapestNode];
            if (currentObject != null)
            {
                //If we have reached the target node, construct the path and exit.
                if (currentObject == targetObject)
                {
                    ConstructPath();
                    UpdateVisuals();
                    return;
                }

                //Get the node details.
                Node currentNode = currentObject.GetComponent<Node>();
                currentCell = currentObject.GetComponent<Cell>();
                if (currentNode != null && currentCell != null)
                {
                    //Get the nodes in the 4 possible directions.
                    List<GameObject> childObjects = GetChildObjects(currentCell);

                    for (int i = 0; i < childObjects.Count; i++)
                    {
                        //Calculate costs.
                        g = currentNode.g + GetEuclideanDistance(childObjects[i], currentObject);
                        h = GetManhattanDistance(childObjects[i], targetObject);
                        f = g + h;

                        Node childNode = childObjects[i].GetComponent<Node>();
                        if (childNode != null)
                        {
                            bool setValues = false;
                            if (IsInList(ClosedList, childObjects[i]) || IsInList(OpenList, childObjects[i]))
                            {
                                //Check if it is cheaper to get to this node from here, if so we need to overwrite with the new values.
                                if (f < childNode.f)
                                    setValues = true;
                            }
                            else
                            {
                                //Not in either list, so add to the OPEN list.
                                OpenList.Add(childObjects[i]);

                                //New node, so we need to store these values.
                                setValues = true;
                            }

                            //Set the node's values.
                            if (setValues)
                            {
                                childNode.g = g;
                                childNode.h = h;
                                childNode.f = f;
                                childNode.ParentX = currentCell.xPos;
                                childNode.ParentY = currentCell.yPos;
                            }
                        }
                    }
                }

                //Remove from the Open list now and add to the Closed list.
                OpenList.Remove(currentObject);
                ClosedList.Add(currentObject);
            }
        }
        else
        {
            firstPass = true;
        }

        UpdateVisuals();

        //Additional visuals for the stepped approach to show which cell we just looked at.
        if(currentCell != null)
        {
            currentCell.SetColour(Color.green);
        }
    }

    //-----------------------------------------------------------------------------

    public override void ConstructPath()
    {
        GameObject startObject = Grid.grid[Grid.StartX, Grid.StartY];
        GameObject currentObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Path.Add(currentObject);

        //Work backwards from the target node through its parent to the start node.
        do
        {
            if (currentObject != null)
            {
                Path.Add(currentObject);

                Node currentNode = currentObject.GetComponent<Node>();
                if (currentNode != null)
                {
                    currentObject = Grid.grid[currentNode.ParentX, currentNode.ParentY];
                }
            }
        } while (currentObject != startObject);

        //We need to complete the path by adding the starting position.
        Path.Add(startObject);

        //Reverse the order.
        Path.Reverse();

        //Allow the stepped approach to restart.
        firstPass = true;
    }

    //-----------------------------------------------------------------------------

    public override void UpdateVisuals()
    {
        //Set cells in the open list to be blue.
        for (int i = 0; i < OpenList.Count; i++)
        {
            Cell currentCell = OpenList[i].GetComponent<Cell>();
            if (currentCell != null)
            {
                currentCell.SetColour(Color.blue);
            }
        }

        //Set the cells in the Closed list to be red.
        for (int i = 0; i < ClosedList.Count; i++)
        {
            Cell currentCell = ClosedList[i].GetComponent<Cell>();
            if (currentCell != null)
            {
                currentCell.SetColour(Color.red);
            }
        }

        //Set the cells in the path to be green.
        for (int i = 0; i < Path.Count; i++)
        {
            Cell currentCell = Path[i].GetComponent<Cell>();
            if (currentCell != null)
            {
                currentCell.SetColour(Color.green);
            }
        }
    }

    //-----------------------------------------------------------------------------

    //Returns index of cheapest node.
    int FindCheapestNodeInOpenList()
    {
        float cheapestCost = 9999.0f;
        int cheapestIndex = -1;
        for(int i = 0; i < OpenList.Count; i++)
        {
            Node currentNode = OpenList[i].GetComponent<Node>();
            if(currentNode != null)
            {
                if(currentNode.f < cheapestCost)
                {
                    cheapestCost = currentNode.f;
                    cheapestIndex = i;
                }
            }
        }

        return cheapestIndex;
    }

    //-----------------------------------------------------------------------------
}
