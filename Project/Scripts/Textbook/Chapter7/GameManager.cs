using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera camera = null;
    [SerializeField] private Environment environment = null;
    [SerializeField] Transform tankParent = null;
    [SerializeField] Transform rocketParent = null;
    [SerializeField] Text winnnerText = null;

    public float fTankRadius = 5.0f;
    public int playerTurn = 0;
    public PlayState currentState = PlayState.ChangeWind;

    public List<GameObject> playerTurnIndicators = new List<GameObject>();
    public List<TankPlayer> players = new List<TankPlayer>();

    private bool bSetup = false;
    private bool bGameAlive = false;

    //--------------------------------------------------------------------------------------------------

    private void SetUp()
    {
        if (environment != null && environment.IsSetup())
        {
            //Always starts with player 1's turn.
            playerTurn = 0;

            //Set the wind.
            currentState = PlayState.ChangeWind;

            if (playerTurnIndicators[playerTurn] != null)
            {
                playerTurnIndicators[playerTurn].SetActive(true);
            }

            if (environment != null && tankParent != null && camera != null)
            {
                TankPlayer player1Tank = tankParent.GetChild(0).GetComponent<TankPlayer>();
                if (player1Tank != null)
                {
                    player1Tank.rocketParent = rocketParent;
                    player1Tank.environment = environment;
                    player1Tank.camera = camera;
                    player1Tank.gameManager = this;
                    player1Tank.SetPowerBarPositioning();
                    players.Add(player1Tank);
                }

                TankPlayer player2Tank = tankParent.GetChild(1).GetComponent<TankPlayer>();
                if (player2Tank != null)
                {
                    player2Tank.rocketParent = rocketParent;
                    player2Tank.environment = environment;
                    player2Tank.camera = camera;
                    player2Tank.gameManager = this;
                    player2Tank.SetPowerBarPositioning();
                    players.Add(player2Tank);
                }

                if(winnnerText != null)
                {
                    winnnerText.gameObject.SetActive(false);
                }

                bSetup = true;
                bGameAlive = true;
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (!bSetup)
        {
            //Ensure we are setup before we try and dow anything.
            SetUp();
        }
        else if (bGameAlive == true)
        {
            //Game flow.
            switch (currentState)
            {
                case PlayState.ChangeWind:
                    ChangeWind();
                    break;

                case PlayState.Aim:
                    PlayerAim();
                    break;

                case PlayState.Fire:
                    PlayerFire();
                    break;

                case PlayState.ModifyLand:
                    ModifyLand();
                    break;

                case PlayState.CheckForSurvivors:
                    CheckForSurvivors();
                    break;
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    private void ChangeTurn()
    {
        //Turn off indicator for last player.
        if (playerTurnIndicators[playerTurn] != null)
        {
            playerTurnIndicators[playerTurn].SetActive(false);
        }

        //Switch player.
        playerTurn = playerTurn == 0 ? 1 : 0;

        //Turn on indicator for current player.
        if (playerTurnIndicators[playerTurn] != null)
        {
            playerTurnIndicators[playerTurn].SetActive(true);
        }

        //Debug.Log("Player " + playerTurn);

        //always start a turn by changing the wind.
        currentState = PlayState.ChangeWind;
        //Debug.Log("ChangeWind");
    }

    //--------------------------------------------------------------------------------------------------
    // Play states:

    void ChangeWind()
    {
        if (environment != null)
        {
            environment.GenerateWind();

            //Move on to next state of play.
            currentState = PlayState.Aim;
            //Debug.Log("PlayerAim");
        }
    }


    //--------------------------------------------------------------------------------------------------

    void PlayerAim()
    {
        if (players[playerTurn] != null)
        {
            if (players[playerTurn].Aim())
            {
                //Move on to next state of play.
                currentState = PlayState.Fire;
                //Debug.Log("PlayerFire");
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    
    void PlayerFire()
    {
        if (players[playerTurn] != null)
        {
            if (players[playerTurn].Fire())
            {
                //Move on to next state of play.
                currentState = PlayState.ModifyLand;
                //Debug.Log("ModifyLand");
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    void ModifyLand()
    {
        if (rocketParent != null)
        {
            if (rocketParent.childCount == 0)
            {
                //Move on to next state of play.
                currentState = PlayState.CheckForSurvivors;
                //Debug.Log("CheckForSurvivors");
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    void CheckForSurvivors()
    {
        //Check how many players are still alive.
        int numberOfPlayersAlive = 0;
        for (int tankIndex = 0; tankIndex < players.Count; tankIndex++)
        {
            if(players[tankIndex].IsAlive())
            {
                numberOfPlayersAlive++;
            }
        }

        if (numberOfPlayersAlive == 2)
        {
            //Move on to next player's turn.
            ChangeTurn();
        }
        else
        {
            EndGame();
        }
    }

    //---------------------------------------------------------------------------

    public void CheckForPlayerExplosions(Vector2 explosionPos)
    {
        //Check if an explosion happened within range of a tank, and kill it.
        for(int tankIndex = 0; tankIndex < players.Count; tankIndex++)
        {
            Vector2 vExplosionToTank = (Vector2)players[tankIndex].transform.position - explosionPos;
            if(VectorMath.Magnitude(vExplosionToTank) < fTankRadius)
            {
                players[tankIndex].KillPlayer();
            }
        }
    }

    //---------------------------------------------------------------------------

    void EndGame()
    {
        //Don't alow further updates.
        bGameAlive = false;

        //Find which player is dead.
        int deadPlayerIndex = 0;
        for (int tankIndex = 0; tankIndex < players.Count; tankIndex++)
        {
            if(!players[tankIndex].IsAlive())
            {
                deadPlayerIndex = tankIndex;
                break;
            }
        }

        //Display correct win text.
        if(winnnerText != null)
        {
            if (deadPlayerIndex == 0)
            {
                winnnerText.text = "Blue Wins";
                winnnerText.color = Color.blue;
            }
            else
            {
                winnnerText.text = "Red Wins";
                winnnerText.color = Color.red;
            }

            winnnerText.gameObject.SetActive(true);
        }
    }

    //--------------------------------------------------------------------------------------------------

    public void Quit()
    {
        SceneManager.LoadScene("Chapter7_FuzzyLogic");
    }

    //--------------------------------------------------------------------------------------------------
}
