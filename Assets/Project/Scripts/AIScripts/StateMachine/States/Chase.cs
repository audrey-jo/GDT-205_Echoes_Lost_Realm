using UnityEngine;

public class Chase : IMinionState
{
    Minion minion;
    Vector2 chaseTarget;

    public Chase(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.chaseSpeed;
        chaseTarget = GameObject.Find("Aria").transform.position;

    }

    public void UpdateLogic()
    {
        Vector2 steeringDirection = CalculateChase();

        if (steeringDirection.sqrMagnitude > 0.001f) steeringDirection.Normalize();

        Vector2 desiredVelocity = steeringDirection * minion.currentSpeed;
        minion.rb.linearVelocity = Vector2.MoveTowards(minion.rb.linearVelocity, desiredVelocity, minion.acceleration * Time.fixedDeltaTime);
    }

    public void Exit()
    {

    }

    private Vector2 CalculateChase()
    {
        Vector2 directionToTarget = chaseTarget - minion.rb.position;
        return directionToTarget.normalized;
    }
}
