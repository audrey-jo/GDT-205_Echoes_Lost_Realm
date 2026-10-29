using UnityEngine;

public class RetreatState : IMinionState
{
    Minion minion;

    public RetreatState(Minion minionController) { minion = minionController; }

    public void Enter() { minion.currentSpeed = minion.retreatSpeed; }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = minion.CalculateDirection(minion.retreatTarget.position, minion.rb.position);

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, minion.acceleration * Time.fixedDeltaTime);

        if (minion.GetDistance(minion.transform.position, minion.retreatTarget.position) <= minion.wanderRadius)
            minion.ChangeState(minion.wanderState);
    }

    public void Exit() { }
}
