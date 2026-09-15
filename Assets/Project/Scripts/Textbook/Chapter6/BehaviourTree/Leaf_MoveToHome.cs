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

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}