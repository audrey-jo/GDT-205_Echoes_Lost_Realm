using UnityEngine;

public class Leaf_MoveAheadOfPlayer : TreeNode_Base
{
    private const float kMaxDuration = 10.0f;
    private float       duration     = kMaxDuration;

    //------------------------------------------------------------------------------

    public Leaf_MoveAheadOfPlayer()
    {
        //Leaf - No children.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_MoveAheadOfPlayer");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}