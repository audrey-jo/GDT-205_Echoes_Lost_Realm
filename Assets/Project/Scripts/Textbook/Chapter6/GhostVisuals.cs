/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostVisuals : MonoBehaviour
{
    private SpriteRenderer  visibleSpriteRenderer   = null;
    private Direction       currentDirection        = Direction.Up;
    private SpriteSet       currentSpriteSet        = SpriteSet.Chasing;


    public List<Sprite>     chasingSprites          = new List<Sprite>();
    public List<Sprite>     evadingSprites          = new List<Sprite>();
    public List<Sprite>     eyesSprites             = new List<Sprite>();

    //------------------------------------------------------------------------------------------------

    void Start()
    {
        visibleSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    //------------------------------------------------------------------------------------------------

    public void SetDirection(Direction newDirection, bool forceChange = false)
    {
        if(newDirection != currentDirection || forceChange)
        {
            //Store the new direction.
            currentDirection = newDirection;

            //Set the visuals.
            switch(currentSpriteSet)
            {
                case SpriteSet.Chasing:
                    visibleSpriteRenderer.sprite = chasingSprites[(int)currentDirection];
                break;

                case SpriteSet.Evading:
                    visibleSpriteRenderer.sprite = evadingSprites[(int)currentDirection];
                break;

                case SpriteSet.Eyes:
                    visibleSpriteRenderer.sprite = eyesSprites[(int)currentDirection];
                break;

                default:
                    Debug.LogError("---GhostVisuals.cs--- Incorrect SpriteSet");
                break;
            }
        }
    }

    //------------------------------------------------------------------------------------------------

    public Direction GetDirection()
    {
        return currentDirection;
    }

    //------------------------------------------------------------------------------------------------

    public void SetSpriteSet(SpriteSet newSpriteSet)
    {
        if (newSpriteSet != currentSpriteSet)
        {
            //Store the new sprite set.
            currentSpriteSet = newSpriteSet;

            //Ensure it is visible.
            SetVisible();

            //Change the visuals of the ghost as well.
            SetDirection(currentDirection, true);
        }
    }

    //------------------------------------------------------------------------------------------------

    public void Flash()
    {
        //Switch the alpha between 0 and 1 to flash the sprite.
        Color tempColor = visibleSpriteRenderer.color;
        if (tempColor.a == 1.0f)
        {
            tempColor.a = 0.0f;
        }
        else
        {
            tempColor.a = 1.0f;
        }

        visibleSpriteRenderer.color = tempColor;
    }

    //------------------------------------------------------------------------------------------------

    public void SetVisible()
    {
        //Set the alpha to 1.
        Color tempColor = visibleSpriteRenderer.color;
        tempColor.a = 1.0f;
        visibleSpriteRenderer.color = tempColor;
    }

    //------------------------------------------------------------------------------------------------
}
*/