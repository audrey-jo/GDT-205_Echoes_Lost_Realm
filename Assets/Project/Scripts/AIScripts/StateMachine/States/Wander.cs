using UnityEngine;

public class Wander : IMinionState
{
    Minion minion;

    float minWanderDistance;
    float wanderRadius;
    float wanderChangeInterval;
    float wanderPointReachedDistance;
    float wanderTimer;
    bool hasWanderTarget;
    float acceleration;

    Vector2 wanderTarget;
    Transform boss;

    public Wander(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.wanderSpeed;
        boss = GameObject.Find("Boss").GetComponent<Transform>();

        minWanderDistance = minion.minWanderDistance;
        wanderRadius = minion.wanderRadius;
        wanderChangeInterval = minion.wanderChangeInterval;
        wanderPointReachedDistance = minion.wanderPointReachedDistance;
        acceleration = minion.acceleration;
    }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = CalculateWander();

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);
    }

    public void Exit()
    {

    }

    private Vector2 CalculateWander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        bool reachedWanderTarget = hasWanderTarget && Vector2.Distance(minion.rb.position, wanderTarget) <= wanderPointReachedDistance;

        bool wanderPointTooFar = hasWanderTarget && Vector2.Distance(wanderTarget, boss.position) > wanderRadius;

        bool wanderPointTooClose = hasWanderTarget && Vector2.Distance(wanderTarget, boss.position) < minWanderDistance;

        if (!hasWanderTarget || wanderTimer <= 0f || reachedWanderTarget ||
            wanderPointTooFar || wanderPointTooClose) ChooseNewWanderTarget();

        Vector2 directionToTarget = wanderTarget - minion.rb.position;

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
