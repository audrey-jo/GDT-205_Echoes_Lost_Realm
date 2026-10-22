using UnityEngine;

public class AriaCollisions : MonoBehaviour
{
    public SceneManager sceneManager;
    public GameObject spawnPoint;

    public void Start()
    {
        sceneManager = FindAnyObjectByType<SceneManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = spawnPoint.transform.position;
        }
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
