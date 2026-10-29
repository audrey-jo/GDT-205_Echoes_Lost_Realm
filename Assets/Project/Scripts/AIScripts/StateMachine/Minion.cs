using System;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [Header("Health and Damage")]

    public float maxHealth;
    public float damageDealt;
    public float retreatHealth;

    [Header("Movement")]

    
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
    public float chaseRange = 50f;

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

    [HideInInspector] public float currentSpeed;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D spiritCollider;
    [HideInInspector] public Collider2D ariaCollider;

    IMinionState currentState;

    public WanderState wanderState;
    public ChaseState chaseState;
    public RetreatState retreatState;

    Vector2 steeringDirection;
    [HideInInspector] public float currentHealth;

    public static event Action OnDeath;

    private void OnEnable()
    {
        SpawnedObject.HitEnemy += TakeDamage;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        wanderState = new WanderState(this);
        chaseState = new ChaseState(this);
        retreatState = new RetreatState(this);

        Activate();
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

    private void OnDisable()
    {
        SpawnedObject.HitEnemy -= TakeDamage;
    }

    // STAT UPDATES

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Deactivate();
        if (currentHealth <= retreatHealth) ChangeState(retreatState);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnDeath?.Invoke();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        ChangeState(wanderState);
    }

    // MOVEMENT

    protected Vector2 AvoidObstacles(Vector2 movementDirection)
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

    protected Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }

    public Vector2 CalculateDirection(Vector2 aPos, Vector2 bPos) { return (aPos - bPos).normalized; }

    public float GetDistance(Vector2 aPos, Vector2 bPos) { return Vector2.Distance(aPos, bPos); }
}
