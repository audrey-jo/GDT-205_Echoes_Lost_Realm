/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyFunctions
{
    //--------------------------------------------------------------------------------------------------
    //Set Operators

    static public float AND(float a, float b)
    {
        return Mathf.Min(a, b);
    }

    //--------------------------------------------------------------------------------------------------

    static public float OR(float a, float b)
    {
        return Mathf.Max(a, b);
    }

    //--------------------------------------------------------------------------------------------------

    static public float NOT(float a)
    {
        return 1.0f - a;
    }

    //--------------------------------------------------------------------------------------------------
    //Fuzzy Set functions.

    static public float Gradient(float fValue, float fLow, float fHigh)
    {
        //            fHigh
        //  |        X------------------
        //  |       .
        //  |      .
        //  |     X
        //  ----------------------------
        //        fLow

        if(fValue <= fLow)
        {
            return 0.0f;
        }
        else if(fValue >= fHigh)
        {
            return 1.0f;
        }
        else
        {
            float fDifference = fHigh - fLow;
            if(fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return ((fValue - fLow) / fDifference);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    static public float ReverseGradient(float fValue, float fLow, float fHigh)
    {
        //           fHigh
        //  |-------X
        //  |        .
        //  |         .
        //  |          X
        //  ----------------------------
        //              fLow

        if(fValue <= fHigh)
        {
            return 1.0f;
        }
        else if(fValue >= fLow)
        {
            return 0.0f;
        }
        else
        {
            float fDifference = fLow-fHigh;
            if (fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f - ((fValue - fHigh) / fDifference);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    static public float Triangle(float fValue, float fLowStart, float fHigh, float fLowEnd)
    {
        //           fHigh
        //  |          X
        //  |        .   .
        //  |      .       .
        //  |     X         X
        //  ----------------------------
        //   fLowStart     fLowEnd

        if (fValue <= fLowStart || fValue >= fLowEnd)
        {
            return 0.0f;
        }
        else if ((fValue > fLowStart) && (fValue < fHigh))
        {
            float fDifference = fHigh - fLowStart;
            if (fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return ((fValue - fLowStart) / fDifference);
            }
        }
        else
        {
            float fDifference = fLowEnd - fHigh;
            if (fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f - ((fValue - fHigh) / fDifference);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------

    static public float BinaryStep(float fValue, float fCutoff, bool bPositiveIfGreaterThanCutoff)
    {
        //         fCutoff
        //  |      X----------
        //  |      .   
        //  |      .       
        //  |------.         
        //  ----------------------------
        
        if(bPositiveIfGreaterThanCutoff)
        {
            if(fValue >= fCutoff)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }
        else
        {
            if(fValue < fCutoff)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

    }

    //--------------------------------------------------------------------------------------------------

    static public float Trapezoid(float fValue, float fLowStart, float fHighStart, float fHighEnd, float fLowEnd)
    {
        //     HighStart    fHighEnd
        //  |        X-------X
        //  |      .          .
        //  |    .             .
        //  |   X               X
        //  ----------------------------
        //   fLowStart        fLowEnd

        if(fValue <= fLowStart)
        {
            return 0.0f;
        }
        else if((fValue >= fHighStart) && (fValue <= fHighEnd))
        {
            return 1.0f;
        }
        else if(fValue >= fLowEnd)
        {
            return 0.0f;
        }
        else if((fValue > fLowStart) && (fValue < fHighStart))
        {
            float fDifference = fHighStart - fLowStart;
            if (fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return ((fValue - fLowStart) / fDifference);
            }
        }
        else
        {
            float fDifference = fLowEnd - fHighEnd;
            if (fDifference == 0.0f)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f - ((fValue - fHighEnd) / fDifference);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
}
*/