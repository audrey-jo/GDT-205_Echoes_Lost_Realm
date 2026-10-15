using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;

    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    [HideInInspector] public Node cameFrom;
    private float linkRadius = 15f;
    [HideInInspector] public float FCost => gCost + hCost;

    [HideInInspector] public bool isWalkable;
    public LayerMask obstacleMask;

    public UnityEvent NodeStateChanged; 

    void Awake()
    {
        neighbors = new List<Node>();
        neighbors.Clear();
    }

    void Start()
    {
        Node[] all = FindObjectsByType<Node>();

        foreach (var other in all)
        {
            if (other == this) continue;
            if (Vector3.Distance(transform.position, other.transform.position) > linkRadius) continue;
            if (Physics2D.Linecast(transform.position, other.transform.position, obstacleMask).collider != null) continue;

            neighbors.Add(other);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        Gizmos.color = Color.cyan;
        foreach (var n in neighbors)
            if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Door"))
        {
            isWalkable = false;
            NodeStateChanged?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Door"))
        {
            isWalkable = true;
            NodeStateChanged?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Door"))
        {
            isWalkable = false;
        }
    }
}