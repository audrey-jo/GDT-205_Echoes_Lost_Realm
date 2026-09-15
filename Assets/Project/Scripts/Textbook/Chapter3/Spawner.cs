using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static int iNumberOfZombies = 0;
    public static int MaxZombiesAllowed = 50;

    public Transform        zombieParent            = null;
    public GameObject       prefabToSpawn           = null;
    public float            fDelayBetweenSpawns     = 5.0f;
    public ObstacleManager  obstacleManager         = null;
    public Transform        transformOfInterest     = null;

    private float           fAccumulatedDeltaTime   = 0.0f;

    //--------------------------------------------------------

    private void Start()
    {
        //Set timer at the time to spawn, so we get an instant spawn.
        fAccumulatedDeltaTime = fDelayBetweenSpawns;
    }

    //--------------------------------------------------------

    void Update()
    {
        //Ensure we have all the settings required to spawn our objects.
        if (prefabToSpawn != null && transformOfInterest != null && obstacleManager != null && zombieParent != null)
        {
            if(MaxZombiesAllowed != 0 && iNumberOfZombies < MaxZombiesAllowed)
            { 
                fAccumulatedDeltaTime += Time.deltaTime;

                //Is it time to spawn another?
                if (fAccumulatedDeltaTime > fDelayBetweenSpawns)
                {
                    //Keep track of how many zombies are in the scene.
                    Spawner.iNumberOfZombies++;

                    GameObject newGO = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, zombieParent);
                    SteeringBehaviours zombieSteering = newGO.GetComponent<SteeringBehaviours>();

                    if (zombieSteering != null)
                    {
                        //Set the transform to chase - The player for a zombie spawner.
                        zombieSteering.targetTransform = transformOfInterest;

                        //Set the obstacle manager.
                        zombieSteering.obstacleManager = obstacleManager;

                        //Turn on the steering behaviours we need.
                        zombieSteering.bEnabled_Seek = true;
                        zombieSteering.bEnabled_Wander = true;
                        zombieSteering.bEnabled_Avoidance = true;
                        zombieSteering.bEnabled_Flocking = true;
                    }

                    //Reset the timer.
                    fAccumulatedDeltaTime = 0.0f;
                }
            }
        }
    }

    //--------------------------------------------------------
}
