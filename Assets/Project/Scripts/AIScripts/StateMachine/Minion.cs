using UnityEngine;

public class Minion : MonoBehaviour
{
    [Header("Health and Damage")]
    public float maxHealth;
    public float damageDealt;

    [Header("Movement")]
    public float maxSpeed;
    public float currentSpeed;
    public float acceleration;

    [Header("Wander")]
    public float wanderSpeed;
    public float minWanderDistance;
    public float wanderRadius;
    public float wanderChangeInterval;
    public float wanderPointReachedDistance;
    public float followRange;

    [Header("Chase")]

    public float chaseSpeed;

    [Header("Retreat")]

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

    public Wander wander;
    public Chase chase;
    public Retreat retreat;

    Vector2 steeringDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        wander = new Wander(this);
        chase = new Chase(this);
        retreat = new Retreat(this);

        ChangeState(wander);
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
}
