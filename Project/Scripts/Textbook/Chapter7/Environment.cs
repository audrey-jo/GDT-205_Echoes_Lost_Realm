using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    public static float kMaxWindStrength = 10.0f;

    [SerializeField] Transform tankParent = null;
    public List<GameObject> environmentPrefabs = new List<GameObject>();
    public GameObject tankPrefab = null;
    public GameObject aiTankPrefab = null;

    [SerializeField] Material redMaterial = null;
    [SerializeField] Material blueMaterial = null;

    public float fWindStrength = 0.0f;
    public WindDirection eWindDir = WindDirection.Left;

    [SerializeField] Text windSpeedText = null;
    [SerializeField] Text windDirText = null;

    public LineRenderer environmentLines = null;

    private bool bSetup = false;
    public bool IsSetup() { return bSetup; }

    private bool bIsAiGame = false;
    public void SetAIGame() { bIsAiGame = true; }

    //---------------------------------------------------------------------------

    private void Start()
    {
        int randomEnvironment = Random.Range(0, environmentPrefabs.Count);

        if(environmentPrefabs[randomEnvironment] != null)
        {
            GameObject newGO = Instantiate(environmentPrefabs[randomEnvironment]);
            environmentLines = newGO.GetComponent<LineRenderer>();

            GeneratePlayerTanks();
            GenerateWind();
        }

        bSetup = true;
    }

    //---------------------------------------------------------------------------

    private void GeneratePlayerTanks()
    {
        if(environmentLines != null && tankPrefab != null && tankParent != null)
        {
            float xRange = environmentLines.GetPosition(0).x - environmentLines.GetPosition(environmentLines.positionCount - 1).x;
            float xHalfRange = xRange / 2.0f;

            //---------------------------------------------------------------------------
            //Player 1
            //Randomly select an x position over to the left of environment.
            float leftX = -xHalfRange;
            float RightX = (xRange / 3.0f) - xHalfRange;
            float randomX = Random.Range(leftX, RightX);

            //Project a vector down from the top of the screen and find where it itersects with the environment lines.
            Vector2 vStartPosition = new Vector2(randomX, -100.0f);
            Vector2 vEndPosition = new Vector2(randomX, 100.0f);
            Vector2 vIntersection = Vector2.zero;

            //Loop through the lines that make up the lines that make up the environment and see if we are intersecting any of them.
            for (int groundLineIdx = 0; groundLineIdx < environmentLines.positionCount - 1; groundLineIdx++)
            {
                Vector2 environmentLinePos1 = environmentLines.GetPosition(groundLineIdx);
                Vector2 environmentLinePos2 = environmentLines.GetPosition(groundLineIdx + 1);

                //Did we intersect?
                if (VectorMath.LineToLineIntersection(vStartPosition, vEndPosition, environmentLinePos1, environmentLinePos2, ref vIntersection))
                {
                    //If so, exit this loop.
                    break;
                }
            }

            //Place a tank here.
            GameObject tank1GO = Instantiate(tankPrefab, vIntersection, Quaternion.identity, tankParent);

            //Colour it blue.
            LineRenderer tank1LR = tank1GO.GetComponent<LineRenderer>();
            if(tank1LR != null && redMaterial != null)
            {
                tank1LR.material = redMaterial;
            }


            //---------------------------------------------------------------------------
            //Player 2
            //Randomly select an x position over to the right of environment.
            leftX = (xRange - (xRange / 3.0f))-xHalfRange;
            RightX = xRange - xHalfRange;
            randomX = Random.Range(leftX, RightX);

            //Project a vector down from the top of the screen and find where it itersects with the environment lines.
            vStartPosition = new Vector2(randomX, -100.0f);
            vEndPosition = new Vector2(randomX, 100.0f);
            vIntersection = Vector2.zero;

            //Loop through the lines that make up the lines that make up the environment and see if we are intersecting any of them.
            for (int groundLineIdx = 0; groundLineIdx < environmentLines.positionCount - 1; groundLineIdx++)
            {
                Vector2 environmentLinePos1 = environmentLines.GetPosition(groundLineIdx);
                Vector2 environmentLinePos2 = environmentLines.GetPosition(groundLineIdx + 1);

                //Did we intersect?
                if (VectorMath.LineToLineIntersection(vStartPosition, vEndPosition, environmentLinePos1, environmentLinePos2, ref vIntersection))
                {
                    //If so, exit this loop.
                    break;
                }
            }

            //Place a tank here.
            GameObject tank2GO = null;
            if (bIsAiGame)
            {
                //Create an AI player.
                tank2GO = Instantiate(aiTankPrefab, vIntersection, Quaternion.identity, tankParent);
            }
            else
            {
                //Create a 2nd human ontrolled player.
                tank2GO = Instantiate(tankPrefab, vIntersection, Quaternion.identity, tankParent);
            }

            //Colour it red.
            LineRenderer tank2LR = tank2GO.GetComponent<LineRenderer>();
            if (tank2LR != null && blueMaterial != null)
            {
                tank2LR.material = blueMaterial;
            }
        }
    }

    //---------------------------------------------------------------------------

    public void GenerateWind()
    {
        fWindStrength = Random.Range(-kMaxWindStrength, kMaxWindStrength);
        if (windSpeedText != null)
        {
            windSpeedText.text = "Wind Strength: " + fWindStrength.ToString("#0.00"); ;
        }

        if (fWindStrength == 0.0f)
        {
            if (windDirText != null)
            {
                windDirText.text = "";

            }
        }
        else if (fWindStrength > 0.0f)
        {
            eWindDir = WindDirection.Right;
            if (windDirText != null)
            {
                windDirText.text = ">>";

                int numberOfArrows = (int)Mathf.Abs(fWindStrength);
                for (int i = 0; i < numberOfArrows; i++)
                {
                    windDirText.text += ">>";
                }
            }
        }
        else
        {
            eWindDir = WindDirection.Left;
            if (windDirText != null)
            {
                windDirText.text = "<<";

                int numberOfArrows = (int)Mathf.Abs(fWindStrength);
                for (int i = 0; i < numberOfArrows; i++)
                {
                    windDirText.text += "<<";
                }
            }
        }
    }

    //---------------------------------------------------------------------------

    public void CreateCrater(Vector2 explosionPos, Vector2 direction)
    {
        //Normalise - Just in case it isn't already a unit vector.
        direction = VectorMath.Normalize(direction);

        if (environmentLines != null)
        {
            float fExplosionRadius = 2.0f;
            int leftSegment = -1;
            int rightSegment = -1;

            //Find the line segment affected to the left of the explosion.
            //Project a vector down from the top of the screen and find where it itersects with the environment lines.
            Vector2 vStartPosition = new Vector2(explosionPos.x + fExplosionRadius, -100.0f);
            Vector2 vEndPosition = new Vector2(explosionPos.x + fExplosionRadius, 100.0f);
            Vector2 vLeftIntersection = Vector2.zero;

            //Loop through the lines that make up the lines that make up the environment and see if we are intersecting any of them.
            for (int groundLineIdx = 0; groundLineIdx < environmentLines.positionCount - 1; groundLineIdx++)
            {
                Vector2 environmentLinePos1 = environmentLines.GetPosition(groundLineIdx);
                Vector2 environmentLinePos2 = environmentLines.GetPosition(groundLineIdx + 1);

                //Did we intersect?
                if (VectorMath.LineToLineIntersection(vStartPosition, vEndPosition, environmentLinePos1, environmentLinePos2, ref vLeftIntersection))
                {
                    //If so, exit this loop.
                    leftSegment = groundLineIdx+1;
                    break;
                }
            }

            vStartPosition = new Vector2(explosionPos.x - fExplosionRadius, -100.0f);
            vEndPosition = new Vector2(explosionPos.x - fExplosionRadius, 100.0f);
            Vector2 vRightIntersection = Vector2.zero;

            //Loop through the lines that make up the lines that make up the environment and see if we are intersecting any of them.
            for (int groundLineIdx = 0; groundLineIdx < environmentLines.positionCount - 1; groundLineIdx++)
            {
                Vector2 environmentLinePos1 = environmentLines.GetPosition(groundLineIdx);
                Vector2 environmentLinePos2 = environmentLines.GetPosition(groundLineIdx + 1);

                //Did we intersect?
                if (VectorMath.LineToLineIntersection(vStartPosition, vEndPosition, environmentLinePos1, environmentLinePos2, ref vRightIntersection))
                {
                    //If so, exit this loop.
                    rightSegment = groundLineIdx+1;
                    break;
                }
            }
            Vector3[] positionsAsArray = new Vector3[environmentLines.positionCount];
            environmentLines.GetPositions(positionsAsArray);
            List<Vector3> positionAsList = new List<Vector3>();
            for(int i = 0; i < positionsAsArray.Length; i++)
            {
                positionAsList.Add(positionsAsArray[i]);
            }

            if (leftSegment != -1 && rightSegment != -1)
            {
                int lineSegment;
                if (leftSegment == rightSegment)
                {
                    lineSegment = leftSegment;

                    //Duplicate position at this index.
                    positionAsList.Insert(lineSegment, positionsAsArray[lineSegment]);
                   
                    //Change where this line segment ends.
                    positionAsList[lineSegment] = vLeftIntersection;
                    lineSegment++;

                    //Add a new position at other side of explosion next.
                    positionAsList.Insert(lineSegment, vRightIntersection);

                    //Add a deep position between the two.
                    explosionPos += direction * fExplosionRadius;
                    positionAsList.Insert(lineSegment, explosionPos);
                }
                else
                {
                    //Get the valid line segment.
                    lineSegment = leftSegment;

                    //Push this position back.
                    positionAsList[lineSegment] += (Vector3)direction * fExplosionRadius;
                }

                //Change the environment lines.
                positionsAsArray = new Vector3[positionAsList.Count];
                for (int i = 0; i < positionAsList.Count; i++)
                {
                    positionsAsArray[i] = positionAsList[i];
                }
                environmentLines.positionCount = positionAsList.Count;
                environmentLines.SetPositions(positionsAsArray);
            }
        }
    }

    //---------------------------------------------------------------------------
}
