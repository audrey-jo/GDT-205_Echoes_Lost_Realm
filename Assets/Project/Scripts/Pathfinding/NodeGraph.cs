using UnityEngine;
using System.Collections.Generic;

public class NodeGraph : MonoBehaviour
{
    public List<PathNode> AllNodes = new();
    public HashSet<(PathNode, PathNode)> blockedEdges = new();

    public bool IsEdgeBlocked(PathNode a, PathNode b) => blockedEdges.Contains((a, b));

    public void UpdateWalkability(LayerMask wallMask, float checkRadius)
    {
        foreach (var node in AllNodes)
            node.IsWalkable = Physics2D.OverlapCircle(node.Position, checkRadius, wallMask) == null;
    }

    public void LinkProximityNeighbors(float maxDistance, LayerMask wallMask, float agentRadius = 0f)
    {
        for (int i = 0; i < AllNodes.Count; ++i)
        {
            for (int j = i + 1; j < AllNodes.Count; j++)
            {
                PathNode a = AllNodes[i];
                PathNode b = AllNodes[j];

                if (!a.IsWalkable || !b.IsWalkable) continue;

                float dist = Vector2.Distance(a.Position, b.Position);

                if (dist > maxDistance || IsBlocked(a.Position, b.Position, dist, wallMask, agentRadius))
                    continue;

                if (!a.Neighbors.Contains(b)) a.Neighbors.Add(b);
                if (!b.Neighbors.Contains(a)) b.Neighbors.Add(a);
            }
        }
    }

    public PathNode GetNearestNode(Vector2 position)
    {
        PathNode best = null;
        float bestDist = float.MaxValue;

        foreach (var node in AllNodes)
        {
            if (!node.IsWalkable) continue;
            float d = (node.Position - position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = node;
            }
        }

        return best;
    }

    public void UpdateEnemyBlocking(LayerMask enemyMask, float avoidRadius)
    {
        foreach (var node in AllNodes)
            node.IsBlockedByEnemy = Physics2D.OverlapCircle(node.Position, avoidRadius, enemyMask) != null;

        blockedEdges.Clear();

        foreach (var a in AllNodes)
        {
            if (a.IsBlockedByEnemy) continue;

            foreach (var b in a.Neighbors)
            {
                if (b.IsBlockedByEnemy) continue;
                if (blockedEdges.Contains((a,b))) continue;

                Vector2 delta = b.Position - a.Position;
                var hit = Physics2D.CircleCast(a.Position, avoidRadius, delta.normalized, delta.magnitude, enemyMask);
                if (hit.collider != null)
                {
                    blockedEdges.Add((a, b));
                    blockedEdges.Add((b, a));
                }
            }
        }
    }

    private bool IsBlocked(Vector2 from, Vector2 to, float dist, LayerMask wallMask, float agentRadius)
    {
        if (agentRadius > 0)
            return Physics2D.CircleCast(from, agentRadius, (to - from).normalized, dist, wallMask).collider != null;
        return Physics2D.Linecast(from, to, wallMask).collider != null;
    }

    public void ResetNodes()
    {
        foreach (var n in AllNodes)
        {
            n.gCost = float.MaxValue;
            n.hCost = 0;
            n.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
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
