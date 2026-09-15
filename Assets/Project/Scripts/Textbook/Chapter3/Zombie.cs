using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviours))]
public class Zombie : BaseEntity
{
    private SteeringBehaviours steering = null;

    //--------------------------------------------------------------------------------------
    private void Start()
    {
        steering = GetComponent<SteeringBehaviours>();
    }

    //--------------------------------------------------------------------------------------

    void Update()
    {
        Vector2 force = steering.GetCombinedForce();

        //Acceleration = Force/Mass
        Vector2 acceleration = force / fMass;

        //Update velocity.
        vCurrentVelocity += acceleration * Time.deltaTime;

        //Don't allow the zombie to go faster than maximum speed.
        vCurrentVelocity = Vector2.ClampMagnitude(vCurrentVelocity, fMaxMoveSpeed);

        //Move the zombie using the new velocity.
        gameObject.transform.position += (Vector3)vCurrentVelocity * Time.deltaTime;

        //Turn to face the direction we are moving in.
        UpdateFacingDirection();

        //Debug output to see what is going on with vectors.
        Debug.DrawRay(transform.position, vFacing*10.0f, Color.red, 0.0f, true);
        Debug.DrawRay(transform.position, vRight*5.0f, Color.blue, 0.0f, true);
    }

    //--------------------------------------------------------------------------------------
}
