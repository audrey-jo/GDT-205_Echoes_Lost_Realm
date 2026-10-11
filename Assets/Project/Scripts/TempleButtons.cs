using UnityEngine;

public class TempleButtons : MonoBehaviour
{
    public GameObject door;
    void Start()
    {
        door.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            door.SetActive(false);
        }
    }
}
