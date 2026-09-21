/*
using UnityEngine;

public class Selector_Chase : TreeNode_Base
{
    private int previousRunningNode = 0;

    //------------------------------------------------------------------------------

    public Selector_Chase()
    {
        childrenNodes.Add(new Leaf_MoveToPlayer());
        childrenNodes.Add(new Leaf_MoveAheadOfPlayer());
        childrenNodes.Add(new Leaf_MoveBehindPlayer());
        childrenNodes.Add(new Leaf_MoveToRandomPosition());

        //Choose a random index for the next update.
        System.Random rnd = new System.Random();
        previousRunningNode = rnd.Next(0, childrenNodes.Count);
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Selector_Chase");

        Status ghostStatus = childrenNodes[previousRunningNode].OnUpdate(ghost, player);

        if (ghostStatus == Status.SUCCESS)
        {
            //Choose a random index for the next update.
            System.Random rnd = new System.Random();
            previousRunningNode = rnd.Next(0, childrenNodes.Count);
        }

        return ghostStatus;
    }

    //------------------------------------------------------------------------------
}
*/