using UnityEngine;

public class AriaMovement : MonoBehaviour
{
    // INPUT

    private PlayerInput input;

    private Vector2 movementInput;


    // MOVEMENT

    [SerializeField] private float moveSpeed = 5f;


    // START

    private void Awake()
    {
        // Create the input controls
        input = new PlayerInput();
    }


    private void OnEnable()
    {
        // Make sure input exists
        if (input == null)
        {
            input = new PlayerInput();
        }

        // Turn controls on
        input.Enable();
    }


    private void OnDisable()
    {
        // Only disable if input exists
        if (input != null)
        {
            input.Disable();
        }
    }


    // MOVEMENT

    private void Update()
    {
        // Get movement input
        movementInput =
            input.Aria.Move.ReadValue<Vector2>();


        // Make movement direction
        Vector3 move = new Vector3(
            movementInput.x,
            movementInput.y,
            0f
        );


        // Move Aria
        transform.Translate(
            move * moveSpeed * Time.deltaTime,
            Space.World
        );
    }
}