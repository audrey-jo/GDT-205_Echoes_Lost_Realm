using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public List<Transform> targets = new();
    public float speed;
    public float arriveDistance = 0.1f;

    Node[] allNodes;
    List<Node> path;
    int pathIndex;
    int targetNum = 0;

    bool reachedTarget;

    Rigidbody2D rb;
    public LayerMask obstacleMask;

    void Start()
    {
        allNodes = FindObjectsByType<Node>();
        rb = GetComponent<Rigidbody2D>();
        RequestPath();
    }

    void FixedUpdate()
    {
        FollowPath();
    }

    public void RequestPath()
    {
        Node start = GetNearestNode(transform.position);
        Node goal = GetNearestNode(targets[targetNum].position);

        print("start: " + start);
        print("goal: " + goal);

        if (start == null || goal == null) return;

        path = Pathfinding.FindPath(start, goal, allNodes);
    }

    void FollowPath()
    {
        if (path == null ||  pathIndex >= path.Count) return;

        Vector2 dest = path[pathIndex].transform.position;
        Vector2 next = Vector2.MoveTowards(rb.position, dest, speed * Time.fixedDeltaTime);

        rb.MovePosition(next);

        if (Vector3.Distance(rb.position, dest) < arriveDistance)
        {
            pathIndex++;
            reachedTarget = false;
        }
        else
            reachedTarget = true;
    }

    Node GetNearestNode(Vector3 pos)
    {
        Node best = null;
        float bestDist = float.MaxValue;

        foreach (var n in allNodes)
        {
            //if (!n.isWalkable) continue;

            float d = Vector3.Distance(pos, n.transform.position);

            //if (Physics2D.Linecast(pos, n.transform.position, obstacleMask) &&
            //    !Physics2D.Linecast(pos, n.transform.position, obstacleMask).collider.isTrigger) continue;

            if (d < bestDist)
            {
                bestDist = d;
                best = n;
            }
        }

        return best;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && reachedTarget)
        {
            targetNum++;
            RequestPath();
        }
    }
}
