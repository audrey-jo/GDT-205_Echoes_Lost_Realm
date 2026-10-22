using UnityEngine;

public class GuardianSpiritSeek : MonoBehaviour
{
    [Header("Seek Settings")]
    public Transform target;

    public float maxSpeed = 4f;
    public float acceleration = 5f;
    public float slowDistance = 2f;
    public float stopDistance = 0.2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Seek();
    }

    void Seek()
    {
        // Find the target
        Vector2 direction = target.position - transform.position;

        // Find how far away the target is
        float distance = direction.magnitude;

        // Stops when close to the target
        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                acceleration * Time.fixedDeltaTime
            );

            return;
        }

        // Normalize the direction
        direction.Normalize();

        // Maximum speed
        float targetSpeed = maxSpeed;

        // Slow down when getting close to the target
        if (distance < slowDistance)
        {
            targetSpeed = maxSpeed * (distance / slowDistance);
        }

        // Calculate velocity
        Vector2 desiredVelocity = direction * targetSpeed;

        // Smoothly accelerate 
        rb.linearVelocity = Vector2.MoveTowards(
            rb.linearVelocity,
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );
    }
}
