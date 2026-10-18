using UnityEngine;

public class PathDemo : MonoBehaviour
{
    public NodeGraph graph;
    public AStarPathfinder pathfinder;
    public Agent agent;
    public Transform waypointParent;
    public float linkDistance = 5f;
    public float speed = 3f;
    public int startIndex = 0;
    public int targetIndex = 5;

    public LayerMask wallMask;
    public float agentRadius = 0.5f;

    public LayerMask enemyMask;
    public float enemyAvoidRadius = 1.0f;
    public float repathInterval = 0.25f;
    public int lookAhead = 6;

    PathNode target;
    float timer;

    void Start()
    {
        foreach (Transform child in waypointParent)
            graph.AllNodes.Add(new PathNode(child.position));

        graph.UpdateWalkability(wallMask, agentRadius);
        graph.LinkProximityNeighbors(linkDistance, wallMask, agentRadius);

        target = graph.AllNodes[targetIndex];
        agent.transform.position = graph.AllNodes[startIndex].Position;
        Repath();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < repathInterval) return;
        timer = 0f;

        graph.UpdateEnemyBlocking(enemyMask, enemyAvoidRadius);

        if (agent.IsMoving && !PathBlocked()) return;

        Repath();
    }

    bool PathBlocked()
    {
        var path = agent.CurrentPath;
        int end = Mathf.Min(path.Count, agent.PathIndex + lookAhead);
        if (agent.PathIndex >= path.Count) return false;

        Vector2 pos = agent.transform.position;
        Vector2 delta = path[agent.PathIndex].Position - pos;
        if (Physics2D.CircleCast(pos, agentRadius, delta.normalized, delta.magnitude, enemyMask).collider != null)
            return true;

        for (int i = agent.PathIndex; i < end; i++)
        {
            if (path[i].IsBlockedByEnemy) return true;
            if (i > agent.PathIndex && graph.IsEdgeBlocked(path[i-1], path[i])) return true;
        }
        return false;
    }

    void Repath()
    {
        PathNode start = agent.LastNode ?? graph.GetNearestNode(agent.transform.position);

        if (start == null) return;

        if (!agent.IsMoving && start == target) return;

        graph.ResetNodes();
        var path = pathfinder.FindPath(start, target);

        if (path == null)
        {
            if (!agent.IsMoving || PathBlocked()) agent.Stop();
            return;
        }
        
        path.Insert(0, start);
        agent.FollowPath(path, speed);
    }

    private void OnDrawGizmos()
    {
        if (graph == null) return;

        Gizmos.color = Color.yellow;
        foreach (var node in graph.AllNodes)
        {
            if (node.IsBlockedByEnemy)
                Gizmos.DrawSphere(node.Position, 0.35f);
        }
    }
}
