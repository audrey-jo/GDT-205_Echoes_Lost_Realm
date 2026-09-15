using UnityEngine;

public class Leaf_Evade : TreeNode_Base
{
    Vector2Int randomEvadePosition = new Vector2Int();

    //------------------------------------------------------------------------------

    public Leaf_Evade()
    {
        //Leaf - No children.

        //Choose a random position off the map to evade to - The ghost will never reach it,
        //but will keep circling the map in search of it.
        System.Random rnd = new System.Random();
        randomEvadePosition.x = rnd.Next(0,1) * GameWorld.BoardRows;
        randomEvadePosition.x = rnd.Next(0, 1) * GameWorld.BoardColumns;
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_Evade");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}
