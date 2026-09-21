using UnityEngine;

public class CollisionRect : MonoBehaviour
{
    public Vector2 vPosition = Vector2.zero;
    public float width = 0.0f;
    public float height = 0.0f;
    public void Start()
    {
        //Ensure details of this rect are setup on game start.
        width = transform.localScale.x / 10.0f;
        height = transform.localScale.y / 10.0f;

        //vPosition is the top left corner.
        vPosition = new Vector2(transform.position.x - (width * 0.5f), transform.position.y - (height * 0.5f));
    }

    public bool IsInBounds(Vector2 pos)
    {
        return pos.x > vPosition.x && pos.x < (vPosition.x + width) &&
               pos.y > vPosition.y && pos.y < (vPosition.y + height);   

    }
}
