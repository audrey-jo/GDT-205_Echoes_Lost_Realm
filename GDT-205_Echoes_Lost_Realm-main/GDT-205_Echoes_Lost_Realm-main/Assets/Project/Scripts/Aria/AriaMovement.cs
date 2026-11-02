using UnityEngine;
using UnityEngine.InputSystem;

public class AriaMovement : MonoBehaviour
{
    private PlayerInput input;
    private Vector2 movementInput;
    private Vector2 facingDirection = Vector2.up;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Min(0f)] private float projectileSpawnOffset = 0.75f;

    private Rigidbody2D rb;
    private bool missingProjectileWarningShown;

    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (input == null) return;
        input.Enable();
        input.Aria.Shoot.performed += Shoot;
    }

    private void OnDisable()
    {
        if (input == null) return;
        input.Aria.Shoot.performed -= Shoot;
        input.Disable();
    }

    private void FixedUpdate()
    {
        movementInput = input.Aria.Move.ReadValue<Vector2>();
        Vector2 move = new Vector2(movementInput.x, movementInput.y).normalized;
        if (move.sqrMagnitude > 0f) facingDirection = move;
        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * move);
    }

    private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (projectilePrefab == null)
        {
            if (!missingProjectileWarningShown)
            {
                Debug.LogWarning("Assign the Projectile prefab on AriaMovement to enable shooting.", this);
                missingProjectileWarningShown = true;
            }
            return;
        }

        Vector2 arrowAim = ReadArrowAimDirection();
        if (arrowAim.sqrMagnitude > 0f)
        {
            facingDirection = arrowAim;
        }
        else
        {
            Vector2 currentMove = input.Aria.Move.ReadValue<Vector2>().normalized;
            if (currentMove.sqrMagnitude > 0f) facingDirection = currentMove;
        }

        Vector3 spawnPosition = transform.position + (Vector3)(facingDirection * projectileSpawnOffset);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        SpawnedObject projectileMovement = projectile.GetComponent<SpawnedObject>();
        if (projectileMovement == null)
        {
            Debug.LogError("The assigned Projectile prefab needs a SpawnedObject component.", projectile);
            Destroy(projectile);
            return;
        }

        projectileMovement.SetDirection(facingDirection);
    }

    private static Vector2 ReadArrowAimDirection()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return Vector2.zero;

        Vector2 direction = Vector2.zero;
        if (keyboard.leftArrowKey.isPressed) direction.x -= 1f;
        if (keyboard.rightArrowKey.isPressed) direction.x += 1f;
        if (keyboard.downArrowKey.isPressed) direction.y -= 1f;
        if (keyboard.upArrowKey.isPressed) direction.y += 1f;

        return direction.sqrMagnitude > 0f ? direction.normalized : Vector2.zero;
    }
}