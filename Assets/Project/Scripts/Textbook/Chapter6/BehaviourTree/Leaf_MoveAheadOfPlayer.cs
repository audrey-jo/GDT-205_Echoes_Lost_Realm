/*
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

        //Tick down the duration.
        duration -= Time.deltaTime;

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Chasing);

        //Calculate a position ahead of the player.
        Vector2Int positionAheadOfPlayer = new Vector2Int();
        switch (player.GetDirection())
        {
            case Direction.Left:
                positionAheadOfPlayer = player.GetBoardPosition() + new Vector2Int(-1, 0);
            break;

            case Direction.Right:
                positionAheadOfPlayer = player.GetBoardPosition() + new Vector2Int(1, 0);
            break;

            case Direction.Up:
                positionAheadOfPlayer = player.GetBoardPosition() + new Vector2Int(0, -1);
            break;

            case Direction.Down:
                positionAheadOfPlayer = player.GetBoardPosition() + new Vector2Int(0, 1);
            break;
        }

        //Check this position is valid, if not default to the players position.
        if (GameWorld.IsCellAccessible(positionAheadOfPlayer))
        {
            ghost.SetTargetBoardPosition(positionAheadOfPlayer);
        }
        else
        {
            ghost.SetTargetBoardPosition(player.GetBoardPosition());
        }

        //Move the ghost.
        ghost.Move();

        //After the desired duration, we will return the status FAILURE to allow our parent node to select
        //another way to chase the player.
        if (duration <= 0.0f)
        {
            //Reset for next time.
            duration = kMaxDuration;

            //We moved in this manner for the duration
            return Status.SUCCESS;
        }
        else
        {
            return Status.RUNNING;
        }
    }

    //------------------------------------------------------------------------------
}

*/