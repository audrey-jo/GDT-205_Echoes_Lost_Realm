using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    //Physics details.
    public float fMaxMoveSpeed = 500.0f;
    public float GetMaxMoveSpeed()                                  { return fMaxMoveSpeed; }
    protected float fMass = 1.0f;

    //Vector details used to move entity.
    protected Vector2 vCurrentVelocity = Vector2.zero;
    public Vector2 GetCurrentVelocity()                             { return vCurrentVelocity; }
    protected Vector2 vFacing = new Vector2(1.0f, 0.0f);
    public Vector2 GetFacing()                                      { return vFacing; }
    protected Vector2 vRight = new Vector2(0.0f, 1.0f);
    public Vector2 GetRight()                                       { return vRight; }

    //Details for rotation.
    protected float fCurrentDegree = 0.0f;
    protected Quaternion qNewRotation = Quaternion.identity;


    //--------------------------------------------------------------------------------------

    private void Start()
    {
        vCurrentVelocity = Vector2.zero;
    }

    //--------------------------------------------------------------------------------------

    void Update()
    {
    }

    //--------------------------------------------------------------------------------------
    protected void UpdateFacingDirection()
    {
        //The velocity must be greater than zero, otherwise we do not want to run this code.
        if (VectorMath.Magnitude(vCurrentVelocity) > 0.0f)
        {
            float fDot = VectorMath.Dot(vFacing, vCurrentVelocity);

            //Just to err on the side of caution - Ensure we are bound between -1 and 1.
            fDot = Mathf.Min(1.0f, fDot);
            fDot = Mathf.Max(-1.0f, fDot);

            float fRadian = Mathf.Acos(fDot);
            float fDegree = Mathf.Rad2Deg * fRadian;

            //Flip the angle of rotation depending on with side of the object we are turning.
            if (VectorMath.Dot(vRight, vCurrentVelocity) < 0.0f)
            {
                fDegree *= -1.0f;
            }

            fCurrentDegree += fDegree;

            Vector3 euler = new Vector3(0.0f, 0.0f, fCurrentDegree);

            qNewRotation.eulerAngles = euler;
            transform.rotation = qNewRotation;

            vFacing = VectorMath.Normalize(vCurrentVelocity);
            vRight = VectorMath.Perpendicular(vFacing);
        }
    }

    //--------------------------------------------------------------------------------------
}
