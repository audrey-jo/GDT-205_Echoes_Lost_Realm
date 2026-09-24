/*
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

        //Tick down the duration.
        duration -= Time.deltaTime;

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Chasing);

        //Set the desired position.
        ghost.SetTargetBoardPosition(randomPosition);

        //Move the ghost.
        ghost.Move();

        //After the desired duration, we will return the status FAILURE to allow our parent node to select
        //another way to chase the player.
        if (duration <= 0.0f)
        {
            //Reset for next time.
            duration = kMaxDuration;
            ResetRandomPosition();

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