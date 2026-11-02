/*using UnityEngine;

public class MoveToHome_State : Base_State
{
    private Vector2Int homePos = new Vector2Int(14, 13);

    //-------------------------------------------------------------------------------------

    public MoveToHome_State(Ghost g) : base(g)
    {
    }

    //-------------------------------------------------------------------------------------

    public override void OnEnter()
    {
        if (ghost)
        {
            ghost.ghostVisuals.SetSpriteSet(SpriteSet.Eyes);
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnUpdate()
    {
        //This is super simple - Just set the position we want to go to.
        if (ghost)
        {
            ghost.SetCurrentAIString("MoveToHome_State");

            ghost.SetTargetBoardPosition(homePos);

            ghost.MoveHome();
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnExit()
    {
        if (ghost)
        {
            ghost.SetEaten(false);
        }
    }

    //-------------------------------------------------------------------------------------

    public override GhostState CheckTransitions()
    {
        //MOVETOHOME can only move in to EXITHOME.
        if (ghost && ghost.GetBoardPosition() == homePos)
        {
            return GhostState.ExitHome;
        }
        else
        {
            return GhostState.ReturnToHome;
        }
    }

    //-------------------------------------------------------------------------------------
}
*/