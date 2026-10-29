using UnityEngine;

public class Minion : MonoBehaviour
{
    [Header("Health and Damage")]

    public float maxHealth;
    public float damageDealt;
    public float retreatHealth;
    public float chaseHealth;

    [Header("Movement")]

    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;

    [Header("Wander")]

    public Transform wanderTarget;
    public float wanderSpeed;
    public float minWanderDistance;
    public float wanderRadius;
    public float wanderChangeInterval;
    public float wanderPointReachedDistance;
    public float followRange;

    [Header("Chase")]

    public Transform chaseTarget;
    public float chaseSpeed;

    [Header("Retreat")]

    public Transform retreatTarget;
    public LayerMask ariaLayer;
    public float ariaAvoidanceDistance;
    public float ariaAvoidanceStrength;
    public float retreatSpeed;

    [Header("Obstacle Avoidance")]

    public LayerMask obstacleLayer;
    public float obstacleDetectionDistance;
    public float obstacleAvoidanceStrength;
    public float sideRayAngle;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D spiritCollider;
    [HideInInspector] public Collider2D ariaCollider;

    IMinionState currentState;

    public WanderState wanderState;
    public ChaseState chaseState;
    public RetreatState retreatState;

    Vector2 steeringDirection;
    float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        wanderState = new WanderState(this);
        chaseState = new ChaseState(this);
        retreatState = new RetreatState(this);

        Revive();

        ChangeState(wanderState);
    }

    void Update()
    {
        currentState?.UpdateLogic();
        steeringDirection += AvoidObstacles(steeringDirection);
    }

    public void ChangeState(IMinionState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // STAT UPDATES

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die() { gameObject.SetActive(false); }

    private void Revive()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
    }

    // STATE CHANGES

    public void Wander() { ChangeState(wanderState); }

    public void Chase() { ChangeState(chaseState); }

    public void Retreat() { ChangeState(retreatState); }

    // MOVEMENT

    private Vector2 AvoidObstacles(Vector2 movementDirection)
    {
        if (movementDirection.sqrMagnitude < 0.01f) return Vector2.zero;

        Vector2 forward = movementDirection.normalized;
        Vector2 leftDirection = RotateVector(forward, sideRayAngle);
        Vector2 rightDirection = RotateVector(forward, -sideRayAngle);

        RaycastHit2D centerHit = Physics2D.Raycast( rb.position, forward, obstacleDetectionDistance, obstacleLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(rb.position, leftDirection, obstacleDetectionDistance, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rb.position, rightDirection, obstacleDetectionDistance, obstacleLayer);

        Vector2 avoidance = Vector2.zero;

        if (centerHit.collider != null) avoidance += centerHit.normal * obstacleAvoidanceStrength;
        if (leftHit.collider != null) avoidance += RotateVector(forward,-90f) * obstacleAvoidanceStrength;
        if (rightHit.collider != null) avoidance += RotateVector(forward, 90f) * obstacleAvoidanceStrength;

        return avoidance;
    }

    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }

    public Vector2 CalculateDirection(Vector2 aPos, Vector2 bPos) { return (aPos - bPos).normalized; }

    public float GetDistance(Vector2 aPos, Vector2 bPos) { return Vector2.Distance(aPos, bPos); }
}
