using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerAi : TankPlayer
{
    const float kMinPowerRange = (kMaxPower / 4);
    const float kLowPowerRange = (kMaxPower / 2);
    const float kMediumPowerRange = kMaxPower - (kMaxPower / 4);
    const float kStrongPowerRange = kMaxPower;

    const float kLowAngleRange = 90.0f;
    const float kMediumAngleRange = 60.0f;
    const float kHighAngleRange = 30.0f;

    public float fTurretRotationSpeed = 20.0f;

    bool bChosenAim = false;
    float fDesiredAngle = 0.0f;
    bool bChosenPower = false;
    float fDesiredPower = 0.0f;

    float fCurrentAngle = 0.0f;

    //--------------------------------------------------------------------------------------------------

    private void Reset()
    {
        bChosenAim = false;
        bChosenPower = false;
        fCurrentAngle = 0.0f;
        fDesiredPower = 0.0f;
        fFirePower = 0.0f;

        powerBar.transform.gameObject.SetActive(false);
    }

    //--------------------------------------------------------------------------------------------------

    public override bool Aim()
    {
        if (!bChosenAim)
        {
            //Set turret to face up, so we can rotate it into position.
            RotateCannon(0.0f, 1);

            //Get the desired power for the shot.
            SetAim();
        }
        else
        {
            if (fCurrentAngle < fDesiredAngle)
            {
                fCurrentAngle += fTurretRotationSpeed * Time.deltaTime;

                RotateCannon(fCurrentAngle, 1);
            }
            else
            {
                //Get the vector in current direction.
                float radian = Mathf.Deg2Rad * fCurrentAngle;

                Vector2 vEndOfTurretPos = (Vector2)transform.position + Vector2.up;
                vFireDirection = VectorMath.RotateAroundAPoint((Vector2)transform.position, vEndOfTurretPos, radian);
                vFireDirection = VectorMath.Normalize(vFireDirection);

                powerBar.transform.gameObject.SetActive(true);

                //We are facing the correct direction.
                return true;
            }
        }

        //return true when at correct angle.
        return false;
    }

    //--------------------------------------------------------------------------------------------------

    void SetAim()
    {
        Vector3 vecToOpp = Vector3.zero;

        //----------------------------------------------------------------------------------------------
        //Get the other tank - Both should be children of the same parent.
        Transform parent = transform.parent;
        for (int iTankIdx = 0; iTankIdx < parent.childCount; iTankIdx++)
        {
            //If the current tank is not us, then it must be the other tank.
            if (parent.GetChild(iTankIdx) != transform)
            {
                vecToOpp = transform.position - parent.GetChild(iTankIdx).position;
                break;
            }
        }
        //----------------------------------------------------------------------------------------------


        //Todo: Add code here.


        //We now have our angle.
        bChosenAim = true;
    }

    //--------------------------------------------------------------------------------------------------

    public override bool Fire()
    {
        if(!bChosenPower)
        {
            //Get the desired power for the shot.
            SetPower();
        }
        else
        {
            if (powerBar != null)
            {
                //Power of shot increases whilst we are not at the right power.
                if (fFirePower < fDesiredPower)
                {
                    fFirePower += 50.0f;

                    //Fill the power bar the required amount.
                    powerBar.value = fFirePower / kMaxPower;
                }

                //When power is set - Fire a rocket.
                else
                {
                    FireRocket();

                    //Reset internal variables to allow us to recalculate next turn.
                    Reset();

                    return true;
                }
            }
        }
       
        //return true when at correct power.
        return false;
    }

    //--------------------------------------------------------------------------------------------------

    void SetPower()
    { 
        Vector3 vecToOpp = Vector3.zero;

        //----------------------------------------------------------------------------------------------
        //Get the other tank - Both should be children of the same parent.
        Transform parent = transform.parent;
        for (int iTankIdx = 0; iTankIdx < parent.childCount; iTankIdx++)
        {
            //If the current tank is not us, then it must be the other tank.
            if (parent.GetChild(iTankIdx) != transform)
            {
                vecToOpp = parent.GetChild(iTankIdx).position - transform.position;
                break;
            }
        }
        //----------------------------------------------------------------------------------------------


        //Todo: Add code here.


        //We now have our power.
        bChosenPower = true;
    }

    //--------------------------------------------------------------------------------------------------
}
