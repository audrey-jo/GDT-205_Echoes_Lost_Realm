using UnityEngine;
using System.Collections.Generic;

public class NodeGraph : MonoBehaviour
{
    // List of nodes and blocked edges
    public List<PathNode> AllNodes = new();
    public HashSet<(PathNode, PathNode)> blockedEdges = new();

    // Checks for blocked edges
    public bool IsEdgeBlocked(PathNode a, PathNode b) => blockedEdges.Contains((a, b));

    public void UpdateWalkability(LayerMask wallMask, float checkRadius)
    {
        // Sets each node to walkable or not depending on if it's being blocked
        foreach (var node in AllNodes)
            node.IsWalkable = Physics2D.OverlapCircle(node.Position, checkRadius, wallMask) == null;
    }

    public void LinkProximityNeighbors(float maxDistance, LayerMask wallMask, float agentRadius = 0f)
    {
        // Finds each node's neighbors and links them together
        for (int i = 0; i < AllNodes.Count; ++i)
        {
            for (int j = i + 1; j < AllNodes.Count; j++)
            {
                // Loops through every node
                PathNode a = AllNodes[i];
                PathNode b = AllNodes[j];

                // Skips the node if it's not walkable
                if (!a.IsWalkable || !b.IsWalkable) continue;

                // Finds distance between two nodes
                float dist = Vector2.Distance(a.Position, b.Position);

                // Skips if the nodes are too far away from each other or one is blocked
                if (dist > maxDistance || IsBlocked(a.Position, b.Position, dist, wallMask, agentRadius))
                    continue;

                // Adds the node to the list of neighbors if it's not already there
                // Goes both ways
                if (!a.Neighbors.Contains(b)) a.Neighbors.Add(b);
                if (!b.Neighbors.Contains(a)) b.Neighbors.Add(a);
            }
        }
    }

    public PathNode GetNearestNode(Vector2 position)
    {
        // Finds closest node to passed in position
        PathNode best = null;
        float bestDist = float.MaxValue;

        foreach (var node in AllNodes)
        {
            // Loops through every node

            // If the node isn't walkable, skips
            if (!node.IsWalkable) continue;
            // Finds distance between the node and position
            float d = (node.Position - position).sqrMagnitude;
            if (d < bestDist)
            {
                // Sets best distance to whichever one is less
                bestDist = d;
                // Sets best node to current node
                best = node;
            }
        }

        return best;
    }

    public void UpdateEnemyBlocking(LayerMask enemyMask, float avoidRadius)
    {
        // Checks each node if it's blocked by an enemy by checking if an enemy is overlapping with it
        foreach (var node in AllNodes)
            node.IsBlockedByEnemy = Physics2D.OverlapCircle(node.Position, avoidRadius, enemyMask) != null;

        // Clears blocked edges
        blockedEdges.Clear();

        foreach (var a in AllNodes)
        {
            // Loops through every node

            // Skips if the node is already blocked by an enemy
            if (a.IsBlockedByEnemy) continue;

            foreach (var b in a.Neighbors)
            {
                // Loops through the node's neighbors

                // Skips if the neighbor is blocked by an enemy or an edge
                if (b.IsBlockedByEnemy) continue;
                if (blockedEdges.Contains((a,b))) continue;

                // Finds direction of neighbor
                Vector2 delta = b.Position - a.Position;
                // Casts CircleCast towards neighbor
                var hit = Physics2D.CircleCast(a.Position, avoidRadius, delta.normalized, delta.magnitude, enemyMask);
                if (hit.collider != null)
                {
                    // Adds nodes to blocked edges if the cast hits something
                    blockedEdges.Add((a, b));
                    blockedEdges.Add((b, a));
                }
            }
        }
    }

    private bool IsBlocked(Vector2 from, Vector2 to, float dist, LayerMask wallMask, float agentRadius)
    {
        // Checks if a path is blocked using physics casts
        if (agentRadius > 0)
            return Physics2D.CircleCast(from, agentRadius, (to - from).normalized, dist, wallMask).collider != null;
        return Physics2D.Linecast(from, to, wallMask).collider != null;
    }

    public void ResetNodes()
    {
        // Resets each node's cost
        foreach (var n in AllNodes)
        {
            n.gCost = float.MaxValue;
            n.hCost = 0;
            n.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        // Debugging visuals
        if (AllNodes == null) return;

        foreach (var node in AllNodes)
        {
            Gizmos.color = node.IsWalkable ? Color.green : Color.red;
            Gizmos.DrawSphere(node.Position, 0.3f);

            Gizmos.color = Color.blue;
            foreach (var neighbor in node.Neighbors)
            {
                Gizmos.DrawLine(node.Position, neighbor.Position);
            }
        }
    }


}
