using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSAlgorithm : BaseAlgorithm
{
    private Queue<GameObject>   SearchQueue         = new Queue<GameObject>();
    private List<GameObject>    AlreadySearchedList = new List<GameObject>();
    //-----------------------------------------------------------------------------------

    public override void Search()
    {
        Debug.Log("---BFS Search---");

        //Start by clearing out any old path.
        Path.Clear();
        SearchQueue.Clear();
        AlreadySearchedList.Clear();

        //Add the starting node to the Open List.
        SearchQueue.Enqueue(Grid.grid[Grid.StartX, Grid.StartY]);

        GameObject targetObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Node targetNode = targetObject.GetComponent<Node>();

        while (SearchQueue.Count > 0)
        {
            GameObject currentObject = SearchQueue.Dequeue();
            if (currentObject != null)
            {
                //If we have reached the target node, construct the path and exit.
                if (currentObject == targetObject)
                {
                    ConstructPath();
                    UpdateVisuals();
                    return;
                }

                //Get next node's details.
                Node currentNode = currentObject.GetComponent<Node>();
                Cell currentCell = currentObject.GetComponent<Cell>();
                if (currentNode != null && currentCell != null)
                {
                    //Get the nodes in the 4 possible directions.
                    List<GameObject> childObjects = GetChildObjects(currentCell);
                    for (int i = 0; i < childObjects.Count; i++)
                    {
                        Node childNode = childObjects[i].GetComponent<Node>();
                        if (childNode != null)
                        {
                            if (!IsInList(AlreadySearchedList, childObjects[i]) && !IsInList(SearchQueue, childObjects[i]))
                            {
                                childNode.ParentX = currentCell.xPos;
                                childNode.ParentY = currentCell.yPos;
                                SearchQueue.Enqueue(childObjects[i]);
                            }
                        }
                    }
                }

                AlreadySearchedList.Add(currentObject);
            }
        }

        UpdateVisuals();
        firstPass = true;
    }

    //-----------------------------------------------------------------------------------

    public override void SteppedSearch()
    {
        Debug.Log("---BFS Stepped Search---");

        if (firstPass)
        {
            //Start by clearing out any old path.
            Path.Clear();
            SearchQueue.Clear();
            AlreadySearchedList.Clear();

            //Add the starting node to the Open List.
            SearchQueue.Enqueue(Grid.grid[Grid.StartX, Grid.StartY]);

            //We don't need to reset these values next time around.
            firstPass = false;

            ClearVisuals();
        }

        Cell currentCell = null;
        GameObject targetObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Node targetNode = targetObject.GetComponent<Node>();

        if (SearchQueue.Count > 0)
        {
            GameObject currentObject = SearchQueue.Dequeue();
            if (currentObject != null)
            {
                //If we have reached the target node, construct the path and exit.
                if (currentObject == targetObject)
                {
                    ConstructPath();
                    UpdateVisuals();
                    return;
                }

                //Get next node's details.
                Node currentNode = currentObject.GetComponent<Node>();
                currentCell = currentObject.GetComponent<Cell>();
                if (currentNode != null && currentCell != null)
                {
                    //Get the nodes in the 4 possible directions.
                    List<GameObject> childObjects = GetChildObjects(currentCell);
                    for (int i = 0; i < childObjects.Count; i++)
                    {
                        Node childNode = childObjects[i].GetComponent<Node>();
                        if (childNode != null)
                        {
                            if (!IsInList(AlreadySearchedList, childObjects[i]) && !IsInList(SearchQueue, childObjects[i]))
                            {
                                childNode.ParentX = currentCell.xPos;
                                childNode.ParentY = currentCell.yPos;
                                SearchQueue.Enqueue(childObjects[i]);
                            }
                        }
                    }
                }

                AlreadySearchedList.Add(currentObject);
            }
        }
        else
        {
            firstPass = true;
        }

        UpdateVisuals();

        //Additional visuals for the stepped approach to show which cell we just looked at.
        if (currentCell != null)
        {
            currentCell.SetColour(Color.green);
        }
    }

    //-----------------------------------------------------------------------------

    public override void UpdateVisuals()
    {
        //Set cells in the open list to be blue.
        if (SearchQueue.Count > 0)
        {
            foreach (GameObject obj in SearchQueue)
            {
                Cell currentCell = obj.GetComponent<Cell>();
                if (currentCell != null)
                {
                    currentCell.SetColour(Color.blue);
                }
            }
        }

        //Set the cells in the already searched list to be red.
        for (int i = 0; i < AlreadySearchedList.Count; i++)
        {
            Cell currentCell = AlreadySearchedList[i].GetComponent<Cell>();
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

    //-----------------------------------------------------------------------------------
}
