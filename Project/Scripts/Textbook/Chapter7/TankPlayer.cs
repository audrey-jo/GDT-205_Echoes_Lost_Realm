using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankPlayer : MonoBehaviour
{
    protected const float kMaxPower = 4000.0f;

    public Camera camera = null;
    public Environment environment = null;
    public GameManager gameManager = null;
    public Transform rocketParent = null;
    [SerializeField] protected GameObject rocketPrefab = null;
    [SerializeField] protected GameObject cannon = null;

    protected bool bIsAlive = true;
    public bool IsAlive()                   { return bIsAlive; }
    public void SetAlive(bool isAlive)      { bIsAlive = isAlive; }

    protected Vector2 vFireDirection = Vector2.zero;
    protected float fFirePower = 0.0f;

    [SerializeField] protected Slider powerBar = null;

    //--------------------------------------------------------------------------------------------------

    public void SetPowerBarPositioning()
    {
        Vector3 tankScreenPos = camera.WorldToScreenPoint(transform.position);
        tankScreenPos.y += 50.0f;
        powerBar.transform.position = tankScreenPos;
    }

    //--------------------------------------------------------------------------------------------------

    public virtual bool Aim()
    {
        if (camera != null)
        {
            if (Input.GetMouseButton(0))
            {
                //Calculate a vector from centre of tank to the mouse position.
                Vector3 tankScreenPos = camera.WorldToScreenPoint(transform.position);
                Vector3 toMousePos = Input.mousePosition - tankScreenPos;

                //Normalize it.
                Vector2 unitVectorToMousePos = VectorMath.Normalize(toMousePos);
                vFireDirection = unitVectorToMousePos;

                //Then find the dot product from the up vector (0,1).
                float fDot = VectorMath.Dot(Vector2.up, unitVectorToMousePos);

                //Get the Radians.
                float fRadians = Mathf.Acos(fDot);

                //Get the degrees.
                float degrees = fRadians * Mathf.Rad2Deg;

                //-----------------------------------------------------------------------------------------
                //We need to know whether this is a right or left rotation.
                float fDotRight = VectorMath.Dot(Vector2.right, unitVectorToMousePos);

                //Default to a left rotation.
                int dir = 1;

                //If the dot product is greater than 0.0, then it indicates a rotation to the right.
                if (fDotRight > 0.0f)
                {
                    dir = -1;
                }
                RotateCannon(degrees, dir);
            }
        }

        //return true when complete.
        if (Input.GetMouseButtonUp(0))
        {
            fFirePower = 0.0f;

            //Empty the power bar and show it.
            if (powerBar != null)
            {
                powerBar.value = 0.0f;
                powerBar.transform.gameObject.SetActive(true);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    //--------------------------------------------------------------------------------------------------

    protected void RotateCannon(float degrees, int dir)
    {
        if (cannon != null)
        {
            //Rotate the arrow to face this direction.
            Vector3 euler = new Vector3(0.0f, 0.0f, degrees * dir);

            Quaternion qNewRotation = Quaternion.identity;
            qNewRotation.eulerAngles = euler;
            cannon.transform.rotation = qNewRotation;
        }
    }

    //--------------------------------------------------------------------------------------------------

    public virtual bool Fire()
    {
        if (powerBar != null)
        {
            //Power of shot increases whilst right mouse button held.
            if(Input.GetMouseButton(1))
            {
                fFirePower += 50.0f;
                fFirePower = Mathf.Min(kMaxPower, fFirePower);

                //Fill the power bar the required amount.
                powerBar.value = fFirePower / kMaxPower;
            }

            //Release right mouse button to fire.
            else if(Input.GetMouseButtonUp(1))
            {
                FireRocket();

                return true;
            }
        }

        //return true when complete.
        return false;
    }

    //--------------------------------------------------------------------------------------------------

    protected void FireRocket()
    {
        if (rocketPrefab != null && rocketParent != null && powerBar != null && gameManager != null)
        {
            GameObject rocketGO = Instantiate(rocketPrefab, transform.position, Quaternion.identity, rocketParent);
            if (rocketGO != null)
            {
                Rocket rocket = rocketGO.GetComponent<Rocket>();
                if (rocket != null)
                {
                    rocket.camera = camera;
                    rocket.environment = environment;
                    rocket.gameManager = gameManager;
                    rocket.Fire(vFireDirection, fFirePower);
                }
            }

            //Remove power bar.
            powerBar.transform.gameObject.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------------------------

    public void KillPlayer()
    {
        bIsAlive = false;
        gameObject.SetActive(false);
    }

    //--------------------------------------------------------------------------------------------------
}
