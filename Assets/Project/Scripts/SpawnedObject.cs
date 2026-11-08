using UnityEngine;
using UnityEngine.Events;

public class SpawnedObject : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;

    [HideInInspector]
    public UnityEvent OnDespawned;

    private bool instantKill;
    private bool isEnemyProjectile;
    private Vector2 travelDirection = Vector2.right;
    private SpriteRenderer projectileRenderer;
    private Sprite regularSprite;
    private Color regularColor;

    private void Awake()
    {
        projectileRenderer = GetComponent<SpriteRenderer>();
        if (projectileRenderer != null)
        {
            regularSprite = projectileRenderer.sprite;
            regularColor = projectileRenderer.color;
        }
    }

    private void Update()
    {
        transform.position += (Vector3)(travelDirection * speed * Time.deltaTime);
    }

    public void SetTravelDirection(Vector2 direction, bool enemyProjectile = false)
    {
        if (direction.sqrMagnitude > Mathf.Epsilon)
            travelDirection = direction.normalized;

        isEnemyProjectile = enemyProjectile;
    }

    public void SetInstantKill(bool enabled, SpriteRenderer instantKillAppearance)
    {
        instantKill = enabled;

        if (projectileRenderer == null) return;

        if (enabled && instantKillAppearance != null)
        {
            projectileRenderer.sprite = instantKillAppearance.sprite;
            projectileRenderer.color = instantKillAppearance.color;
        }
        else
        {
            projectileRenderer.sprite = regularSprite;
            projectileRenderer.color = regularColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle trigger and solid hits the same way.
        HandleHit(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject target)
    {
        AriaCollisions aria = target.GetComponentInParent<AriaCollisions>();
        if (aria != null)
        {
            aria.TakeProjectileHit();
        }
        else if (!isEnemyProjectile)
        {
            BossHealth bossHealth = target.GetComponentInParent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(instantKill ? int.MaxValue : 1);
            }
            else
            {
                GameObject enemy = target;
                while (enemy != null && !enemy.CompareTag("Enemy"))
                {
                    enemy = enemy.transform.parent != null ? enemy.transform.parent.gameObject : null;
                }

                if (enemy != null)
                {
                    Minion minion = target.GetComponentInParent<Minion>();
                    if (minion != null)
                        minion.TakeDamage(instantKill ? int.MaxValue : 2);
                    else if (instantKill)
                        enemy.SetActive(false);
                }
            }
        }

        // Disabling the projectile sends it back to the pool.
        gameObject.SetActive(false);
    }

    // The Factory uses this to return the projectile to its pool.
    private void OnDisable() { OnDespawned.Invoke(); }
}