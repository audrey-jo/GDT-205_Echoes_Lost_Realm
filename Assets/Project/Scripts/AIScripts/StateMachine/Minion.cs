using UnityEngine;

public class Minion : MonoBehaviour
{
    public float maxHealth;
    public float damageDealt;
    public float wanderSpeed;
    public float chaseSpeed;
    public float retreatSpeed;
    [HideInInspector] public float currentSpeed;

    IMinionState currentState;

    public Wander wander;
    public Chase chase;
    public Retreat retreat;

    void Start()
    {
        wander = new Wander(this);
        chase = new Chase(this);
        retreat = new Retreat(this);

        ChangeState(wander);
    }

    void Update() { currentState?.UpdateLogic(); }

    public void ChangeState(IMinionState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
