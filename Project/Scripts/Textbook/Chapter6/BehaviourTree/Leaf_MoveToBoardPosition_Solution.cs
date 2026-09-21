/*
using UnityEngine;

public class Leaf_MoveToBoardPosition : TreeNode_Base
{
    private Vector2Int targetPosition;

    //------------------------------------------------------------------------------

    public Leaf_MoveToBoardPosition(Vector2Int positionToMoveTo)
    {
        //Leaf - No children.

        targetPosition = positionToMoveTo;
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_MoveToBoardPosition");

        //Set the desired position.
        ghost.SetTargetBoardPosition(targetPosition);

        //Move the ghost.
        ghost.Move();

        //Have we reached the target position yet?
        if (ghost.GetBoardPosition() == targetPosition)
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