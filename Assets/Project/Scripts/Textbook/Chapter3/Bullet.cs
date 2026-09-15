using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 vMovementDirection;
    public float fBulletMovementSpeed = 50.0f;

    //--------------------------------------------------------------------------------------

    public void SetMovementDirection(Vector2 dir)
    {
        vMovementDirection = dir;
    }

    //--------------------------------------------------------------------------------------

    void Update()
    {
        transform.position += (Vector3)vMovementDirection * fBulletMovementSpeed * Time.deltaTime;
    }

    //--------------------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        //Deduct 1 form the bullet count, as we are about to kill it.
        FireBullet.iNumberOfBullets--;

        //If we hit a zombie - Kill it.
        if(other.gameObject.tag == "Zombie")
        {
            //Deduct 1 from the zombie spawner count.
            Spawner.iNumberOfZombies--;

            //Destroy the zombie.
            Destroy(other.gameObject);
        }

        //Destroy the bullet.
        Destroy(this.gameObject);
    }

    //--------------------------------------------------------------------------------------
}
