using UnityEngine;

public class Wander : IMinionState
{
    Minion minion;

    float minWanderDistance = 1.5f;
    float wanderRadius = 3f;
    float wanderChangeInterval = 2f;
    float wanderPointReachedDistance = 0.35f;
    float wanderTimer;
    bool hasWanderTarget = false;
    float acceleration = 6f;

    Rigidbody2D rb;
    Collider2D minionCollider;
    Collider2D bossCollider;
    Vector2 wanderTarget;
    Transform boss;

    public Wander(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.wanderSpeed;
    }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = CalculateWander();

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
    }

    public void Exit()
    {

    }

    private Vector2 CalculateWander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        bool reachedWanderTarget =
            hasWanderTarget &&
            Vector2.Distance(
                rb.position,
                wanderTarget
            ) <= wanderPointReachedDistance;

        bool wanderPointTooFar =
            hasWanderTarget &&
            Vector2.Distance(
                wanderTarget,
                boss.position
            ) > wanderRadius;

        bool wanderPointTooClose = hasWanderTarget && Vector2.Distance(wanderTarget, boss.position) < minWanderDistance;

        if (!hasWanderTarget || wanderTimer <= 0f || reachedWanderTarget ||
            wanderPointTooFar || wanderPointTooClose) ChooseNewWanderTarget();

        Vector2 directionToTarget = wanderTarget - rb.position;

        return directionToTarget.normalized;
    }

    private void ChooseNewWanderTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float randomDistance = Random.Range(minWanderDistance, wanderRadius);

        wanderTarget = (Vector2) boss.position + randomDirection * randomDistance;

        wanderTimer = wanderChangeInterval;

        hasWanderTarget = true;
    }
}
