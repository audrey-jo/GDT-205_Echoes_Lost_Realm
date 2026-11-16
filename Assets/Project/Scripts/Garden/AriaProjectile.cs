using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class AriaProjectile : MonoBehaviour
{
    // The projectile moves at a steady speed and expires if it never hits a guardian.
    [SerializeField, Min(0f)] private float speed = 20f;
    [SerializeField, Min(0.1f)] private float lifetime = 5f;

    private Rigidbody2D body;
    private Vector2 travelDirection;
    private float remainingLifetime;

    private void Awake()
    {
        // Use a trigger so scenery does not block the projectile.
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Initialize(Vector2 direction, float projectileSpeed)
    {
        // The shooter sets the direction and speed right after creating this object.
        travelDirection = direction.normalized;
        speed = Mathf.Max(0f, projectileSpeed);
        remainingLifetime = lifetime;
    }

    private void FixedUpdate()
    {
        remainingLifetime -= Time.fixedDeltaTime;
        if (remainingLifetime <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        body.MovePosition(body.position + travelDirection * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only guardians stop the projectile and take damage.
        GuardianStatues guardian = other.GetComponentInParent<GuardianStatues>();
        if (guardian == null)
            return;

        guardian.TakeDamage(AriaCollisions.InstantKillProjectilesEnabled ? int.MaxValue : 1);
        Destroy(gameObject);
    }
}
