public class Chase : IMinionState
{
    Minion minion;

    public Chase(Minion minionController) { minion = minionController; }

    public void Enter()
    {
        minion.currentSpeed = minion.chaseSpeed;
    }

    public void UpdateLogic()
    {

    }

    public void Exit()
    {

    }
}
