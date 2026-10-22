using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    public NodeGraph graph;
    public List<PathNode> FindPath(PathNode startNode, PathNode targetNode)
    {
        // Actual A* pathfinding logic

        // Returns no path if there's no starting point, ending point, or if the target node isn't accessible
        if (startNode == null || targetNode == null || !targetNode.IsWalkable || targetNode.IsBlockedByEnemy)
            return null;

        // Sets starting cost to the distance between itself and the target node
        // gCost is zero because the agent is already there
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        // List and set of unchecked and checked nodes, respectively
        List<PathNode> openSet = new();
        HashSet<PathNode> closedSet = new();

        // Adds starting node to unchecked nodes
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            PathNode currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                // Finds path in order of cost by going through each node

                // First compares total cost; if they're the same, compares heuristic cost
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            // Removes node from unchecked list and adds it to checked list
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Creates path for the agent to follow once it reaches the target
            if (currentNode == targetNode) return RetracePath(startNode, targetNode);

            foreach (PathNode neighbor in currentNode.Neighbors)
            {
                // Loops through all neighbors

                // If the neighbor is inaccessible or already checked, it is skipped
                if (!neighbor.IsWalkable ||
                    neighbor.IsBlockedByEnemy ||
                    graph.IsEdgeBlocked(currentNode, neighbor) ||
                    closedSet.Contains(neighbor)) continue;

                // Finds the cost to go to the neighbor
                float movementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (movementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    // Sets the neighbor's cost to its new cost from the current node
                    neighbor.gCost = movementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    // Sets previous node
                    neighbor.parent = currentNode;

                    // Adds neighbor to unchecked list with new cost
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        return null;
    }
    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        // Reverses the path that FindPath returned so that the agent can follow it from the start
        List<PathNode> path = new();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private float GetDistance(PathNode nodeA, PathNode nodeB)
    {
        // Finds the distance between two nodes
        return Vector2.Distance(nodeA.Position, nodeB.Position);
    }
}
