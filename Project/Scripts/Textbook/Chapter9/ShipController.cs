using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody = null;
    [SerializeField] private ParticleSystem particles = null;
    [SerializeField] private float fThrustPower = 10.0f;
    [SerializeField] private float fRotationAmount = 45.0f;

    [SerializeField] private LineRenderer environmentLines = null;
    [SerializeField] private LineRenderer finishLine = null;
    [SerializeField] private LineRenderer shipLine = null;

    private Vector3 vStartPosition;
    public void SetStartPosition(Vector3 pos) { vStartPosition = pos; }

    private bool bAlive = true;
    public bool IsAlive() { return bAlive; }

    private bool bSuccess = false;
    public bool WasSuccessful() { return bSuccess; }

    private bool bIsAI = true;
    public void SetIsAI(bool isAi) { bIsAI = isAi; }

    //--------------------------------------------------------------------------------------

    public void ResetShip()
    {
        transform.gameObject.SetActive(true);
        transform.position = vStartPosition;

        bSuccess = false;
        bAlive = true;
    }

    //--------------------------------------------------------------------------------------

    void Update()
    {
        //Did we crash?
        if(CheckForCrash() == true)
        {
            bAlive = false;
            transform.gameObject.SetActive(false);
            return;
        }

        //Did we reach the finish line?
        if(CheckForSuccess() == true)
        {
            bSuccess = true;
            bAlive = false;
            transform.gameObject.SetActive(false);
            return;
        }

        //Only allow control on User controlled ships.
        if (bIsAI == false)
        {
            //Key press to simulate what the GA will do.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Thrust();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateRight();
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void Thrust()
    {
        //Add a force in the up direction of the ship.
        rigidBody.AddForce(transform.up * fThrustPower);

        //If we have particles, trigger them.
        if(particles != null)
        {
            particles.startDelay = 0.0f;
            particles.Play();
        }
    }

    //--------------------------------------------------------------------------------------

    public void RotateRight()
    {
        //Rotate.
        rigidBody.rotation -= fRotationAmount;
    }

    //--------------------------------------------------------------------------------------

    public void RotateLeft()
    {
        //Rotate.
        rigidBody.rotation += fRotationAmount;
    }

    //--------------------------------------------------------------------------------------

    bool CheckForCrash()
    {
        float fShipRotationInRadians = rigidBody.rotation * Mathf.Deg2Rad;

        //Debug.Log((Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(0), fShipRotationInRadians));
        //Debug.Log((Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(1), fShipRotationInRadians));

        Vector2 vIntersection = Vector2.zero;

        //Environment lines.
        if (environmentLines != null)
        {
            //Loop through each of the lines that make up the ship and check them against the ground lines.
            for (int shipLineIdx = 0; shipLineIdx < shipLine.positionCount - 1; shipLineIdx++)
            {
                //Ship positions are from an upright orientation. We need to rotate the local positions ourselves, then add to the world position.
                Vector2 shipPos1 = (Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(shipLineIdx), fShipRotationInRadians);
                Vector2 shipPos2 = (Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(shipLineIdx + 1), fShipRotationInRadians);

                //Loop through the lines that make up the ground and see if we are intersecting any of them.
                for (int groundLineIdx = 0; groundLineIdx < environmentLines.positionCount - 1; groundLineIdx++)
                {
                    //Ground lines don't move, so no rotation needed.
                    Vector2 groundPos1 = environmentLines.GetPosition(groundLineIdx);
                    Vector2 groundPos2 = environmentLines.GetPosition(groundLineIdx + 1);

                    //Did we intersect?
                    if(VectorMath.LineToLineIntersection(shipPos1, shipPos2, groundPos1, groundPos2, ref vIntersection))
                    {
                        //If so, return true to signify a crash.
                        return true;
                    }
                }
            }
        }

        //No intersecting lines, so no crash.
        return false;
    }

    //--------------------------------------------------------------------------------------

    bool CheckForSuccess()
    {
        float fShipRotationInRadians = rigidBody.rotation * Mathf.Deg2Rad;
        Vector2 vIntersection = Vector2.zero;

        //Winning line.
        if (finishLine != null)
        {
            //Loop through each of the lines that make up the ship and check them against the ground lines.
            for (int shipLineIdx = 0; shipLineIdx < shipLine.positionCount - 1; shipLineIdx++)
            {
                //Ship positions are from an upright orientation. We need to rotate the local positions ourselves, then add to the world position.
                Vector2 shipPos1 = (Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(shipLineIdx), fShipRotationInRadians);
                Vector2 shipPos2 = (Vector2)transform.position + VectorMath.RotateAroundAPoint(Vector2.zero, shipLine.GetPosition(shipLineIdx + 1), fShipRotationInRadians);

                //Loop through the lines that make up the finish line and see if we are intersecting any of them.
                for (int groundLineIdx = 0; groundLineIdx < finishLine.positionCount - 1; groundLineIdx++)
                {
                    //Winning line doesn't move, so no rotation needed.
                    Vector2 finishLinePos1 = finishLine.GetPosition(groundLineIdx);
                    Vector2 finishLinePos2 = finishLine.GetPosition(groundLineIdx + 1);

                    //Did we intersect?
                    if (VectorMath.LineToLineIntersection(shipPos1, shipPos2, finishLinePos1, finishLinePos2, ref vIntersection))
                    {
                        //If so, return true to signify an overlap with the finish line
                        return true;
                    }
                }
            }
        }

        //No intersecting lines, so not there yet.
        return false;
    }

    //--------------------------------------------------------------------------------------
}
