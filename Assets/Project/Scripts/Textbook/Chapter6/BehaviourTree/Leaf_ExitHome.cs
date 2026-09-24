/*
using UnityEngine;

public class Leaf_ExitHome : TreeNode_Base
{
    private Vector2Int exitPos = new Vector2Int(14, 11);

    //------------------------------------------------------------------------------

    public Leaf_ExitHome()
    {
        //Leaf - No children.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_ExitHome");

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Chasing);

        //Set the desired position.
        ghost.SetTargetBoardPosition(exitPos);

        //Navigate out of the home.
        ghost.MoveHome();

        //Return a status to show our progress. (Failure not an option).
        if (ghost.GetBoardPosition() == exitPos)
        {
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