using UnityEngine;

public class AriaCollisions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Aria collided with an enemy!");
            // Handle collision with enemy
        }

        
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portal"))
        {
            Debug.Log("Aria collided with the portal!");
            // Handle collision with portal
        }
    }
}
