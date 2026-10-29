using UnityEngine;

public class WanderState : IMinionState
{
    Minion minion;

    float minWanderDistance;
    float wanderRadius;
    float wanderChangeInterval;
    float wanderPointReachedDistance;
    float wanderTimer;
    bool hasWanderPoint;
    float acceleration;

    Vector2 wanderPoint;

    public WanderState(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.wanderSpeed;

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

    private Vector2 CalculateWander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        bool reachedWanderTarget = hasWanderPoint && Vector2.Distance(minion.rb.position, wanderPoint) <= wanderPointReachedDistance;
        bool wanderPointTooFar = hasWanderPoint && Vector2.Distance(wanderPoint, minion.wanderTarget.position) > wanderRadius;
        bool wanderPointTooClose = hasWanderPoint && Vector2.Distance(wanderPoint, minion.wanderTarget.position) < minWanderDistance;

        if (!hasWanderPoint || wanderTimer <= 0f || reachedWanderTarget ||
            wanderPointTooFar || wanderPointTooClose) ChooseNewWanderTarget();

        Vector2 directionToTarget = wanderPoint - minion.rb.position;

        return directionToTarget.normalized;
    }

    private void ChooseNewWanderTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minWanderDistance, wanderRadius);

        wanderPoint = (Vector2) minion.wanderTarget.position + randomDirection * randomDistance;
        wanderTimer = wanderChangeInterval;
        hasWanderPoint = true;
    }

    public void Exit() { }
}
