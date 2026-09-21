using UnityEngine;

public class Selector_Chase : TreeNode_Base
{
    private int previousRunningNode = 0;

    //------------------------------------------------------------------------------

    public Selector_Chase()
    {
        //Delete me.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Selector_Chase");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}