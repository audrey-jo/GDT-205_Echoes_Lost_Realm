using UnityEngine;

public class AriaMovement : MonoBehaviour
{
    // INPUT

    private PlayerInput input;
    private Vector2 movementInput;

    // MOVEMENT

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    // START

    // Create the input controls and get rigidbody
    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    // INPUT

    private void OnEnable() { input?.Enable(); }
    private void OnDisable() { input?.Disable(); }

    // MOVEMENT

    private void FixedUpdate()
    {
        // Get movement input
        movementInput = input.Aria.Move.ReadValue<Vector2>();

        // Make movement direction
        Vector2 move = new Vector2(movementInput.x, movementInput.y).normalized;

        // Move Aria
        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * move);
    }
}