/*
using UnityEngine;

public class Leaf_MoveToHome : TreeNode_Base
{
    private Vector2Int homePos = new Vector2Int(14, 13);

    //------------------------------------------------------------------------------

    public Leaf_MoveToHome()
    {
        //Leaf - No children.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_MoveToHome");

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Eyes);

        //Set the desired position.
        ghost.SetTargetBoardPosition(homePos);

        //Move the ghost.
        ghost.MoveHome();

        //Return a status to show our progress. (Failure not an option).
        if (ghost.GetBoardPosition() == homePos)
        {
            ghost.SetEaten(false);
            return Status.SUCCESS;
        }
        else
        {
            return Status.RUNNING;
        }
    }

    //------------------------------------------------------------------------------
}
*/