/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(SpriteRenderer))]
public class Pill : MonoBehaviour
{
    public static int pillCount = 0;

    [SerializeField] private Sprite normalPillSprite = null;
    [SerializeField] private Sprite powerPillSprite  = null;

    private GhostHandler            ghostHandler     = null;

    private PillType                pillType         = PillType.Normal;

    //-------------------------------------------------------------------------------------------

    public void SetPillType(PillType newType)
    {
         //Switch the type.
        pillType = newType;

        //Set the correct sprite to show.
        if (pillType == PillType.Normal)
        {
            GetComponent<SpriteRenderer>().sprite = normalPillSprite;
        }
        else if (pillType == PillType.Power)
        {
            GetComponent<SpriteRenderer>().sprite = powerPillSprite;
        }
    }

    //-------------------------------------------------------------------------------------------

    void Start()
    {
        Pill.pillCount++;
        ghostHandler = GameObject.FindGameObjectWithTag("GhostHandler").GetComponent<GhostHandler>();
    }

    //-------------------------------------------------------------------------------------------

    public PillType GetPillType()
    {
        return pillType;
    }

    //-------------------------------------------------------------------------------------------

    public void SetEaten()
    {
        //Ghosts are afraid of player when it has easten a power pill.
        if (pillType == PillType.Power)
        {
            ghostHandler.SetGhostsAfraid();
        }

        Pill.pillCount--;
        //Debug.Log("Pills: " + Pill.pillCount);

        Destroy(gameObject);
    }

    //-------------------------------------------------------------------------------------------
}
*/