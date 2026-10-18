using UnityEngine;
using System.Collections.Generic;

public class PathNode
{
    public Vector2 Position;
    public bool IsWalkable;
    public bool IsBlockedByEnemy;
    public List<PathNode> Neighbors = new();

    public float gCost;
    public float hCost;
    public float FCost => gCost + hCost;

    public PathNode parent;

    public PathNode(Vector2 position, bool isWalkable = true)
    {
        Position = position;
        IsWalkable = isWalkable;
    }
}
