using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class SpawnedObject : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;
    [SerializeField, Min(0)] private int damage = 2;
    [SerializeField, Min(0.1f)] private float lifetime = 5f;

    [HideInInspector]
    public UnityEvent OnDespawned;

    private Rigidbody2D body;
    private Vector2 direction = Vector2.right;
    private float lifeTimer;
    private bool hasHit;
    private bool isPooled;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        lifeTimer = lifetime;
        hasHit = false;
        direction = Vector2.right;
        if (body != null) body.linearVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        if (body != null) body.linearVelocity = direction * speed;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) Despawn();
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0f) direction = newDirection.normalized;
        if (body != null) body.linearVelocity = direction * speed;
    }

    public void SetPooled(bool pooled)
    {
        isPooled = pooled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitTarget(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitTarget(collision.gameObject);
    }

    private void HitTarget(GameObject target)
    {
        if (hasHit) return;
        hasHit = true;

        Minion minion = target.GetComponentInParent<Minion>();
        if (minion != null)
        {
            minion.TakeDamage(damage);
        }
        else
        {
            BossHealth boss = target.GetComponentInParent<BossHealth>();
            if (boss != null) boss.TakeProjectileHit();
        }

        Despawn();
    }

    private void Despawn()
    {
        if (isPooled)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        OnDespawned?.Invoke();
    }
}