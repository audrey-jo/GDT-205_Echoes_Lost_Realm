/*using UnityEngine;

//-------------------------------------------------------------------------------------------
[RequireComponent(typeof(GhostVisuals)), RequireComponent(typeof(AudioSource))]
public class Ghost : MonoBehaviour
{
    private string                              currentAINode   = "";
    private string                              combinedAINodes = "";

    public  float                               moveSpeed    = 1.0f;
    private float                               afraidTime   = 0.0f;
    private bool                                eaten        = false;
    private Player                              player       = null;

    [HideInInspector]  public GhostVisuals      ghostVisuals = null;

    private Vector2Int                          boardPos;
    private Vector2Int                          boardTargetPos;
    private Vector2Int                          boardDownPos;
    private Vector2Int                          boardLeftPos;
    private Vector2Int                          boardRightPos;
    private Vector2Int                          boardUpPos;

    private Vector2Int                          boardPosAtLastDirectionChange;

    private Vector2Int                          playerBoardPos;

    //-------------------------------------------------------------------------------------------

    private void Start()
    {
        //Where are we currently on the board?
        SetBoardPositions();

        player          = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ghostVisuals    = GetComponent<GhostVisuals>();
    }

    //-------------------------------------------------------------------------------------------

    private void SetBoardPositions()
    {
        Vector3 offsetPosition = transform.position;
        offsetPosition -= new Vector3(0.05f, 0.25f, 0.0f);
        boardPos = GameWorld.GetBoardPosition(offsetPosition);
        boardLeftPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.left * 0.75f);
        boardRightPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.right * 0.75f);
        boardUpPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.up * 0.5f);
        boardDownPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.down * 1.0f);
    }

    //-------------------------------------------------------------------------------------------

    private void Update()
    {
        if (GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            //Where are we currently on the board?
            SetBoardPositions();

            //Count the afraid time down.
            if (afraidTime > 0.0f)
            {
                afraidTime -= Time.deltaTime;

                //Flash the ghost in the last 1/2 second to alert the player.
                if (afraidTime < 1.0f)
                {
                    if (ghostVisuals)
                    {
                        ghostVisuals.Flash();
                    }
                }
            }
            else
            {
                if (ghostVisuals)
                {
                    ghostVisuals.SetVisible();
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------

    public bool IsPowerPillActive()
    {
        return afraidTime > 0.0f;
    }

    //-------------------------------------------------------------------------------------------

    public void SetEaten(bool isEaten)
    {
        if(eaten != isEaten)
        {
            eaten = isEaten;
        }
    }

    //-------------------------------------------------------------------------------------------

    public bool HasBeenEaten()
    {
        return eaten;
    }

    //-------------------------------------------------------------------------------------------

    public void SetTargetBoardPosition(Vector2Int targetPos)
    {
        boardTargetPos = targetPos;
    }

    //-------------------------------------------------------------------------------------------

    public Vector2Int GetTargetBoardPosition()
    {
        return boardTargetPos;
    }


    //-------------------------------------------------------------------------------------------

    public Vector2Int GetBoardPosition()
    {
        return boardPos;
    }

    //-------------------------------------------------------------------------------------------

    public void SetAfraid(float duration)
    {
        afraidTime = duration;
    }

    //-------------------------------------------------------------------------------------------

    public bool IsInHome()
    {
        return GameWorld.ReturnCellValue(boardPos) == 2;
    }

    //-------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If colliding with the player, either kill player or get killed - Depends if currently afraid.
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsPowerPillActive())
            {
                SetEaten(true);
            }
            else if (!HasBeenEaten())
            {
                player.SetEaten();
            }
        }
    }

    //-------------------------------------------------------------------------------------------

    //Move() allows only movement outside of the ghost house.
    public void Move()
    {
        if (ghostVisuals)
        {
            //Directional change.
            if (boardPosAtLastDirectionChange.x != boardPos.x || boardPosAtLastDirectionChange.y != boardPos.y)
            {
                //Remember where we were when we made this decision.
                boardPosAtLastDirectionChange = boardPos;

                //Can we change direction?
                if (ghostVisuals.GetDirection() != Direction.Up && boardTargetPos.y > boardPos.y && GameWorld.IsCellAccessible(boardDownPos))     //Go Down
                {
                    MoveDown();
                }
                else if (ghostVisuals.GetDirection() != Direction.Down && boardTargetPos.y < boardPos.y && GameWorld.IsCellAccessible(boardUpPos))       //Go Up
                {
                    MoveUp();
                }
                else if (ghostVisuals.GetDirection() != Direction.Right && boardTargetPos.x < boardPos.x && GameWorld.IsCellAccessible(boardLeftPos))          //Go Left
                {
                    MoveLeft();
                }
                
                else if (ghostVisuals.GetDirection() != Direction.Left && boardTargetPos.x > boardPos.x && GameWorld.IsCellAccessible(boardRightPos))    //Go Right
                {
                    MoveRight();
                }
                else
                {
                    //No options are good, so keep going in current direction.
                    MoveInCurrentDirection();
                }
            }
            else
            {
                //Still on the same position when we last made a decision, so keep going.
                MoveInCurrentDirection();
            }
        }
    }

    //-------------------------------------------------------------------------------------------

    //MoveHome() allows ghosts to move withing the house.
    public void MoveHome()
    {
        if (ghostVisuals)
        {
            //Directional change.
            if (boardPosAtLastDirectionChange.x != boardPos.x || boardPosAtLastDirectionChange.y != boardPos.y)
            {
                //Remember where we were when we made this decision.
                boardPosAtLastDirectionChange = boardPos;

                //Can we change direction?
                if (ghostVisuals.GetDirection() != Direction.Up && boardTargetPos.y > boardPos.y && GameWorld.IsCellAccessibleToGhost(boardDownPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)     //Go Down
                {
                    MoveDown();
                }
                else if (ghostVisuals.GetDirection() != Direction.Right && boardTargetPos.x < boardPos.x && GameWorld.IsCellAccessibleToGhost(boardLeftPos))          //Go Left
                {
                    MoveLeft();
                }
                else if (ghostVisuals.GetDirection() != Direction.Down && boardTargetPos.y < boardPos.y && GameWorld.IsCellAccessibleToGhost(boardUpPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)       //Go Up
                {
                    MoveUp();
                }
                else if (ghostVisuals.GetDirection() != Direction.Left && boardTargetPos.x > boardPos.x && GameWorld.IsCellAccessibleToGhost(boardRightPos))    //Go Right
                {
                    MoveRight();
                }
                else
                {
                    //No options are good, so keep going in current direction.
                    MoveInCurrentDirection();
                }
            }
            else
            {
                //Still on the same position when we last made a decision, so keep going.
                MoveInCurrentDirection();
            }
        }
    }

    //-------------------------------------------------------------------------------------------

    void MoveInCurrentDirection()
    {
        switch (ghostVisuals.GetDirection())
        {
            case Direction.Left:
                if (GameWorld.IsCellAccessibleToGhost(boardLeftPos))
                {
                    MoveLeft();
                }
                else if (GameWorld.IsCellAccessibleToGhost(boardUpPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveUp();
                }
                else if (boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveDown();
                }
            break;

            case Direction.Up:
                if (GameWorld.IsCellAccessibleToGhost(boardUpPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveUp();
                }
                else if (GameWorld.IsCellAccessibleToGhost(boardRightPos))
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            break;

            case Direction.Right:
                if (GameWorld.IsCellAccessibleToGhost(boardRightPos))
                {
                    MoveRight();
                }
                else if (GameWorld.IsCellAccessibleToGhost(boardDownPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveDown();
                }
                else if (boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveUp();
                }
            break;

            case Direction.Down:
                if (GameWorld.IsCellAccessibleToGhost(boardDownPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    MoveDown();
                }
                else if (GameWorld.IsCellAccessibleToGhost(boardLeftPos))
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            break;

            default:
            break;
        }
    }

    //-------------------------------------------------------------------------------------------

    void MoveLeft()
    {
        ghostVisuals.SetDirection(Direction.Left);

        //Specific check for off the side wrap around.
        if (boardLeftPos.x == -1 && boardLeftPos.y == 14)
        {
            transform.position = new Vector3(27, -GameWorld.GetWorldPosition(boardLeftPos).y, 0.0f);
        }
        else
        {
            //Move Left.
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, -GameWorld.GetWorldPosition(boardLeftPos).y, 0.0f);

        }
    }

    //-------------------------------------------------------------------------------------------

    void MoveUp()
    {
        ghostVisuals.SetDirection(Direction.Up);

        //Move Up.
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(GameWorld.GetWorldPosition(boardUpPos).x, transform.position.y, 0.0f);
    }
    
    //-------------------------------------------------------------------------------------------

    void MoveRight()
    {
        ghostVisuals.SetDirection(Direction.Right);

        //Specific check for off the side wrap around.
        if (boardRightPos.x == 29 && boardRightPos.y == 14)
        {
            transform.position = new Vector3(0, -GameWorld.GetWorldPosition(boardRightPos).y, 0.0f);
        }
        else
        {
            //Move Right.
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, -GameWorld.GetWorldPosition(boardRightPos).y, 0.0f);
        }
    }

    //-------------------------------------------------------------------------------------------

    void MoveDown()
    {
        ghostVisuals.SetDirection(Direction.Down);

        //Move Down.
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(GameWorld.GetWorldPosition(boardDownPos).x, transform.position.y, 0.0f);
    }

    //-------------------------------------------------------------------------------------------

    public void SetCurrentAIString(string currentState)
    {
        currentAINode = currentState;
    }

    //-------------------------------------------------------------------------------------------

    public void AddToCombinedAIString(string currentState)
    {
        currentAINode = currentState;
        combinedAINodes += currentState;
    }

    //-------------------------------------------------------------------------------------------

    public string GetCombinedAIString()
    {
        return combinedAINodes;
    }

    //-------------------------------------------------------------------------------------------

    public void ClearCombinedAIString()
    {
        combinedAINodes = "";
    }

    //-------------------------------------------------------------------------------------------

    public string GetCurrentAIString()
    {
        return currentAINode;
    }
    
    //-------------------------------------------------------------------------------------------
}
*/