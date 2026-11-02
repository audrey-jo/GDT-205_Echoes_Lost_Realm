/*
using UnityEngine;

public class Selector_IsAlive : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Selector_IsAlive()
    {
        childrenNodes.Add(new Selector_IsAfraid());
        childrenNodes.Add(new Sequence_GoHome());
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        //This is for visual debug output.
        ghost.AddToCombinedAIString("Selector_IsAlive");

        if (!ghost.HasBeenEaten())
        {
            return childrenNodes[0].OnUpdate(ghost, player);    //Selector_IsAfraid
        }
        else
        {
            return childrenNodes[1].OnUpdate(ghost, player);    //Sequence_GoHome
        }
    }

    //------------------------------------------------------------------------------
}
*/