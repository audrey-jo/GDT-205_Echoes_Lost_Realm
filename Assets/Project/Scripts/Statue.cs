using UnityEngine;

public class Statue : MonoBehaviour
{
    public Transform pair;

    void Start() { transform.position = new Vector2(-pair.position.x, -pair.position.y); }

    void Update() { transform.position = new Vector2(-pair.position.x, -pair.position.y); }
}
