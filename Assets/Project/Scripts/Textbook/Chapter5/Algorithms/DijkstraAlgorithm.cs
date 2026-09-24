/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraAlgorithm : BaseAlgorithm
{
    //Frontier - Stores those nodes not yet explored.
    private List<GameObject> Frontier = new List<GameObject>();
    //ShortestPathTree - Stores those nodes already explored.
    private List<GameObject> SPT = new List<GameObject>();

    //-----------------------------------------------------------------------------

    public override void Search()
    {
        Debug.Log("---Dijkstra Search---");
        float g;

        //Start by clearing out any old path.
        Path.Clear();

        //Also clear out the 2 lists.
        Frontier.Clear();
        SPT.Clear();

        //Add the starting node to the Frontier.
        Frontier.Add(Grid.grid[Grid.StartX, Grid.StartY]);

        GameObject targetObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Node targetNode = targetObject.GetComponent<Node>();

        //Start the algorithm.
        while (Frontier.Count > 0)
        {
            //Get the cheapest node from the Frontier.
            int indexOfCheapestNode = FindCheapestNodeInFrontier();
            GameObject currentObject = Frontier[indexOfCheapestNode];
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
                Cell currentCell = currentObject.GetComponent<Cell>();
                if (currentNode != null && currentCell != null)
                {
                    //Get the nodes in the 4 possible directions.
                    List<GameObject> childObjects = GetChildObjects(currentCell);

                    for (int i = 0; i < childObjects.Count; i++)
                    {
                        //Calculate costs.
                        g = currentNode.g + GetEuclideanDistance(childObjects[i], currentObject);

                        Node childNode = childObjects[i].GetComponent<Node>();
                        if (childNode != null)
                        {
                            bool setValues = false;
                            if (IsInList(SPT, childObjects[i]) || IsInList(Frontier, childObjects[i]))
                            {
                                //Check if it is cheaper to get to this node from here, if so we need to overwrite with the new values.
                                if (g < childNode.g)
                                    setValues = true;
                            }
                            else
                            {
                                //Not in either list, so add to the Frontier.
                                Frontier.Add(childObjects[i]);

                                //New node, so we need to store these values.
                                setValues = true;
                            }

                            //Set the node's values.
                            if (setValues)
                            {
                                childNode.g = g;
                                childNode.ParentX = currentCell.xPos;
                                childNode.ParentY = currentCell.yPos;
                            }
                        }
                    }
                }

                //Remove from the Frontier now and add to the SPT.
                Frontier.Remove(currentObject);
                SPT.Add(currentObject);
            }
        }

        UpdateVisuals();
        firstPass = true;
    }

    //-----------------------------------------------------------------------------

    public override void SteppedSearch()
    {
        Debug.Log("---Dijkstra Stepped Search---");
        float g;

        if (firstPass)
        {
            //Start by clearing out any old path.
            Path.Clear();

            //Also clear out the 2 lists.
            Frontier.Clear();
            SPT.Clear();

            //Add the starting node to the Frontier.
            Frontier.Add(Grid.grid[Grid.StartX, Grid.StartY]);

            //We don't need to reset these values next time around.
            firstPass = false;

            ClearVisuals();
        }

        Cell currentCell = null;
        GameObject targetObject = Grid.grid[Grid.TargetX, Grid.TargetY];
        Node targetNode = targetObject.GetComponent<Node>();

        //Do one pass of the algorithm.
        if (Frontier.Count > 0)
        {
            //Get the cheapest node from the Frontier.
            int indexOfCheapestNode = FindCheapestNodeInFrontier();
            GameObject currentObject = Frontier[indexOfCheapestNode];
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

                        Node childNode = childObjects[i].GetComponent<Node>();
                        if (childNode != null)
                        {
                            bool setValues = false;
                            if (IsInList(SPT, childObjects[i]) || IsInList(Frontier, childObjects[i]))
                            {
                                //Check if it is cheaper to get to this node from here, if so we need to overwrite with the new values.
                                if (g < childNode.g)
                                    setValues = true;
                            }
                            else
                            {
                                //Not in either list, so add to the Frontier.
                                Frontier.Add(childObjects[i]);

                                //New node, so we need to store these values.
                                setValues = true;
                            }

                            //Set the node's values.
                            if (setValues)
                            {
                                childNode.g = g;
                                childNode.ParentX = currentCell.xPos;
                                childNode.ParentY = currentCell.yPos;
                            }
                        }
                    }
                }

                //Remove from the Frontier now and add to the SPT.
                Frontier.Remove(currentObject);
                SPT.Add(currentObject);
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
        //Set cells in the Frontier to be blue.
        for (int i = 0; i < Frontier.Count; i++)
        {
            Cell currentCell = Frontier[i].GetComponent<Cell>();
            if (currentCell != null)
            {
                currentCell.SetColour(Color.blue);
            }
        }

        //Set the cells in the SPT to be red.
        for (int i = 0; i < SPT.Count; i++)
        {
            Cell currentCell = SPT[i].GetComponent<Cell>();
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
    int FindCheapestNodeInFrontier()
    {
        float cheapestCost = 9999.0f;
        int cheapestIndex = -1;
        for (int i = 0; i < Frontier.Count; i++)
        {
            Node currentNode = Frontier[i].GetComponent<Node>();
            if (currentNode != null)
            {
                if (currentNode.g < cheapestCost)
                {
                    cheapestCost = currentNode.g;
                    cheapestIndex = i;
                }
            }
        }

        return cheapestIndex;
    }
    
    //-----------------------------------------------------------------------------
}
*/