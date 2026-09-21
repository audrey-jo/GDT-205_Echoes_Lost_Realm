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

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}