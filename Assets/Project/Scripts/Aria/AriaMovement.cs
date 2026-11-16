using UnityEngine;

public class AriaMovement : MonoBehaviour
{
    // INPUT

    private PlayerInput input;
    private Vector2 movementInput;
    private Vector2 lastMovementDirection = Vector2.right;
    private Transform cannon;

    public Vector2 LastMovementDirection => lastMovementDirection;

    // MOVEMENT

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    // START

    // Create the input controls and get rigidbody
    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        cannon = transform.Find("Cannon");
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

        // Keep the last direction so Aria can shoot forward while standing still.
        if (move.sqrMagnitude > 0.001f)
            lastMovementDirection = move;

        if (cannon != null)
            cannon.right = lastMovementDirection;

        // Move Aria
        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * move);
    }
}