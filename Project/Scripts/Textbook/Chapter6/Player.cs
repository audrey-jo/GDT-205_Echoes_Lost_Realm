using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [SerializeField] private MenuOptions menuOptions         = null;

    private Animator                     animator            = null;

    public float                         moveSpeed           = 1.0f;

    private Vector2Int                   boardPos;
    private Vector2Int                   boardDownPos;
    private Vector2Int                   boardLeftPos;
    private Vector2Int                   boardRightPos;
    private Vector2Int                   boardUpPos;

    private Direction                    currentDirection    = Direction.Right;

    //-------------------------------------------------------------------------------------

    void Start()
    {
        animator    = GetComponent<Animator>();
        animator.SetBool("Alive", true);
    }

    //-------------------------------------------------------------------------------------

    void Update()
    {
        if (GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            Vector3 offsetPosition = transform.position;
            offsetPosition -= new Vector3(0.05f, 0.25f, 0.0f);

            //Get player's position within the grid.
            boardPos = GameWorld.GetBoardPosition(offsetPosition);

            boardLeftPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.left * 0.75f);
            boardRightPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.right * 0.75f);
            boardUpPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.up * 0.5f);
            boardDownPos = GameWorld.GetBoardPosition(offsetPosition + Vector3.down * 1.0f);

            //Get player input.
            HandleInput();

            //Now move in this direction if possible.
            Move();

            //All pills collected - Winner winner chicken dinner.
            if (Pill.pillCount <= 0)
            {
                menuOptions.EndGame(true);
            }
        }
    }

    //-------------------------------------------------------------------------------------

    public Direction GetDirection()
    {
        return currentDirection;
    }

    //-------------------------------------------------------------------------------------

    public Vector2Int GetBoardPosition()
    {
        return boardPos;
    }

    //-------------------------------------------------------------------------------------

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && GameWorld.IsCellAccessible(boardLeftPos))
        {
            //Animate Left.
            animator.SetFloat("MoveX", -1.0f);
            animator.SetFloat("MoveY", 0.0f);

            //Set which way we are heading for later use.
            currentDirection = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && GameWorld.IsCellAccessible(boardRightPos))
        {
            //Animate Right.
            animator.SetFloat("MoveX", 1.0f);
            animator.SetFloat("MoveY", 0.0f);

            //Set which way we are heading for later use.
            currentDirection = Direction.Right;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && GameWorld.IsCellAccessible(boardUpPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
        {
            //Animate Up.
            animator.SetFloat("MoveY", 1.0f);
            animator.SetFloat("MoveX", 0.0f);

            //Set which way we are heading for later use.
            currentDirection = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && GameWorld.IsCellAccessible(boardDownPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
        {
            //Animate Down.
            animator.SetFloat("MoveY", -1.0f);
            animator.SetFloat("MoveX", 0.0f);

            //Set which way we are heading for later use.
            currentDirection = Direction.Down;
        }
    }

    //-------------------------------------------------------------------------------------

    void Move()
    {
        //Only move if alive.
        if (animator.GetBool("Alive"))
        {
            if (animator.GetFloat("MoveX") == -1.0f)
            {
                //Specific check for off the side wrap around.
                if (boardLeftPos.x == -1 && boardLeftPos.y == 14)
                {
                    transform.position = new Vector3(27, -GameWorld.GetWorldPosition(boardLeftPos).y, 0.0f);
                }
                else if (GameWorld.IsCellAccessible(boardLeftPos))
                {
                    //Move Left.
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, -GameWorld.GetWorldPosition(boardLeftPos).y, 0.0f);

                }
            }
            else if (animator.GetFloat("MoveX") == 1.0f)
            {
                //Specific check for off the side wrap around.
                if (boardRightPos.x == 29 && boardRightPos.y == 14)
                {
                    transform.position = new Vector3(0, -GameWorld.GetWorldPosition(boardRightPos).y, 0.0f);
                }
                else if (GameWorld.IsCellAccessible(boardRightPos))
                {
                    //Move Right.
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, -GameWorld.GetWorldPosition(boardRightPos).y, 0.0f);
                }
            }
            else if (animator.GetFloat("MoveY") == 1.0f)
            {
                //Move Up.
                if (GameWorld.IsCellAccessible(boardUpPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                    transform.position = new Vector3(GameWorld.GetWorldPosition(boardUpPos).x, transform.position.y, 0.0f);
                }
            }
            else if (animator.GetFloat("MoveY") == -1.0f)
            {
                //Move Down.
                if (GameWorld.IsCellAccessible(boardDownPos) && boardPos.x > 0 && boardPos.x < GameWorld.BoardColumns)
                {
                    transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                    transform.position = new Vector3(GameWorld.GetWorldPosition(boardDownPos).x, transform.position.y, 0.0f);
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------

    public void SetEaten()
    {
        if (animator.GetBool("Alive") == true)
        {
            animator.SetBool("Alive", false);

            StartCoroutine(EndAfterDelay(2.0f));
        }
    }

    //-------------------------------------------------------------------------------------------

    IEnumerator EndAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        menuOptions.EndGame(false);
    }

    //-------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            //If colliding with the player, destroy self.
            if (other.gameObject.CompareTag("Pill"))
            {
                Pill pill = other.gameObject.GetComponent<Pill>();
                if (pill != null)
                {
                    pill.SetEaten();
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------
}
