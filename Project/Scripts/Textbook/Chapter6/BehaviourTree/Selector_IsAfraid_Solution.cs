/*
using UnityEngine;

public class Selector_IsAfraid : TreeNode_Base
{
    //------------------------------------------------------------------------------

    public Selector_IsAfraid()
    {
        //Delete me.

        childrenNodes.Add(new Leaf_ExitHome());
        childrenNodes.Add(new Leaf_Evade());
        childrenNodes.Add(new Selector_Chase());
    }

    //------------------------------------------------------------------------------

    public override Status OnUpdate(Ghost ghost, Player player)
    {
        ghost.AddToCombinedAIString("Selector_IsAfraid");

        //We need to make sure the ghost has left his home (If it is dead it will head down a different branch of the tree).
        //This is purely for a living ghost at home (Think: At the start of the game)
        if (ghost.IsInHome())
        {
            return childrenNodes[0].OnUpdate(ghost, player);        //Leaf_ExitHome
        }
        if (ghost.IsPowerPillActive())
        {
            return childrenNodes[1].OnUpdate(ghost, player);        //Evade
        }
        else
        {
            return childrenNodes[2].OnUpdate(ghost, player);        //Chase
        }
    }

    //------------------------------------------------------------------------------
}
*/