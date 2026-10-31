using UnityEngine;

// Inherits from IMinionState rather than MonoBehavior
public class RetreatState : IMinionState
{
    Minion minion;

    // Constructor; sets minion to the controller
    public RetreatState(Minion minionController) { minion = minionController; }

    // Sets everything needed to the values set in the inspector
    // (can't set states in the inspector since it doesn't inherit from MonoBehavior)
    public void Enter() { minion.currentSpeed = minion.retreatSpeed; }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = minion.CalculateDirection(minion.retreatTarget.position, minion.rb.position);

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, minion.acceleration * Time.fixedDeltaTime);

        // Condition to change out of state
        if (minion.GetDistance(minion.transform.position, minion.retreatTarget.position) <= minion.wanderRadius)
            minion.ChangeState(minion.wanderState);
    }

    public void Exit() { }
}
