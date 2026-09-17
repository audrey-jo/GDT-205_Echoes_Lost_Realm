using UnityEngine;

public class AriaMovement : MonoBehaviour
{
    private PlayerInput input;
    private Vector2 movementInput;
    [SerializeField] private float moveSpeed = 5f;
    void Awake()
    {
        input = new PlayerInput();
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Update()
    {
        movementInput = input.Aria.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, movementInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
