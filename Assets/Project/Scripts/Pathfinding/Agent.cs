using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Agent : MonoBehaviour
{
    Coroutine moveRoutine;
    public PathDemo demo;

    public List<PathNode> CurrentPath
    {
        get;
        private set;
    }

    public PathNode LastNode
    {
        get;
        private set;
    }

    public int PathIndex
    {
        get;
        private set;
    }

    public bool IsMoving => CurrentPath != null;
    
    public void FollowPath(List<PathNode> path, float speed)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        CurrentPath = path;
        moveRoutine = StartCoroutine(Follow(path, speed));
    }

    public void Stop()
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = null;
        CurrentPath = null;
    }

    IEnumerator Follow(List<PathNode> path, float speed)
    {
        for (PathIndex = 0; PathIndex < path.Count; PathIndex++)
        {
            Vector2 goal = path[PathIndex].Position;
            while (Vector2.Distance(transform.position, goal) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);
                yield return null;
            }
            LastNode = path[PathIndex];
        }
        CurrentPath = null;
    }

    private void OnDrawGizmos()
    {
        if (demo == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, demo.agentRadius);
    }
}
