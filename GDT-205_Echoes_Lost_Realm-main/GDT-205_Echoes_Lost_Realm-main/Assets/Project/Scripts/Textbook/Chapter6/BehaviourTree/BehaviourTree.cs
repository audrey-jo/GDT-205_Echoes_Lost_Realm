/*using UnityEngine;

//-------------------------------------------------------------------------------------
//                                    Tree Structure
//-------------------------------------------------------------------------------------
//
//                                      --------
//                                      | Root |
//                                      --------
//                                         |
//                                --------------------
//                                | Selector_IsAlive |
//                                --------------------
//                                  /               \
//                    ---------------------         -------------------
//                    | Selector_IsAfraid |         | Sequence_GoHome |
//                    ---------------------         -------------------
//                 /            |           \                |         \
//        ----------      ---------------   -------     ------------   -----------
//        |  Leaf_ |      |  Selector_  |  |Leaf_ |     |  Leaf_   |   |  Leaf_  |
//        |ExitHome|      |    Chase    |  |Evade |     |MoveToHome|   | ExitHome|
//        ----------      ---------------  --------     ------------   -----------
//                       /      /   \   \
//                  -----      /     \   --------------
//                 /          /       \                \
//      -----------  ---------------   --------------   ----------------  
//      |Leaf_Move|  | Leaf_Move   |   | Leaf_Move  |   |Leaf_MoveTo   |
//      | ToPlayer|  |AheadOfPlayer|   |BehindPlayer|   |RandomPosition|
//      -----------  ---------------   ---- ---------   ----------------  
//
//-------------------------------------------------------------------------------------

public enum Status
{
    SUCCESS,
    FAILURE,
    RUNNING
};

//-------------------------------------------------------------------------------------

[RequireComponent(typeof(Ghost))]
public class BehaviourTree : MonoBehaviour
{
    private TreeNode_Base rootNode          = new Selector_IsAlive();
    private Ghost         ghost             = null;
    private Player        player            = null;

    //-------------------------------------------------------------------------------------

    void Start()
    {
        ghost  = GetComponent<Ghost>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //-------------------------------------------------------------------------------------

    void Update()
    {
        if (GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            if (ghost != null && player != null)
            {
                ghost.SetCurrentAIString("");
                ghost.ClearCombinedAIString();

                //Process the behaviour tree from the root node.
                rootNode.OnUpdate(ghost, player);
            }
        }
    }

    //-------------------------------------------------------------------------------------
}
*/