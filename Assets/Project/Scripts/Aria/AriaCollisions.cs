using UnityEngine;
using UnityEngine.InputSystem;

public class AriaCollisions : MonoBehaviour
{
    public SceneManager sceneManager;
    public GameObject spawnPoint;

    public bool IsInvincible { get; private set; }
    public static bool InstantKillProjectilesEnabled { get; private set; }

    public void Start()
    {
        sceneManager = FindAnyObjectByType<SceneManager>();
        InstantKillProjectilesEnabled = false;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.iKey.wasPressedThisFrame)
        {
            IsInvincible = !IsInvincible;
            Debug.Log($"Aria invincibility: {(IsInvincible ? "ON" : "OFF")}.", this);
        }

        if (keyboard.kKey.wasPressedThisFrame)
        {
            InstantKillProjectilesEnabled = !InstantKillProjectilesEnabled;
            Debug.Log($"Instant-kill projectiles: {(InstantKillProjectilesEnabled ? "ON" : "OFF")}.", this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            TakeProjectileHit();
    }

    public void TakeProjectileHit()
    {
        if (IsInvincible || spawnPoint == null) return;
        transform.position = spawnPoint.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ToForest"))
        {
            sceneManager.LoadScene("Forest");
        }

        if (collision.gameObject.CompareTag("ToRuins"))
        {
            sceneManager.LoadScene("Ruins");
        }

        if (collision.gameObject.CompareTag("ToTemple"))
        {
            sceneManager.LoadScene("Temple");
        }

        if (collision.gameObject.CompareTag("ToInnerTemple"))
        {
            sceneManager.LoadScene("InnerTemple");
        }

        if (collision.gameObject.CompareTag("ToNext"))
        {
            Debug.Log("To next area (portal not set up yet)");
        }
    }
}
