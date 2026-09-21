using UnityEngine;

public class Sequence_GoHome : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Sequence_GoHome()
    {
        //Delete me.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Sequence_GoHome");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}