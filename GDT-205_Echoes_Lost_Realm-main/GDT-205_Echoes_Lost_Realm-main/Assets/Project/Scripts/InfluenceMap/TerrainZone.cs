
using UnityEngine;

// The areas the Guardian needs to check.
public enum TerrainType
{
    Safe,
    SpikePit,
    EvilSpirit,
    Blocked
}

[RequireComponent(typeof(BoxCollider2D))]
public class TerrainZone : MonoBehaviour
{
    [Header("Terrain Settings")]
    public TerrainType terrainType = TerrainType.Safe;

    private BoxCollider2D area;

    void Awake()
    {
        // Gets the collider marking this area.
        area = GetComponent<BoxCollider2D>();

        // The map zone should not physically block Aria.
        area.isTrigger = true;
    }

    // Gives each area its danger value.
    public int GetDanger()
    {
        switch (terrainType)
        {
            case TerrainType.EvilSpirit:
                return 6;

            case TerrainType.SpikePit:
            case TerrainType.Blocked:
                return 1000;

            default:
                return 0;
        }
    }

    // Checks whether the area should be avoided completely.
    public bool IsBlocked()
    {
        return terrainType == TerrainType.SpikePit ||
               terrainType == TerrainType.Blocked;
    }

    // Checks if a map cell is inside this area.
    public bool Contains(Vector2 position)
    {
        if (!isActiveAndEnabled)
            return false;

        if (area == null)
            area = GetComponent<BoxCollider2D>();

        return area != null &&
               area.enabled &&
               area.OverlapPoint(position);
    }
}
