/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAInstruction
{
    Thrust,
    RotateLeft,
    RotateRight,
    None
};

//--------------------------------------------------------------------------------------

public class Gene
{
    public float fDuration = 0.0f;
    public GAInstruction eInstruction = GAInstruction.None;

    //--------------------------------------------------------------------------------------

    public Gene()
    {
        fDuration = 0.0f;
        eInstruction = GAInstruction.None;
    }

    //--------------------------------------------------------------------------------------

    public void ClearGene()
    {
        fDuration = 0.0f;
        eInstruction = GAInstruction.None;
    }

    //--------------------------------------------------------------------------------------

    public void GenerateRandomGene()
    {
        //Random instruction.
        int iRandomInstruction = Random.Range((int)GAInstruction.Thrust, (int)GAInstruction.RotateRight);
        eInstruction = (GAInstruction)iRandomInstruction;

        //Random duration to wait before the next gene can be accessed.
        float fRandomDuration = Random.Range(0.0f, 0.25f);
        fDuration = fRandomDuration;
    }


    //--------------------------------------------------------------------------------------

    public void Copy(Gene geneToCopy)
    {
        eInstruction = geneToCopy.eInstruction;
        fDuration = geneToCopy.fDuration;

    }

    //--------------------------------------------------------------------------------------
}
*/