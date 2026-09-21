/*
using UnityEngine;

public class Leaf_MoveToPlayer : TreeNode_Base
{
    private const float kMaxDuration = 10.0f;
    private float       duration     = kMaxDuration;

    //------------------------------------------------------------------------------

    public Leaf_MoveToPlayer()
    {
        //Leaf - No children.
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Leaf_MoveToPlayer");

        //Tick down the duration.
        duration -= Time.deltaTime;

        //Change the visuals of the ghost.
        ghost.ghostVisuals.SetSpriteSet(SpriteSet.Chasing);

        //Set the desired position.
        ghost.SetTargetBoardPosition(player.GetBoardPosition());

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