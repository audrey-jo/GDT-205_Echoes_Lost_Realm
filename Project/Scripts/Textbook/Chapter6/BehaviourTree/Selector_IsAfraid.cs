using UnityEngine;

public class Selector_IsAfraid : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Selector_IsAfraid()
    {
        //Delete me.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Selector_IsAfraid");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}