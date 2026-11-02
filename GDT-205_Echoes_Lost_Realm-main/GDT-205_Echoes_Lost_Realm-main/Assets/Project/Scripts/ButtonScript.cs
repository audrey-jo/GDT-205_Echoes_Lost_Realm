using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject portal;
    void Start()
    {
        portal.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            portal.SetActive(true);
        }
    }
}
