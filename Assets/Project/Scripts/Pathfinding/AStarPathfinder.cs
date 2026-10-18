using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    public NodeGraph graph;
    public List<PathNode> FindPath(PathNode startNode, PathNode targetNode)
    {
        if (startNode == null || targetNode == null || !targetNode.IsWalkable || targetNode.IsBlockedByEnemy)
            return null;

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        List<PathNode> openSet = new();
        HashSet<PathNode> closedSet = new();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            PathNode currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) return RetracePath(startNode, targetNode);

            foreach (PathNode neighbor in currentNode.Neighbors)
            {
                if (!neighbor.IsWalkable ||
                    neighbor.IsBlockedByEnemy ||
                    graph.IsEdgeBlocked(currentNode, neighbor) ||
                    closedSet.Contains(neighbor)) continue;

                float movementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (movementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = movementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }

        return null;
    }
    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
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
        return Vector2.Distance(nodeA.Position, nodeB.Position);
    }
}
