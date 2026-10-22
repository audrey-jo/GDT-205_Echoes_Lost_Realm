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
        // Finds each node and adds it to the node list
        foreach (Transform child in waypointParent)
            graph.AllNodes.Add(new PathNode(child.position));

        // Finds all available nodes and links neighbors together
        graph.UpdateWalkability(wallMask, agentRadius);
        graph.LinkProximityNeighbors(linkDistance, wallMask, agentRadius);

        // Marks the target destination and sets the agent's starting position
        target = graph.AllNodes[targetIndex];
        agent.transform.position = graph.AllNodes[startIndex].Position;
        // Finds the first path to the destination
        Repath();
    }

    void Update()
    {
        // Recalculates path every 0.25 seconds
        timer += Time.deltaTime;
        if (timer < repathInterval) return;
        timer = 0f;

        // Figures out what nodes are being blocked by enemies
        graph.UpdateEnemyBlocking(enemyMask, enemyAvoidRadius);

        // If the agent is moving and the path ahead isn't blocked, it won't repath
        if (agent.IsMoving && !PathBlocked()) return;

        Repath();
    }

    bool PathBlocked()
    {
        // Gets the path and chooses the nodes that it will check
        var path = agent.CurrentPath;
        int end = Mathf.Min(path.Count, agent.PathIndex + lookAhead);
        // Returns if PathIndex is out of range
        if (agent.PathIndex >= path.Count) return false;

        // Finds the agent's position and the direction of the next node
        Vector2 pos = agent.transform.position;
        Vector2 delta = path[agent.PathIndex].Position - pos;
        // Checks if there's anything blocking and returns that the path is blocked if there is
        if (Physics2D.CircleCast(pos, agentRadius, delta.normalized, delta.magnitude, enemyMask).collider != null)
            return true;

        // Checks for enemies and edges
        for (int i = agent.PathIndex; i < end; i++)
        {
            if (path[i].IsBlockedByEnemy) return true;
            if (i > agent.PathIndex && graph.IsEdgeBlocked(path[i-1], path[i])) return true;
        }
        return false;
    }

    void Repath()
    {
        // Finds starting position of repath
        // If agent.LastNode is null, it'll find the nearest node
        // Useful for the very first time this runs when the agent first starts
        PathNode start = agent.LastNode ?? graph.GetNearestNode(agent.transform.position);

        if (start == null) return;

        // If the agent is already at its destination
        if (!agent.IsMoving && start == target) return;

        // Resets node costs (since it's calculating new costs now) and finds the path
        graph.ResetNodes();
        var path = pathfinder.FindPath(start, target);

        if (path == null)
        {
            if (!agent.IsMoving || PathBlocked()) agent.Stop();
            return;
        }
        
        // Sets the new starting position for the new path and has the agent follow the path
        path.Insert(0, start);
        agent.FollowPath(path, speed);
    }

    private void OnDrawGizmos()
    {
        // Debugging visuals
        if (graph == null) return;

        Gizmos.color = Color.yellow;
        foreach (var node in graph.AllNodes)
        {
            if (node.IsBlockedByEnemy)
                Gizmos.DrawSphere(node.Position, 0.35f);
        }
    }
}
