using UnityEngine;

[RequireComponent(typeof(AriaMovement))]
public class AriaProjectileShooter : MonoBehaviour
{
    // These can be assigned on Aria in the Inspector.
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private Collider2D gardenBounds;
    [SerializeField, Min(0f)] private float muzzleOffset = 0.85f;
    [SerializeField, Min(0f)] private float projectileSpeed = 20f;
    [SerializeField, Min(0f)] private float fireCooldown = 0.2f;

    private PlayerInput input;
    private AriaMovement ariaMovement;
    private float nextFireTime;

    private void Awake()
    {
        // Use the same input actions as Aria's movement script.
        input = new PlayerInput();
        ariaMovement = GetComponent<AriaMovement>();

        if (projectileOrigin == null)
            projectileOrigin = transform.Find("Cannon");

        if (gardenBounds == null)
        {
            GameObject boundsObject = GameObject.Find("GardenBounds");
            if (boundsObject != null)
                gardenBounds = boundsObject.GetComponent<Collider2D>();
        }
    }

    private void OnEnable()
    {
        // Listen for the Shoot action, which is bound to Space.
        input?.Enable();
        if (input != null)
            input.Aria.Shoot.performed += Fire;
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.Aria.Shoot.performed -= Fire;
            input.Disable();
        }
    }

    private void OnDestroy()
    {
        input?.Dispose();
    }

    private void Fire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime)
            return;

        if (projectilePrefab == null)
        {
            Debug.LogError("Assign a projectile prefab to AriaProjectileShooter.", this);
            return;
        }

        // Shoot in the last direction Aria moved, even if she has stopped.
        Vector2 direction = ariaMovement.LastMovementDirection;
        if (direction.sqrMagnitude < 0.001f)
            direction = projectileOrigin != null ? projectileOrigin.right : Vector2.right;
        direction.Normalize();

        Vector3 spawnPosition = projectileOrigin != null
            ? projectileOrigin.position
            : transform.position;
        spawnPosition += (Vector3)(direction * muzzleOffset);
        spawnPosition.z = transform.position.z;

        // Create the projectile just in front of the Cannon.
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        SpawnedObject oldMovement = projectile.GetComponent<SpawnedObject>();
        if (oldMovement != null)
            oldMovement.enabled = false;

        // Use this scene's projectile behavior instead of the old pooled movement script.
        AriaProjectile movement = projectile.GetComponent<AriaProjectile>();
        if (movement == null)
            movement = projectile.AddComponent<AriaProjectile>();
        movement.Initialize(direction, projectileSpeed);

        // Draw the shot over Aria so it stays visible as it leaves the muzzle.
        SpriteRenderer ariaRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer projectileRenderer = projectile.GetComponent<SpriteRenderer>();
        if (ariaRenderer != null && projectileRenderer != null)
        {
            projectileRenderer.sortingLayerID = ariaRenderer.sortingLayerID;
            projectileRenderer.sortingOrder = ariaRenderer.sortingOrder + 5;
        }

        // Ignore Aria and the garden's area trigger so neither affects the shot.
        Collider2D[] ariaColliders = GetComponentsInChildren<Collider2D>();
        Collider2D[] projectileColliders = projectile.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D ariaCollider in ariaColliders)
        {
            foreach (Collider2D projectileCollider in projectileColliders)
                Physics2D.IgnoreCollision(projectileCollider, ariaCollider, true);
        }

        if (gardenBounds != null)
        {
            foreach (Collider2D projectileCollider in projectileColliders)
                Physics2D.IgnoreCollision(projectileCollider, gardenBounds, true);
        }

        nextFireTime = Time.time + fireCooldown;
    }
}
