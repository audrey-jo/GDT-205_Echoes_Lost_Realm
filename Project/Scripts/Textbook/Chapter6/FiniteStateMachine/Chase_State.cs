using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChaseType
{
    PlayerPosition,
    BehindPlayer,
    AheadOfPlayer,
    Random
};

//-------------------------------------------------------------------------------------

public class Chase_State : Base_State
{
    private ChaseType   chaseType   = ChaseType.PlayerPosition;
    private Player      player      = null;

    //-------------------------------------------------------------------------------------

    public Chase_State(Ghost g, ChaseType typeOfChase, Player p) : base(g)
    {
        chaseType   = typeOfChase;
        player      = p;
    }

    //-------------------------------------------------------------------------------------

    public override void OnEnter()
    {
        if (ghost)
        { 
            ghost.ghostVisuals.SetSpriteSet(SpriteSet.Chasing);
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnUpdate()
    {
        if (ghost)
        {
            ghost.SetCurrentAIString("Chase_State");

            switch (chaseType)
            {
                case ChaseType.PlayerPosition:
                    ghost.SetTargetBoardPosition(player.GetBoardPosition());
                break;

                //Check which direction the player is facing, and set the position ahead.
                case ChaseType.BehindPlayer:
                    Vector2Int positionBehindPlayer = new Vector2Int();
                    switch (player.GetDirection())
                    {
                        case Direction.Left:
                            positionBehindPlayer = player.GetBoardPosition() + new Vector2Int(1, 0);
                        break;

                        case Direction.Right:
                            positionBehindPlayer = player.GetBoardPosition() + new Vector2Int(-1, 0);
                        break;

                        case Direction.Up:
                            positionBehindPlayer = player.GetBoardPosition() + new Vector2Int(0, 1);
                        break;

                        case Direction.Down:
                            positionBehindPlayer = player.GetBoardPosition() + new Vector2Int(0, -1);
                        break;
                    }

                    //Check this position is valid, if not default to the players position.
                    if (GameWorld.IsCellAccessible(positionBehindPlayer))
                        ghost.SetTargetBoardPosition(positionBehindPlayer);
                    else
                        ghost.SetTargetBoardPosition(player.GetBoardPosition());
                break;

                //Check which direction the player is facing, and set the position ahead.
                case ChaseType.AheadOfPlayer:
                    Vector2Int positionAheadOfPlayer = new Vector2Int();
                    switch (player.GetDirection())
                    {
                        case Direction.Left:
                            positionAheadOfPlayer = player.GetBoardPosition()+new Vector2Int(-1, 0);
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
                        ghost.SetTargetBoardPosition(positionAheadOfPlayer);
                    else
                        ghost.SetTargetBoardPosition(player.GetBoardPosition());
                break;

                case ChaseType.Random:
                    //This changes every frame, but the ghost will keep heading in the same direction until it
                    //reaches an intersection.
                    ghost.SetTargetBoardPosition(new Vector2Int(Random.Range(0, 28), Random.Range(0, 32)));
                break;

                default:
                break;
            }

            ghost.Move();
        }
    }

    //-------------------------------------------------------------------------------------

    public override void OnExit()
    {

    }

    //-------------------------------------------------------------------------------------

    public override GhostState CheckTransitions()
    {
        //CHASE can move into EVADE.
        if (ghost && ghost.IsPowerPillActive())
            return GhostState.Evade;

        //Stick with the current state.
        return GhostState.Chase;
    }

    //-------------------------------------------------------------------------------------
}
