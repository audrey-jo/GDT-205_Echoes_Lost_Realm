using UnityEngine;

public class AriaCollisions : MonoBehaviour
{
    public SceneManager sceneManager;

    public void Start()
    {
        sceneManager = FindAnyObjectByType<SceneManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            sceneManager.LoadScene("MenuScene");
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ForestPortal"))
        {
            sceneManager.LoadScene("ForestScene");
        }

        if (collision.gameObject.CompareTag("RuinsPortal"))
        {
            sceneManager.LoadScene("RuinsScene");
        }

        if (collision.gameObject.CompareTag("TemplePortal"))
        {
            sceneManager.LoadScene("TempleScene");
        }
    }
}
