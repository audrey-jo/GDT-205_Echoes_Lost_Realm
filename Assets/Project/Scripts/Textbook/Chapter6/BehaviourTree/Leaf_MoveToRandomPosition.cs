using UnityEngine;

public class Leaf_MoveToRandomPosition : TreeNode_Base
{
    private const float kMaxDuration = 10.0f;
    private float duration = kMaxDuration;

    Vector2Int randomPosition = new Vector2Int();

    //------------------------------------------------------------------------------

    public Leaf_MoveToRandomPosition()
    {
        //Leaf - No children.

        //This is for visual debug output.
        ResetRandomPosition();
    }

    //------------------------------------------------------------------------------

    private void ResetRandomPosition()
    {
        System.Random rnd = new System.Random();
        randomPosition.x = rnd.Next(1, GameWorld.BoardRows - 2);
        randomPosition.x = rnd.Next(1, GameWorld.BoardColumns - 2);
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Leaf_MoveToRandomPosition");

        //Delete me.
        return Status.FAILURE;
    }

    //------------------------------------------------------------------------------
}
