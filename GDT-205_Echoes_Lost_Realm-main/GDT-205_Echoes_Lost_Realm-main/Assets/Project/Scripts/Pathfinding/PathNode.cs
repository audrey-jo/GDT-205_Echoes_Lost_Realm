using UnityEngine;
using System.Collections.Generic;

// Information regarding the node.

public class PathNode
{
    public Vector2 Position;
    public bool IsWalkable;
    public bool IsBlockedByEnemy;
    public List<PathNode> Neighbors = new();

    // Cost of exact distance
    public float gCost;
    // Cost of heuristic distance
    public float hCost;
    // Cost of both added together; real cost
    public float FCost => gCost + hCost;

    public PathNode parent;

    public PathNode(Vector2 position, bool isWalkable = true)
    {
        // Constructor; allows MonoBehavior children to make nodes
        Position = position;
        IsWalkable = isWalkable;
    }
}
