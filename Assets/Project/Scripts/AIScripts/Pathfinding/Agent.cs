using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Agent : MonoBehaviour
{
    Coroutine moveRoutine;
    public PathDemo demo;

    // Path that agent will move along
    public List<PathNode> CurrentPath
    {
        get;
        private set;
    }

    // Last node that the agent was at
    public PathNode LastNode
    {
        get;
        private set;
    }

    // Node indexes according to order of list
    // Makes it easy to figure out what nodes certain things are at
    public int PathIndex
    {
        get;
        private set;
    }

    // Sets IsMoving based on whether or not CurrenPath is null
    // If it's null, IsMoving will be false
    public bool IsMoving => CurrentPath != null;
    
    public void FollowPath(List<PathNode> path, float speed)
    {
        // Sets the path and starts the Follow coroutine
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        CurrentPath = path;
        moveRoutine = StartCoroutine(Follow(path, speed));
    }

    public void Stop()
    {
        // Makes the agent stop
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = null;
        CurrentPath = null;
    }

    IEnumerator Follow(List<PathNode> path, float speed)
    {
        // Follows path

        // Goes through each node in the path and moves the agent along
        for (PathIndex = 0; PathIndex < path.Count; PathIndex++)
        {
            Vector2 goal = path[PathIndex].Position;
            while (Vector2.Distance(transform.position, goal) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);
                // Returns and waits until next frame to restart
                yield return null;
            }
            // Sets the last node that the agent was at
            LastNode = path[PathIndex];
        }
        CurrentPath = null;
    }

    private void OnDrawGizmos()
    {
        // Debugging visuals
        if (demo == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, demo.agentRadius);
    }
}
