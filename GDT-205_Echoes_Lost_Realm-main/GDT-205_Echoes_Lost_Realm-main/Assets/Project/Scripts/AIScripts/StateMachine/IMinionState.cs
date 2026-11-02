public interface IMinionState
{
    // Interface that all of the states inherit from
    // Each state must implement these three methods

    // Actions upon entering state
    void Enter();

    // Actions while in state
    void UpdateLogic();

    // Actions upon exiting state
    void Exit();
}