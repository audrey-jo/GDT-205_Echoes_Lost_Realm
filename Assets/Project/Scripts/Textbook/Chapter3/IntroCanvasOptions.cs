using UnityEngine;

public class IntroCanvasOptions : MonoBehaviour
{
    [SerializeField] private GameObject     IntroCanvas  = null;
    [SerializeField] private Transform      BulletParent = null;
    [SerializeField] private Transform      ZombieParent = null;
    [SerializeField] private PlayerMovement player       = null;
    
    private GameData gameData = null;

    private void Start()
    {
        gameData = FindFirstObjectByType<GameData>();

        ResetGame();
    }

    public void StartGame()
    {
        if(gameData != null)
        {
            gameData.bGameActive = true;
        }

        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(false);
        }

        //Bring on the zombies.
        Spawner.MaxZombiesAllowed = 50;

        if(player != null)
        {
            player.ResetPlayer();
        }
    }

    public void ResetGame()
    {
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(true);
        }

        //Don't allow any zombies to spawn until we click the start game button.
        Spawner.iNumberOfZombies = 0;
        Spawner.MaxZombiesAllowed = 0;

        //Destroy all bullets.
        if(BulletParent != null)
        {
            foreach (Transform child in BulletParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        //Destroy all zombies.
        if (ZombieParent != null)
        {
            foreach (Transform child in ZombieParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
