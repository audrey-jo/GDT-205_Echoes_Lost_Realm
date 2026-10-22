
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TerrainMaps : MonoBehaviour
{
    [Header("Scene References")]
    public Transform aria;
    public SpriteRenderer referenceSquare;
    public Transform gridBottomLeft;
    public Transform gridTopRight;
    public GameObject temporaryObstacle;

    [Header("Grid Settings")]
    [Min(1)] public int cellsPerSquare = 1;
    [Min(0.005f)] public float fallbackSquareSize = 0.25f;
    [Min(16)] public int maxCellsPerSide = 256;
    public int overlaySortingOrder = 5;

    [Header("Heat Map")]
    public float heatGain = 0.45f;
    public float maxHeat = 5f;
    public float heatFadeSpeed = 0.15f;

    [Header("Influence Map")]
    [Min(0f)] public float heatWeight = 0.5f;
    public float dangerFadeSpeed = 0.3f;

    private int columns;
    private int rows;
    private float cellSize;
    private Vector2 origin;

    private int[,] danger;
    private bool[,] blocked;
    private float[,] heat;
    private float[,] extraDanger;

    private Texture2D mapTexture;
    private Sprite mapSprite;
    private SpriteRenderer mapRenderer;
    private Color[] mapColors;

    private Vector2 lastAriaPosition;

    private float lastHeatTime;
    private float nextHeatUpdate;
    private float nextTerrainUpdate;
    private float nextDisplayUpdate;

    // Keeps track of the selected map.
    private enum MapMode
    {
        Hidden,
        Influence,
        Heat
    }

    private MapMode currentMode = MapMode.Hidden;

    // The four directions used for pathfinding.
    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start()
    {
        if (gridBottomLeft == null || gridTopRight == null)
        {
            Debug.LogError(
                "TerrainMaps: Assign both grid corner objects.",
                this
            );

            enabled = false;
            return;
        }

        // Creates the grid over the existing level.
        if (!CreateGrid())
        {
            enabled = false;
            return;
        }

        if (aria != null)
            lastAriaPosition = aria.position;

        lastHeatTime = Time.time;

        nextHeatUpdate = Time.time + 0.1f;
        nextTerrainUpdate = Time.time + 0.25f;

        RefreshInfluenceMap();
        DrawMap();
    }

    void Update()
    {
        ReadControls();

        // Records movement and fades old activity.
        if (Time.time >= nextHeatUpdate)
        {
            float elapsed = Time.time - lastHeatTime;

            FadeValues(elapsed);
            RecordAriaMovement();

            lastHeatTime = Time.time;
            nextHeatUpdate = Time.time + 0.1f;
        }

        // Checks the spike pits and moving spirits.
        if (Time.time >= nextTerrainUpdate)
        {
            RefreshInfluenceMap();
            nextTerrainUpdate = Time.time + 0.25f;
        }

        // Redraws the selected map.
        if (Time.time >= nextDisplayUpdate)
        {
            DrawMap();
            nextDisplayUpdate = Time.time + 0.1f;
        }
    }

    // Handles the map controls.
    void ReadControls()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // Press 2 to show or hide the Influence Map.
        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            currentMode = currentMode == MapMode.Influence
                ? MapMode.Hidden
                : MapMode.Influence;
        }

        // Press 3 to show or hide the Heat Map.
        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            currentMode = currentMode == MapMode.Heat
                ? MapMode.Hidden
                : MapMode.Heat;
        }

        // Press 1 to hide the current map.
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            currentMode = MapMode.Hidden;
        }

        // Turns the test obstacle on or off.
        if (keyboard.oKey.wasPressedThisFrame &&
            temporaryObstacle != null)
        {
            temporaryObstacle.SetActive(
                !temporaryObstacle.activeSelf
            );

            RefreshInfluenceMap();
        }
    }

    // Creates the grid using the existing Square tile size.
    bool CreateGrid()
    {
        float squareSize = fallbackSquareSize;

        if (referenceSquare != null)
        {
            Vector3 size = referenceSquare.bounds.size;

            float measuredSize = Mathf.Min(size.x, size.y);

            if (measuredSize > 0.001f)
                squareSize = measuredSize;
        }

        float requestedCellSize =
            squareSize / Mathf.Max(1, cellsPerSquare);

        // Finds the edges of the playable area.
        float minX = Mathf.Min(
            gridBottomLeft.position.x,
            gridTopRight.position.x
        );

        float minY = Mathf.Min(
            gridBottomLeft.position.y,
            gridTopRight.position.y
        );

        float maxX = Mathf.Max(
            gridBottomLeft.position.x,
            gridTopRight.position.x
        );

        float maxY = Mathf.Max(
            gridBottomLeft.position.y,
            gridTopRight.position.y
        );

        float mapWidth = maxX - minX;
        float mapHeight = maxY - minY;

        if (mapWidth < 0.01f || mapHeight < 0.01f)
        {
            Debug.LogError(
                "TerrainMaps: The grid corners are too close together.",
                this
            );

            return false;
        }

        origin = new Vector2(minX, minY);

        // Limits the resolution if the selected area is very large.
        int limit = Mathf.Max(16, maxCellsPerSide);

        cellSize = Mathf.Max(
            0.005f,
            requestedCellSize,
            mapWidth / limit,
            mapHeight / limit
        );

        if (cellSize > requestedCellSize + 0.001f)
        {
            Debug.LogWarning(
                "TerrainMaps: Cell size was increased to keep " +
                "the grid within the resolution limit.",
                this
            );
        }

        columns = Mathf.Max(
            1,
            Mathf.CeilToInt(mapWidth / cellSize)
        );

        rows = Mathf.Max(
            1,
            Mathf.CeilToInt(mapHeight / cellSize)
        );

        // Stores danger and player activity separately.
        danger = new int[columns, rows];
        blocked = new bool[columns, rows];
        heat = new float[columns, rows];
        extraDanger = new float[columns, rows];

        // Uses one texture instead of hundreds of tile objects.
        mapTexture = new Texture2D(
            columns,
            rows,
            TextureFormat.RGBA32,
            false
        );

        mapTexture.filterMode = FilterMode.Point;
        mapTexture.wrapMode = TextureWrapMode.Clamp;

        mapColors = new Color[columns * rows];

        // One texture pixel represents one analysis cell.
        mapSprite = Sprite.Create(
            mapTexture,
            new Rect(0, 0, columns, rows),
            Vector2.zero,
            1f / cellSize,
            0,
            SpriteMeshType.FullRect
        );

        GameObject overlay = new GameObject("TerrainMapOverlay");

        overlay.transform.SetParent(transform);

        overlay.transform.position = new Vector3(
            origin.x,
            origin.y,
            0f
        );

        mapRenderer = overlay.AddComponent<SpriteRenderer>();
        mapRenderer.sprite = mapSprite;
        mapRenderer.sortingOrder = overlaySortingOrder;

        if (referenceSquare != null)
        {
            mapRenderer.sortingLayerID =
                referenceSquare.sortingLayerID;
        }

        Debug.Log(
            "Terrain grid created: " +
            columns + " x " + rows +
            " | Cell size: " + cellSize
        );

        return true;
    }

    // Returns the center of a grid cell.
    public Vector3 CellCenter(int x, int y)
    {
        return new Vector3(
            origin.x + (x + 0.5f) * cellSize,
            origin.y + (y + 0.5f) * cellSize,
            0f
        );
    }

    // Converts a scene position into grid coordinates.
    public bool WorldToCell(
        Vector3 position,
        out int x,
        out int y
    )
    {
        x = Mathf.FloorToInt(
            (position.x - origin.x) / cellSize
        );

        y = Mathf.FloorToInt(
            (position.y - origin.y) / cellSize
        );

        return x >= 0 && x < columns &&
               y >= 0 && y < rows;
    }

    // Updates danger using the current hazard positions.
    public void RefreshInfluenceMap()
    {
        if (danger == null)
            return;

        TerrainZone[] zones = FindObjectsByType<TerrainZone>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        Physics2D.SyncTransforms();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Clears the last terrain reading.
                danger[x, y] = 0;
                blocked[x, y] = false;

                Vector2 position = CellCenter(x, y);

                foreach (TerrainZone zone in zones)
                {
                    if (zone == null || !zone.Contains(position))
                        continue;

                    // Spikes and walls cannot be used for a route.
                    if (zone.IsBlocked())
                    {
                        blocked[x, y] = true;
                    }
                    else
                    {
                        // Keeps the highest danger when areas overlap.
                        danger[x, y] = Mathf.Max(
                            danger[x, y],
                            zone.GetDanger()
                        );
                    }
                }
            }
        }
    }

    // Records the cells Aria moves through.
    void RecordAriaMovement()
    {
        if (aria == null)
            return;

        Vector2 currentPosition = aria.position;

        float distance = Vector2.Distance(
            lastAriaPosition,
            currentPosition
        );

        // Standing still does not add heat.
        if (distance < 0.02f)
            return;

        // Checks between positions so small cells are not skipped.
        int samples = Mathf.Clamp(
            Mathf.CeilToInt(distance / (cellSize * 0.5f)),
            1,
            512
        );

        int previousCell = -1;

        for (int i = 1; i <= samples; i++)
        {
            Vector2 position = Vector2.Lerp(
                lastAriaPosition,
                currentPosition,
                (float)i / samples
            );

            int x;
            int y;

            if (!WorldToCell(position, out x, out y))
                continue;

            int cellID = y * columns + x;

            // Doesn't count the same cell twice in one sample.
            if (cellID == previousCell)
                continue;

            previousCell = cellID;

            heat[x, y] = Mathf.Min(
                Mathf.Max(0.01f, maxHeat),
                heat[x, y] + heatGain
            );
        }

        lastAriaPosition = currentPosition;
    }

    // Reduces old heat and temporary danger.
    void FadeValues(float elapsed)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                heat[x, y] = Mathf.Max(
                    0f,
                    heat[x, y] - heatFadeSpeed * elapsed
                );

                extraDanger[x, y] = Mathf.Max(
                    0f,
                    extraDanger[x, y] - dangerFadeSpeed * elapsed
                );
            }
        }
    }

    // Lets the Guardian mark newly detected danger.
    public void MarkDanger(Vector3 position, float amount)
    {
        if (extraDanger == null)
            return;

        int x;
        int y;

        if (!WorldToCell(position, out x, out y))
            return;

        extraDanger[x, y] = Mathf.Clamp(
            extraDanger[x, y] + Mathf.Max(0f, amount),
            0f,
            6f
        );
    }

    // Returns the danger at a given position.
    public float GetInfluence(Vector3 position)
    {
        if (danger == null)
            return float.PositiveInfinity;

        int x;
        int y;

        if (!WorldToCell(position, out x, out y))
            return float.PositiveInfinity;

        if (blocked[x, y])
            return float.PositiveInfinity;

        return danger[x, y] + extraDanger[x, y];
    }

    // Returns the player activity recorded in a cell.
    public float GetHeat(Vector3 position)
    {
        if (heat == null)
            return 0f;

        int x;
        int y;

        if (!WorldToCell(position, out x, out y))
            return 0f;

        return heat[x, y];
    }

    // Adds the cost of entering a cell.
    float GetCellCost(int x, int y)
    {
        if (blocked[x, y])
            return float.PositiveInfinity;

        return 1f +
               danger[x, y] +
               extraDanger[x, y] +
               heat[x, y] * heatWeight;
    }

    // Finds a route using distance, danger, and activity.
    public List<Vector3> FindSafestPath(
        Vector3 startPosition,
        Vector3 endPosition
    )
    {
        List<Vector3> path = new List<Vector3>();

        if (danger == null)
            return path;

        int sx, sy;
        int ex, ey;

        if (!WorldToCell(startPosition, out sx, out sy))
            return path;

        if (!WorldToCell(endPosition, out ex, out ey))
            return path;

        if (blocked[sx, sy] || blocked[ex, ey])
            return path;

        float[,] cost = new float[columns, rows];
        bool[,] closed = new bool[columns, rows];
        bool[,] inOpen = new bool[columns, rows];

        Vector2Int[,] previous =
            new Vector2Int[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                cost[x, y] = float.PositiveInfinity;
                previous[x, y] = new Vector2Int(-1, -1);
            }
        }

        Vector2Int start = new Vector2Int(sx, sy);
        Vector2Int goal = new Vector2Int(ex, ey);

        List<Vector2Int> open = new List<Vector2Int>();

        open.Add(start);
        inOpen[sx, sy] = true;
        cost[sx, sy] = 0f;

        // A* compares the available routes.
        while (open.Count > 0)
        {
            int bestIndex = 0;
            float bestScore = float.PositiveInfinity;

            for (int i = 0; i < open.Count; i++)
            {
                Vector2Int point = open[i];

                float estimate =
                    Mathf.Abs(point.x - ex) +
                    Mathf.Abs(point.y - ey);

                float score = cost[point.x, point.y] + estimate;

                if (score < bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                }
            }

            Vector2Int current = open[bestIndex];

            open.RemoveAt(bestIndex);
            inOpen[current.x, current.y] = false;

            // Builds the path once the destination is reached.
            if (current == goal)
            {
                Vector2Int point = goal;

                while (point != start)
                {
                    path.Add(CellCenter(point.x, point.y));

                    point = previous[point.x, point.y];

                    if (point.x < 0 || point.y < 0)
                        return new List<Vector3>();
                }

                path.Add(CellCenter(sx, sy));
                path.Reverse();

                return path;
            }

            closed[current.x, current.y] = true;

            // Checks the neighboring cells.
            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;

                if (next.x < 0 || next.x >= columns ||
                    next.y < 0 || next.y >= rows)
                    continue;

                if (blocked[next.x, next.y] ||
                    closed[next.x, next.y])
                    continue;

                float newCost =
                    cost[current.x, current.y] +
                    GetCellCost(next.x, next.y);

                if (newCost < cost[next.x, next.y])
                {
                    cost[next.x, next.y] = newCost;
                    previous[next.x, next.y] = current;

                    if (!inOpen[next.x, next.y])
                    {
                        open.Add(next);
                        inOpen[next.x, next.y] = true;
                    }
                }
            }
        }

        // No available path was found.
        return path;
    }

    // Draws the selected map over the ruins.
    void DrawMap()
    {
        if (mapRenderer == null || mapTexture == null)
            return;

        // Completely hides the overlay.
        if (currentMode == MapMode.Hidden)
        {
            mapRenderer.enabled = false;
            return;
        }

        mapRenderer.enabled = true;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Color color;

                if (currentMode == MapMode.Influence)
                {
                    // Blocked areas are dark red.
                    if (blocked[x, y])
                    {
                        color = new Color(
                            0.3f, 0.02f, 0.02f, 0.85f
                        );
                    }
                    else
                    {
                        float value = Mathf.Clamp01(
                            (danger[x, y] + extraDanger[x, y]) / 6f
                        );

                        // Green is safe and red is dangerous.
                        color = Color.Lerp(
                            new Color(0.1f, 0.75f, 0.2f, 0.25f),
                            new Color(0.95f, 0.1f, 0.05f, 0.75f),
                            value
                        );
                    }
                }
                else
                {
                    float value = Mathf.Clamp01(
                        heat[x, y] / Mathf.Max(0.01f, maxHeat)
                    );

                    // Blue is low activity and red is high activity.
                    color = Color.Lerp(
                        new Color(0.1f, 0.3f, 1f, 0.15f),
                        new Color(1f, 0.1f, 0.05f, 0.75f),
                        value
                    );
                }

                mapColors[y * columns + x] = color;
            }
        }

        mapTexture.SetPixels(mapColors);
        mapTexture.Apply(false);
    }

    void OnDestroy()
    {
        // Removes the generated map assets.
        if (mapSprite != null)
            Destroy(mapSprite);

        if (mapTexture != null)
            Destroy(mapTexture);
    }
}
