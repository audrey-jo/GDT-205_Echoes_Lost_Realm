using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Factory : MonoBehaviour
{

    [Header("Sizing")]

    [Range(0, 100)]
    public int poolSize = 10;
    [Range(0, 100)]
    public int spawnRate = 1;

    [Space]

    [Header("Objects")]

    public List<SpawnOption> spawnOptions = new();
    public int selectedIndex;
    [SerializeField] private GameObject instantKillProjectilePrefab;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField, Min(0f)] private float projectileMuzzleOffset = 0.85f;

    public SpawnOption Selected => spawnOptions.Count > 0 ? spawnOptions[selectedIndex] : null;

    private SpriteRenderer instantKillAppearance;
    [Space]

    [Header("Timing")]

    public InputActionReference spawnInput;
    [Range(0f, 100f)]
    public float spawnDelay = 1f;
    [Range(0f, 100f)]

    private Queue<GameObject> objectQueue = new();
    private System.Action<InputAction.CallbackContext> spawnAction;
    private List<GameObject> activeObjects = new();

    void Start()
    {
        if (projectileOrigin == null && transform.parent != null)
            projectileOrigin = transform.parent.Find("Cannon");

        if (instantKillProjectilePrefab != null)
            instantKillAppearance = instantKillProjectilePrefab.GetComponent<SpriteRenderer>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Selected.prefab);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
            if (obj.TryGetComponent<SpawnedObject>(out var spawned))
                spawned.OnDespawned.AddListener(() => Despawn(obj));
        }

        if (spawnInput != null)
        {
            spawnInput.action.Enable();
            spawnAction = ctx => Spawn();
            spawnInput.action.performed += spawnAction;
        }

    }

    void OnDisable()
    {
        if (spawnInput != null)
        {
            spawnInput.action.Disable();
            spawnInput.action.performed -= spawnAction;
        }
    }

    public void Spawn()
    {
        if (objectQueue.Count <= 0)
            Despawn(activeObjects[0]);

        GameObject obj = objectQueue.Dequeue();
        Transform origin = projectileOrigin != null ? projectileOrigin : transform;
        Vector2 direction = origin.right;
        Vector3 spawnPosition = origin.position + (Vector3)(direction.normalized * projectileMuzzleOffset);
        spawnPosition.z = transform.position.z;
        obj.transform.position = spawnPosition;

        if (obj.TryGetComponent<SpawnedObject>(out var spawned))
        {
            AriaCollisions owner = GetComponentInParent<AriaCollisions>();
            spawned.SetOwner(owner != null ? owner.transform : null);
            spawned.SetTravelDirection(direction);
            spawned.SetInstantKill(
                AriaCollisions.InstantKillProjectilesEnabled,
                instantKillAppearance);
        }

        Collider2D ariaCollider = GetComponentInParent<AriaCollisions>()?.GetComponentInChildren<Collider2D>();
        Collider2D projectileCollider = obj.GetComponent<Collider2D>();
        obj.SetActive(true);

        if (ariaCollider != null && projectileCollider != null)
            Physics2D.IgnoreCollision(projectileCollider, ariaCollider, true);

        activeObjects.Add(obj);
    }

    public void Despawn(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);

            objectQueue.Enqueue(obj);

            activeObjects.Remove(obj);
        }
    }
}

[System.Serializable]
public class SpawnOption { public GameObject prefab; }