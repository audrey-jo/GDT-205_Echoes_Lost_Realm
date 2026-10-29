using UnityEngine;

public class ChaseState : IMinionState
{
    Minion minion;

    public ChaseState(Minion minionController) { minion = minionController; }

    public void Enter() { minion.currentSpeed = minion.chaseSpeed; }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = minion.CalculateDirection(minion.chaseTarget.position, minion.rb.position);

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, minion.acceleration * Time.fixedDeltaTime);
    }

    public void Exit() { }
}
