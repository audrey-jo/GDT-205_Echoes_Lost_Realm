using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float g;
    public float h;
    public float f;

    public int ParentX;
    public int ParentY;

    //-----------------------------------------------------------------------------------

    public void SetStartingValues()
    {
        g = 0.0f;
        h = 0.0f;
        f = 0.0f;

        ParentX = 0;
        ParentY = 0;
    }

    //-----------------------------------------------------------------------------------

}
