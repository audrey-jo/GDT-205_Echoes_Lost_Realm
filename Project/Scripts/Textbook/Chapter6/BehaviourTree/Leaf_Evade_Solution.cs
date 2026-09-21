/*
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

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Evading);

        //Set the desired position.
        ghost.SetTargetBoardPosition(randomEvadePosition);

        //Move the ghost.
        ghost.Move();

        //We don't expect the ghost to actually reach this position (its off the board), but
        //it needs to keep trying until the power pill has worn off (Parent will handle this)
        return Status.RUNNING;
    }

    //------------------------------------------------------------------------------
}

*/
