using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
public class Rocket : MonoBehaviour
{
    public Camera camera = null;
    public Environment environment = null;
    public GameManager gameManager = null;

    private LineRenderer lineRenderer = null;
    private Rigidbody2D rigidBody = null;

    private float invincibilityTime = 0.2f;

    //--------------------------------------------------------------------------------------------------

    public void Fire(Vector2 fireDir, float firePower)
    {
        lineRenderer = GetComponent<LineRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.AddForce(fireDir * firePower);

        //We need to allow the rocket to get away from the ground when firing.
        invincibilityTime = 0.2f;
    }

    //--------------------------------------------------------------------------------------------------

    void Update()
    {
        IncorporateWind();

        invincibilityTime -= Time.deltaTime;
        if (invincibilityTime <= 0.0f)
        {
            if (DidCollideWithEnvironment())
            {
                //Tell the environment to make a crater.
                if(environment != null && gameManager != null)
                {
                    gameManager.CheckForPlayerExplosions(transform.position);
                    environment.CreateCrater(transform.position, rigidBody.linearVelocity);
                }

                Destroy(transform.gameObject);
            }
            else if(DidExitScreen())
            {
                //Just destroy the rocket.
                Destroy(transform.gameObject);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    void IncorporateWind()
    {
        if(environment != null)
        {
            if (environment.fWindStrength != 0.0)
            {
                Vector2 windDir = Vector2.zero;

                //Add the wind force.
                rigidBody.AddForce(Vector2.right * environment.fWindStrength);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    bool DidCollideWithEnvironment()
    {
        if (environment != null)
        {
            //Loop through each of the lines that make up the rocket and check them against the ground lines.
            for (int rocketLineIdx = 0; rocketLineIdx < lineRenderer.positionCount - 1; rocketLineIdx++)
            {
                Vector2 rocketPos1 = (Vector2)(transform.position + lineRenderer.GetPosition(rocketLineIdx));
                Vector2 rocketPos2 = (Vector2)(transform.position + lineRenderer.GetPosition(rocketLineIdx + 1));
                Vector2 vIntersection = Vector2.zero;

                //Loop through the lines that make up the lines that make up the environment and see if we are intersecting any of them.
                for (int groundLineIdx = 0; groundLineIdx < environment.environmentLines.positionCount - 1; groundLineIdx++)
                {
                    Vector2 environmentLinePos1 = environment.environmentLines.GetPosition(groundLineIdx);
                    Vector2 environmentLinePos2 = environment.environmentLines.GetPosition(groundLineIdx + 1);

                    //Did we intersect?
                    if (VectorMath.LineToLineIntersection(rocketPos1, rocketPos2, environmentLinePos1, environmentLinePos2, ref vIntersection))
                    {
                        //If so, exit this loop.
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //--------------------------------------------------------------------------------------------------

    bool DidExitScreen()
    {
        if (camera != null)
        {
            Vector3 rocketScreenPos = camera.WorldToScreenPoint(transform.position);
            if(rocketScreenPos.x < 0.0f || rocketScreenPos.x > Screen.width || rocketScreenPos.y < 0.0f)
            {
                //Note - We don't mind the rocket going off the top of the screen.
                return true;
            }
        }

        return false;
    }

    //--------------------------------------------------------------------------------------------------
}
