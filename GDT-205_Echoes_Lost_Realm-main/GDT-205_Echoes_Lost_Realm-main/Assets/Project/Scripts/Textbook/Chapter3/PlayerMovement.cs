/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eDirection
{
    Up,
    Down,
    Left,
    Right
};

public class PlayerMovement : MonoBehaviour
{
    public IntroCanvasOptions   menuCanvas                  = null;
    public ObstacleManager      obstacleManager             = null;
    public float                fPlayerSpeed                = 25.0f;
    public Vector2              vFireDirection              = new Vector2(1.0f, 0.0f);
    public bool                 bPlayerAlive                = false;
    public List<Transform>      collisionCheckTransforms    = new List<Transform>();
    public Transform            bloodTransform              = null;

    private GameData            gameData                    = null;
    private Vector3             vStartPosition;
    private float               fBloodSpreadSpeed           = 1.0f;

    private void Start()
    {
        gameData = FindFirstObjectByType<GameData>();
        bPlayerAlive = false;

        //If the blood transform is set up, shrink it so it cant be seen.
        if(bloodTransform != null)
        {
            bloodTransform.localScale = Vector3.zero;
        }

        vStartPosition = transform.position;
    }

    public void ResetPlayer()
    {
        transform.position = vStartPosition;
        bPlayerAlive = true;

        //If the blood transform is set up, shrink it so it cant be seen.
        if (bloodTransform != null)
        {
            bloodTransform.localScale = Vector3.zero;
        }
    }
    void Update()
    {
        //Early out if game is not active.
        if (gameData != null && !gameData.bGameActive)
        {
            return;
        }

        if (bPlayerAlive == true)
        {
            //Up & Down movement.
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!CheckForCollisions(eDirection.Up))
                {
                    vFireDirection.y = 1.0f;
                    if (Input.GetKey(KeyCode.A)) { vFireDirection.x = -1.0f; }
                    else if (Input.GetKey(KeyCode.D)) { vFireDirection.x = 1.0f; }
                    else { vFireDirection.x = 0.0f; }

                    transform.position += (Vector3)vFireDirection * fPlayerSpeed * Time.deltaTime;
                    return;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!CheckForCollisions(eDirection.Down))
                {
                    vFireDirection.y = -1.0f;
                    if (Input.GetKey(KeyCode.A)) { vFireDirection.x = -1.0f; }
                    else if (Input.GetKey(KeyCode.D)) { vFireDirection.x = 1.0f; }
                    else { vFireDirection.x = 0.0f; }

                    transform.position += (Vector3)vFireDirection * fPlayerSpeed * Time.deltaTime;
                    return;
                }
            }

            //Left & Right movement.
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!CheckForCollisions(eDirection.Left))
                {
                    vFireDirection.x = -1.0f;
                    vFireDirection.y = 0.0f;
                    transform.position += new Vector3(-fPlayerSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!CheckForCollisions(eDirection.Right))
                {
                    vFireDirection.x = 1.0f;
                    vFireDirection.y = 0.0f;
                    transform.position += new Vector3(fPlayerSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }
        }
        else
        {
            //Player is dead, so lets show the blood.
            if (bloodTransform != null)
            {
                bloodTransform.localScale += Vector3.one * fBloodSpreadSpeed * Time.deltaTime;
            }
        }
    }
    bool CheckForCollisions(eDirection dir)
    {
        if (obstacleManager != null && collisionCheckTransforms[(int)dir] != null)
        {
            for (int obstacleIndex = 0; obstacleIndex < obstacleManager.zCollidables.Count; obstacleIndex++)
            {
                CollisionRect obstacle = obstacleManager.zCollidables[obstacleIndex];
                if (obstacle != null)
                {
                    //If the collision transform check is in the bounds of an obstacle, return true. No need to check other obstacles.
                    if (obstacle.IsInBounds(collisionCheckTransforms[(int)dir].position))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the player is alive - We don't want repeated calls to kill the player.
        if (bPlayerAlive)
        {
            //If we hit a zombie - We die!
            if (other.gameObject.tag == "Zombie")
            {
                bPlayerAlive = false;
                StartCoroutine(EndGame());
            }
        }
    }
    
    IEnumerator EndGame()
    {
        //Lets wait 3 seconds before killing the game - This llows for the blood to flow.
        yield return new WaitForSeconds(3.0f);

        //Set game not active, so updates get stopped.
        if (gameData != null)
        {
            gameData.bGameActive = false;
        }
           
        //Show the menu canvas again.
        if (menuCanvas != null)
        {
            menuCanvas.ResetGame();
        }
    }
}
*/