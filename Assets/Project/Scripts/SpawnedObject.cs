using System;
using UnityEngine;
using UnityEngine.Events;

public class SpawnedObject : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;

    [HideInInspector]
    public UnityEvent OnDespawned;

    public static event Action<int> HitEnemy;

    private void Update() { transform.position = transform.position + speed * Time.deltaTime * Vector3.right; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            HitEnemy?.Invoke(2);

        OnDisable();
    }

    void OnDisable() { OnDespawned.Invoke(); }
}