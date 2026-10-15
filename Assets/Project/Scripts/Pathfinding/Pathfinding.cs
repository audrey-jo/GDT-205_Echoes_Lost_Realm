using UnityEngine;
using System.Collections.Generic;
public class Pathfinding : MonoBehaviour
{
    public static List<Node> FindPath(Node start, Node goal, IEnumerable<Node> allNodes)
    {
        foreach (var n in allNodes)
        {
            n.gCost = float.MaxValue;
            n.cameFrom = null;
        }

        List<Node> open = new();
        HashSet<Node> closed = new();

        start.gCost = 0;
        start.hCost = Dist(start, goal);
        open.Add(start);

        while (open.Count > 0)
        {
            Node current = open[0];

            foreach (var n in open)
                if (n.FCost < current.FCost) current = n;

            if (current == goal)
                return Reconstruct(goal);

            open.Remove(current);
            closed.Add(current);

            foreach (var neighbor in current.neighbors)
            {
                if (neighbor == null || closed.Contains(neighbor)) continue;

                float tentative = current.gCost + Dist(current, neighbor);
                if (tentative < neighbor.gCost)
                {
                    neighbor.cameFrom = current;
                    neighbor.gCost = tentative;
                    neighbor.hCost = Dist(neighbor, goal);

                    if (!open.Contains(neighbor)) open.Add(neighbor);
                }
            }
        }

        return null;
    }

    static float Dist(Node a, Node b) { return Vector3.Distance(a.transform.position, b.transform.position); }

    static List<Node> Reconstruct(Node end)
    {
        List<Node> path = new();

        for (var n = end; n != null; n = n.cameFrom) path.Add(n);

        path.Reverse();

        return path;
    }
}