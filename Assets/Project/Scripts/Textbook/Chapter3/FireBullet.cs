using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public static int iNumberOfBullets = 0;
    public static int MaxBulletsAllowed = 100;

    public Transform bulletParent = null;
    public GameObject prefabToSpawn = null;
    private float fDelayBetweenBullets = 0.25f;
    private float fAccumulatedDeltaTime = 0.0f;

    public PlayerMovement playerMovement = null;

    //--------------------------------------------------------------------------------------

    void Update()
    {
        fAccumulatedDeltaTime += Time.deltaTime;

        if (prefabToSpawn != null && playerMovement != null && bulletParent != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (iNumberOfBullets < MaxBulletsAllowed)
                {
                    //Is it time to spawn another?
                    if (fAccumulatedDeltaTime > fDelayBetweenBullets)
                    {
                        //Ensure we have a direction from the player.
                        if (VectorMath.Magnitude(playerMovement.vFireDirection) != 0.0f)
                        {
                            //Keep track of how many bullets are in the scene.
                            iNumberOfBullets++;

                            GameObject newGO = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, bulletParent);
                            Bullet newBullet = newGO.GetComponent<Bullet>();

                            if (newBullet != null)
                            {
                                //Set the bullet's movement diretion to be the normalized direction the player is moving.
                                newBullet.SetMovementDirection(VectorMath.Normalize(playerMovement.vFireDirection));
                            }
                        }

                        //Reset the timer.
                        fAccumulatedDeltaTime = 0.0f;
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------
}
