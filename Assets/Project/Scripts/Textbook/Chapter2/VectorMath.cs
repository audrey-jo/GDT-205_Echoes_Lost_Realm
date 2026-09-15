using UnityEngine;
class VectorMath
{
    static public float Dot(Vector2 A, Vector2 B)
    {
        //Ensure we are working with unit vectors.
        A = Normalize(A);
        B = Normalize(B);

        //Return dot product A.B
        return (A.x * B.x) + (A.y * B.y);
    }

    static public Vector2 Normalize(Vector2 vec)
    {
        float mag = Magnitude(vec);
        return new Vector2((vec.x / mag), (vec.y / mag));
    }

    static public float Magnitude(Vector2 vec) {return Mathf.Sqrt((vec.x * vec.x) + (vec.y * vec.y));}

    static public Vector2 Perpendicular(Vector2 vec) {return new Vector2(-vec.y, vec.x);}

    static public Vector2 RotateAroundOrigin(float radian)
    {
        Vector2 vReturnPosition = Vector2.zero;
        vReturnPosition.x = Mathf.Cos(radian) - Mathf.Sin(radian);
        vReturnPosition.y = Mathf.Cos(radian) + Mathf.Sin(radian);
        return vReturnPosition;
    }

    static public Vector2 RotateAroundAPoint(Vector2 rotationPoint, Vector2 offsetPoint, float radian)
    {
        Vector2 vReturnToOrigin = offsetPoint-rotationPoint;
        Vector2 vReturnPosition = Vector2.zero;
        vReturnPosition.x = vReturnToOrigin.x * Mathf.Cos(radian) - vReturnToOrigin.y * Mathf.Sin(radian);
        vReturnPosition.y = vReturnToOrigin.x * Mathf.Sin(radian) + vReturnToOrigin.y * Mathf.Cos(radian);
        return vReturnPosition + rotationPoint;
    }

    static public bool LineToLineIntersection(Vector2 vLine1_Start, Vector2 vLine1_End, Vector2 vLine2_Start, Vector2 vLine2_End, ref Vector2 vIntersection)
    {
        //Direction of the lines.
        float fDir1 = ((vLine2_End.x - vLine2_Start.x) * (vLine1_Start.y - vLine2_Start.y) - (vLine2_End.y - vLine2_Start.y)
            * (vLine1_Start.x - vLine2_Start.x)) / ((vLine2_End.y - vLine2_Start.y) * (vLine1_End.x - vLine1_Start.x) - (vLine2_End.x - vLine2_Start.x)
            * (vLine1_End.y - vLine1_Start.y));

        float fDir2 = ((vLine1_End.x - vLine1_Start.x) * (vLine1_Start.y - vLine2_Start.y) - (vLine1_End.y - vLine1_Start.y)
            * (vLine1_Start.x - vLine2_Start.x)) / ((vLine2_End.y - vLine2_Start.y) * (vLine1_End.x - vLine1_Start.x) - (vLine2_End.x - vLine2_Start.x)
            * (vLine1_End.y - vLine1_Start.y));

        //If both lines are between 0 and 1 then the lines are colliding.
        if (fDir1 >= 0 && fDir1 <= 1 && fDir2 >= 0 && fDir2 <= 1)
        {
            //Store the intersection point.
            vIntersection.x = vLine1_Start.x + (fDir1 * (vLine1_End.x - vLine1_Start.x));
            vIntersection.y = vLine1_Start.y + (fDir1 * (vLine1_End.y - vLine1_Start.y));

            return true;
        }
        return false;
    }
}