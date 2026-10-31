using UnityEngine;
using UnityEngine.Events;

public class SpawnedObject : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;

    [HideInInspector]
    public UnityEvent OnDespawned;

    private void Update() { transform.position = transform.position + speed * Time.deltaTime * Vector3.right; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.gameObject.GetComponent<Minion>() != null)
            collision.gameObject.GetComponent<Minion>().TakeDamage(2);

        OnDisable();
    }

    private void OnCollisionEnter2D(Collision2D collision) { OnDisable(); }

    void OnDisable() { OnDespawned.Invoke(); }
}