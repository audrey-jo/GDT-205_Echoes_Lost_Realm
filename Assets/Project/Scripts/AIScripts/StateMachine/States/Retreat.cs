public class Retreat : IMinionState
{
    Minion minion;

    public Retreat(Minion minionController) {  minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.retreatSpeed;
    }

    public void UpdateLogic()
    {

    }

    public void Exit()
    {

    }
}
