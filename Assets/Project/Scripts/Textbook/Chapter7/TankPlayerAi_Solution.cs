/*
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
        //Get Fuzzy set values
        float fElevation = vecToOpp.y;

        //Shift elevation into positive values.
        float fElevationShift = 75.0f;
        fElevation += fElevationShift;

        float fElevation_Below = FuzzyFunctions.ReverseGradient(fElevation, fElevationShift, 0.0f);
        float fElevation_Same = FuzzyFunctions.Triangle(fElevation, fElevationShift - 32.5f, fElevationShift, fElevationShift + 32.5f);
        float fElevation_Above = FuzzyFunctions.Gradient(fElevation, fElevationShift, fElevationShift + 75.0f);

        //Ai tank is always to the right, so negative wind (<<<<) is with us, positive wind (>>>>>) is against.
        float fWindStrength = environment.fWindStrength;

        //We need to shift the wind into a positive bracket.
        fWindStrength += Environment.kMaxWindStrength;

        //The fuzzy sets are also offset by the same margin so we are only dealing with positive values
        float fWind_Fast_With = FuzzyFunctions.ReverseGradient(fWindStrength, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength - 5.0f);
        float fWind_Slow_With = FuzzyFunctions.Triangle(fWindStrength, Environment.kMaxWindStrength - 5.0f, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength + 2.5f);
        float fWind_Slow_Against = FuzzyFunctions.Triangle(fWindStrength, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength + 2.5f, Environment.kMaxWindStrength + 5.0f);
        float fWind_Fast_Against = FuzzyFunctions.Gradient(fWindStrength, Environment.kMaxWindStrength + 2.5f, Environment.kMaxWindStrength + 5.0f);
        

        //----------------------------------------------------------------------------------------------
        //Get Fuzzy rule values.
        float fRule1 = FuzzyFunctions.AND(fElevation_Above, fWind_Fast_Against);
        float fRule2 = FuzzyFunctions.AND(fElevation_Same, fWind_Fast_Against);
        float fRule3 = FuzzyFunctions.AND(fElevation_Below, fWind_Fast_Against);

        float fRule4 = FuzzyFunctions.AND(fElevation_Above, fWind_Slow_Against);
        float fRule5 = FuzzyFunctions.AND(fElevation_Same, fWind_Slow_Against);
        float fRule6 = FuzzyFunctions.AND(fElevation_Below, fWind_Slow_Against);

        float fRule7 = FuzzyFunctions.AND(fElevation_Above, fWind_Slow_With);
        float fRule8 = FuzzyFunctions.AND(fElevation_Same, fWind_Slow_With);
        float fRule9 = FuzzyFunctions.AND(fElevation_Below, fWind_Slow_With);

        float fRule10 = FuzzyFunctions.AND(fElevation_Above, fWind_Fast_With);
        float fRule11 = FuzzyFunctions.AND(fElevation_Same, fWind_Fast_With);
        float fRule12 = FuzzyFunctions.AND(fElevation_Below, fWind_Fast_With);

        //----------------------------------------------------------------------------------------------
        //Get Fuzzy membership to output set.
        float fAngle_Low = FuzzyFunctions.OR(fRule1, FuzzyFunctions.OR(fRule2, FuzzyFunctions.OR(fRule3, FuzzyFunctions.OR(fRule9, FuzzyFunctions.OR(fRule10, FuzzyFunctions.OR(fRule11, fRule12))))));
        float fAngle_Medium = FuzzyFunctions.OR(fRule4, FuzzyFunctions.OR(fRule6, fRule8));
        float fAngle_High = FuzzyFunctions.OR(fRule5, fRule7);

        //We don't need to defuzzify these results. Instead we will just take the highest.
        if (fAngle_Low > fAngle_Medium && fAngle_Low > fAngle_High)
        {
            //We want a low shot - Randomly choose a low angle in the relevant range.
            fDesiredAngle = Random.Range(kMediumAngleRange, kLowAngleRange);
        }
        else if (fAngle_Medium > fAngle_High)
        {
            //We want a medium shot - Randomly choose a medium angle in the relevant range.
            fDesiredAngle = Random.Range(kHighAngleRange, kMediumAngleRange);
        }
        else
        {
            //We must want a high shot - Randomly choose a high angle in the relevant range.
            fDesiredAngle = Random.Range(0.0f, kHighAngleRange);
        }

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
        //Get Fuzzy set values
        //We know the width of the environment is 210.
        float fDistance = vecToOpp.magnitude;

        float fDistance_Near = FuzzyFunctions.ReverseGradient(fDistance, 105.0f, 52.5f);
        float fDistance_Medium = FuzzyFunctions.Triangle(fDistance, 52.5f, 105.0f, 157.5f);
        float fDistance_Far = FuzzyFunctions.Gradient(fDistance, 105.0f, 157.5f);

        //Ai tank is always to the right, so negative wind (<<<<) is with us, positive wind (>>>>>) is against.
        float fWindStrength = environment.fWindStrength;

        //We need to shift the wind into a positive bracket.
        fWindStrength += Environment.kMaxWindStrength;

        //The fuzzy sets are also offset by the same margin so we are only dealing with positive values
        float fWind_Fast_With = FuzzyFunctions.ReverseGradient(fWindStrength, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength - 5.0f);
        float fWind_Slow_With = FuzzyFunctions.Triangle(fWindStrength, Environment.kMaxWindStrength - 5.0f, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength + 2.5f);
        float fWind_Slow_Against = FuzzyFunctions.Triangle(fWindStrength, Environment.kMaxWindStrength - 2.5f, Environment.kMaxWindStrength + 2.5f, Environment.kMaxWindStrength + 5.0f);
        float fWind_Fast_Against = FuzzyFunctions.Gradient(fWindStrength, Environment.kMaxWindStrength + 2.5f, Environment.kMaxWindStrength + 5.0f);

        //----------------------------------------------------------------------------------------------
        //Get Fuzzy rule values.
        float fRule1 = FuzzyFunctions.AND(fDistance_Near, fWind_Fast_Against);
        float fRule2 = FuzzyFunctions.AND(fDistance_Medium, fWind_Fast_Against);
        float fRule3 = FuzzyFunctions.AND(fDistance_Far, fWind_Fast_Against);

        float fRule4 = FuzzyFunctions.AND(fDistance_Near, fWind_Slow_Against);
        float fRule5 = FuzzyFunctions.AND(fDistance_Medium, fWind_Slow_Against);
        float fRule6 = FuzzyFunctions.AND(fDistance_Far, fWind_Slow_Against);

        float fRule7 = FuzzyFunctions.AND(fDistance_Near, fWind_Slow_With);
        float fRule8 = FuzzyFunctions.AND(fDistance_Medium, fWind_Slow_With);
        float fRule9 = FuzzyFunctions.AND(fDistance_Far, fWind_Slow_With);

        float fRule10 = FuzzyFunctions.AND(fDistance_Near, fWind_Fast_With);
        float fRule11 = FuzzyFunctions.AND(fDistance_Medium, fWind_Fast_With);
        float fRule12 = FuzzyFunctions.AND(fDistance_Far, fWind_Fast_With);

        //----------------------------------------------------------------------------------------------
        //Get Fuzzy membership to output set.
        float fPower_Light = FuzzyFunctions.OR(fRule4, FuzzyFunctions.OR(fRule7, FuzzyFunctions.OR(fRule10, fRule11)));
        float fPower_Medium = FuzzyFunctions.OR(fRule1,FuzzyFunctions.OR(fRule5, FuzzyFunctions.OR(fRule6, FuzzyFunctions.OR(fRule8, FuzzyFunctions.OR(fRule9, fRule12)))));
        float fPower_Strong = FuzzyFunctions.OR(fRule2, fRule3);

        //We don't need to defuzzify these results. Instead we will just take the highest.
        if(fPower_Light > fPower_Medium && fPower_Light > fPower_Strong)
        {
            //We want a light shot - Randomly choose a light power in the relevant range.
            fDesiredPower = Random.Range(kMinPowerRange, kLowPowerRange);
        }
        else if(fPower_Medium > fPower_Strong)
        {
            //We want a medium shot - Randomly choose a medium power in the relevant range.
            fDesiredPower = Random.Range(kLowPowerRange, kMediumPowerRange);
        }
        else
        {
            //We must want a strong shot - Randomly choose a strong power in the relevant range.
            fDesiredPower = Random.Range(kMediumPowerRange, kStrongPowerRange);
        }

        //We now have our power.
        bChosenPower = true;
    }

    //--------------------------------------------------------------------------------------------------
}
*/