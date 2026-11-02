/*
using UnityEngine;

public class Sequence_GoHome : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Sequence_GoHome()
    {
        childrenNodes.Add(new Leaf_MoveToHome());
        childrenNodes.Add(new Leaf_ExitHome());
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Sequence_GoHome");

        //Sequence needs all children to SUCCEED to be SUCCESSFUL.
        bool childIsRunning = false;

        for(int i =0; i < childrenNodes.Count; i++)
        {
            switch (childrenNodes[i].OnUpdate(ghost, player))
            {
                case Status.FAILURE:
                    //Leave immediately, we have failed in a child leaf node.
                    return Status.FAILURE;
                break;

                case Status.SUCCESS:
                    //Overall SUCCESS is determined below.
                    continue;
                break;

                case Status.RUNNING:
                    i = childrenNodes.Count;
                    childIsRunning = true;
                    continue;
                break;

                default:
                break;
            }
        }

        //If a child is still running, then we need to return a status of RUNNING.
        return childIsRunning ? Status.RUNNING : Status.SUCCESS;
    }

    //------------------------------------------------------------------------------
}
*/