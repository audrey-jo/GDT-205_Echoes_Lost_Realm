using System.Collections.Generic;
using UnityEngine;

public class TreeNode_Base
{
    protected List<TreeNode_Base> childrenNodes = new List<TreeNode_Base>();

    //------------------------------------------------------------------------------

    public TreeNode_Base()
    {
        //Debug.Log("TreeNode_Base - Constructor() - Override required");
    }

    //------------------------------------------------------------------------------

    public virtual Status OnUpdate(Ghost ghost, Player player)
    {
        if (ghost)
        {
            ghost.SetCurrentAIString("TreeNode_Base");
        }

        Debug.Log("TreeNode_Base - OnUpdate() - Override required");
        return Status.RUNNING;
    }

    //------------------------------------------------------------------------------
}
