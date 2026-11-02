using Unity.VisualScripting.FullSerializer;
using UnityEngine;

// Inherits from IMinionState rather than MonoBehavior
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
    float healthNum;

    // Constructor; sets minion to the controller
    public WanderState(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        // Sets everything needed to the values set in the inspector
        // (can't set states in the inspector since it doesn't inherit from MonoBehavior)

        minion.currentSpeed = minion.wanderSpeed;

        minWanderDistance = minion.minWanderDistance;
        wanderRadius = minion.wanderRadius;
        wanderChangeInterval = minion.wanderChangeInterval;
        wanderPointReachedDistance = minion.wanderPointReachedDistance;
        acceleration = minion.acceleration;

    }

    public void UpdateLogic()
    {
        if (minion.GetDistance(minion.transform.position, minion.wanderTarget.position) <= wanderRadius) healthNum += 0.1f;
        if (healthNum >= 1)
        {
            minion.currentHealth = Mathf.Min(minion.currentHealth + 1, minion.maxHealth);
            healthNum = 0;
        }

        Vector2 steeringDirection = CalculateWander();

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, acceleration * Time.fixedDeltaTime);

        // Condition to change out of state
        if (minion.GetDistance(minion.transform.position, minion.chaseTarget.position) <= minion.chaseRange &&
            minion.currentHealth > minion.retreatHealth) minion.ChangeState(minion.chaseState);
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
