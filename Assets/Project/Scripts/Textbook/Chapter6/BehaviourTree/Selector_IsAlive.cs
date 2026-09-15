using UnityEngine;

public class Selector_IsAlive : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Selector_IsAlive()
    {
        //Delete me.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Selector_IsAlive");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}