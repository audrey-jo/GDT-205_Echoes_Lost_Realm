using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public List<CollisionRect> zCollidables = new List<CollisionRect>();
    private void Start()
    {
        //Ensure we have all the collidable objects setup.
        CollisionRect[] collisionRects = GetComponentsInChildren<CollisionRect>();
        for (int i = 0; i < collisionRects.Length; i++)
        {
            zCollidables.Add(collisionRects[i]);
        }
    }
}
