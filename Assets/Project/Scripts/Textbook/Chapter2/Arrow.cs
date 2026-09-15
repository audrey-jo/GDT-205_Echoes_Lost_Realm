using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [SerializeField] Text dotProductOutput;
    [SerializeField] Text degreesOutput;
    [SerializeField] Text radiansOutput;

    Vector3 centreOfUnitCircle;
    private void Start()
    {
        //Calculate the centre of the unit circle, which is centred on the screen.
        centreOfUnitCircle = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    void Update()
    {
        //Check if the left mouse button is down.
        if(Input.GetMouseButton(0))
        {
            //Calculate a vector from centre of circle to the mouse position.
            Vector3 toMousePos = Input.mousePosition - centreOfUnitCircle;
            Debug.Log(toMousePos);

            //Normalize it.
            Vector2 unitVectorToMousePos = VectorMath.Normalize(toMousePos);

            //Then find the dot product from the up vector (-1,1).
            float fDot = VectorMath.Dot(Vector2.up, unitVectorToMousePos);
            if(dotProductOutput != null)
            {
                dotProductOutput.text = fDot.ToString("#0.000");
            }

            //Get the Radians.
            float fRadians = Mathf.Acos(fDot);
            if(radiansOutput != null)
            {
                radiansOutput.text = fRadians.ToString("#0.000");
            }

            //Get the degrees.
            float degrees = fRadians * Mathf.Rad2Deg;
            if (degreesOutput != null)
            {
                degreesOutput.text = degrees.ToString("#0.000");
            }

            //We need to know whether this is a right or left rotation.
            float dotRight = VectorMath.Dot(Vector2.right, unitVectorToMousePos);

            //Default to a left rotation.
            int dir = 1;

            //If the dot product is greater than 0.0, then it indicates a rotation to the right.
            if(dotRight > 0.0f)
            {
                dir = -1;
            }
            RotateArrow(degrees, dir);
        }
    }
    void RotateArrow(float degrees, int dir)
    {
        //Rotate the arrow to face this direction.
        Vector3 euler = new Vector3(0.0f, 0.0f, degrees * dir);

        Quaternion qNewRotation = Quaternion.identity;
        qNewRotation.eulerAngles = euler;
        transform.rotation = qNewRotation;
    }
}
