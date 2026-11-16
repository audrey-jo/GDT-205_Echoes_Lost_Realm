using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuardianStatues : MonoBehaviour
{
    // These references can be set in the Inspector, or found automatically when play starts.
    [Header("References")]
    [SerializeField] private Transform aria;
    [SerializeField] private Collider2D gardenBounds;
    [SerializeField] private GardenPortal portal;

    // The health and movement values can be adjusted for each guardian.
    [Header("Health")]
    [SerializeField, Min(1)] private int maximumHealth = 10;

    [Header("Movement")]
    [SerializeField, Min(0f)] private float passiveSpeed = 1f;
    [SerializeField, Min(0f)] private float aggressiveSpeed = 2.5f;
    [SerializeField, Min(0f)] private float fleeingSpeed = 4f;
    [SerializeField, Min(0f)] private float patrolRadius = 2f;
    [SerializeField, Min(0.1f)] private float maxChaseDistance = 12f;
    [SerializeField, Min(0.1f)] private float safeDistance = 3f;
    [SerializeField, Min(0.1f)] private float attackRange = 1.5f;
    [SerializeField, Min(0.1f)] private float attackCooldown = 1.5f;
    [SerializeField, Min(0.1f)] private float patrolPointInterval = 2.5f;
    [SerializeField, Min(0f)] private float decisionResponse = 1.5f;

    [Header("Obstacle Avoidance")]
    [SerializeField] private string obstacleTag = "Obstacle";
    [SerializeField, Min(0.1f)] private float obstacleDetectionDistance = 1.2f;
    [SerializeField, Min(0f)] private float obstacleAvoidanceStrength = 2f;
    [SerializeField, Range(0f, 80f)] private float obstacleSideAngle = 35f;

    // These values are shown in the Inspector while the game is running.
    [Header("Runtime (read only)")]
    [SerializeField] private int currentHealth;
    [SerializeField, Range(0f, 1f)] private float aggression;
    [SerializeField, Range(0f, 1f)] private float fleeStrength;
    [SerializeField, Range(0f, 1f)] private float perceivedThreat;

    private Rigidbody2D body;
    private Collider2D bodyCollider;
    private SpriteRenderer spriteRenderer;
    private AriaCollisions ariaHealth;
    private Vector2 startingPosition;
    private Vector2 patrolTarget;
    private float patrolTimer;
    private float attackTimer;
    private float previousAriaDistance;
    private bool defeated;
    private Color originalColor;

    // Other scripts use these to check the guardian's health and defeat status.
    public int CurrentHealth => currentHealth;
    public int MaximumHealth => maximumHealth;
    public bool IsDefeated => defeated;

    private void Awake()
    {
        // Get the components this script needs and start at full health.
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maximumHealth = Mathf.Max(1, maximumHealth);
        currentHealth = maximumHealth;
        startingPosition = body.position;
        patrolTarget = startingPosition;

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // A bounds collider is needed so the guardian can stay inside the garden.
        if (gardenBounds == null)
            Debug.LogError($"{nameof(GuardianStatues)} requires a garden bounds collider.", this);
    }

    private void Start()
    {
        // Find Aria and the portal if they were not assigned in the Inspector.
        if (aria == null)
        {
            AriaCollisions foundAria = FindAnyObjectByType<AriaCollisions>();
            if (foundAria != null)
                aria = foundAria.transform;
        }

        if (aria != null)
            ariaHealth = aria.GetComponentInParent<AriaCollisions>();

        if (portal == null)
            portal = FindAnyObjectByType<GardenPortal>();

        if (portal != null)
            portal.RegisterGuardian(this);

        // Pick the first patrol destination and avoid physically pushing Aria.
        previousAriaDistance = GetDistanceToAria();
        ChoosePatrolTarget();
        Collider2D ariaCollider = aria != null ? aria.GetComponent<Collider2D>() : null;
        if (ariaCollider == null && aria != null)
            ariaCollider = aria.GetComponentInChildren<Collider2D>();
        if (bodyCollider != null && ariaCollider != null)
            Physics2D.IgnoreCollision(bodyCollider, ariaCollider, true);
    }

    private void FixedUpdate()
    {
        // A defeated guardian should not make any more decisions or move.
        if (defeated)
            return;

        if (attackTimer > 0f)
            attackTimer -= Time.fixedDeltaTime;

        // Check Aria's distance and update the guardian's changing threat estimate.
        float distance = GetDistanceToAria();
        bool ariaNearby = aria != null && distance <= maxChaseDistance;
        UpdateThreat(distance, ariaNearby);

        // Fuzzy logic uses values from 0 to 1 instead of fixed behavior cutoffs.
        float healthPercent = (float)currentHealth / maximumHealth;
        float normalizedDistance = aria == null ? 1f : Mathf.Clamp01(distance / maxChaseDistance);
        GuardianFuzzyDecision.Evaluate(
            healthPercent,
            normalizedDistance,
            perceivedThreat,
            out float targetAggression,
            out float targetFleeStrength);

        // Smooth the fuzzy outputs so behavior changes gradually.
        float blend = 1f - Mathf.Exp(-decisionResponse * Time.fixedDeltaTime);
        aggression = Mathf.Lerp(aggression, targetAggression, blend);
        fleeStrength = Mathf.Lerp(fleeStrength, targetFleeStrength, blend);

        // Mix passive, aggressive, and fleeing movement using the fuzzy outputs.
        Vector2 direction = GetMovementDirection(distance);
        direction = AvoidObstacles(direction);
        float speed = Mathf.Lerp(passiveSpeed, aggressiveSpeed, aggression);
        speed = Mathf.Lerp(speed, fleeingSpeed, fleeStrength);

        Vector2 nextPosition = body.position + direction * speed * Time.fixedDeltaTime;
        nextPosition = ClampToGarden(nextPosition);
        body.MovePosition(nextPosition);

        // Attack only when close enough and the fuzzy aggression is high enough.
        float nearMembership = GuardianFuzzyDecision.NearMembership(normalizedDistance);
        float attackDrive = aggression * nearMembership;
        if (ariaHealth != null && distance <= attackRange && attackDrive >= 0.35f && attackTimer <= 0f)
        {
            ariaHealth.TakeProjectileHit();
            attackTimer = attackCooldown;
        }

        previousAriaDistance = distance;
    }

    public void TakeDamage(int damage)
    {
        // Ignore invalid hits and any damage after the guardian is defeated.
        if (damage <= 0 || defeated)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        RegisterAriaAttack();

        if (currentHealth == 0)
        {
            // Stop physics and show that this guardian has been defeated.
            defeated = true;
            body.linearVelocity = Vector2.zero;
            body.simulated = false;
            if (bodyCollider != null)
                bodyCollider.enabled = false;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.gray;

            if (portal != null)
                portal.NotifyGuardianDefeated(this);
        }
    }

    public void RegisterAriaAttack()
    {
        // Repeated attacks make the guardian treat Aria as a bigger threat.
        perceivedThreat = Mathf.Clamp01(perceivedThreat + 0.3f);
    }

    private void UpdateThreat(float distance, bool ariaNearby)
    {
        // Retreating lowers threat, while approaching raises it over time.
        if (aria == null)
            return;

        bool retreating = distance > previousAriaDistance + 0.01f;
        float near = GuardianFuzzyDecision.NearMembership(Mathf.Clamp01(distance / maxChaseDistance));
        float threatTarget = perceivedThreat;

        if (retreating)
            threatTarget = Mathf.Max(0f, perceivedThreat - 0.25f);
        else if (ariaNearby)
            threatTarget = Mathf.Max(perceivedThreat, 0.15f + near * 0.35f);

        perceivedThreat = Mathf.MoveTowards(
            perceivedThreat,
            threatTarget,
            (retreating ? 0.2f : 0.12f) * Time.fixedDeltaTime);
    }

    private Vector2 GetMovementDirection(float distance)
    {
        // Choose another patrol point when the current one is reached or times out.
        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f || Vector2.Distance(body.position, patrolTarget) < 0.2f)
            ChoosePatrolTarget();

        Vector2 patrolDirection = patrolTarget - body.position;
        Vector2 towardAria = aria != null ? (Vector2)aria.position - body.position : Vector2.zero;

        if (fleeStrength > 0.05f && aria != null && distance < safeDistance)
        {
            // Blend walking toward the patrol point with moving away from Aria.
            Vector2 awayFromAria = -towardAria.normalized;
            return Vector2.Lerp(patrolDirection.normalized, awayFromAria, fleeStrength).normalized;
        }

        // When Aria is not far away, aggression pulls the guardian toward her.
        float chaseInfluence = 1f - GuardianFuzzyDecision.FarMembership(
            Mathf.Clamp01(distance / maxChaseDistance));
        Vector2 direction = Vector2.Lerp(
            patrolDirection.normalized,
            towardAria.normalized,
            aggression * chaseInfluence);
        return direction.normalized;
    }

    private Vector2 AvoidObstacles(Vector2 movementDirection)
    {
        if (movementDirection.sqrMagnitude < 0.001f)
            return Vector2.zero;

        Vector2 forward = movementDirection.normalized;
        Vector2 left = RotateDirection(forward, obstacleSideAngle);
        Vector2 right = RotateDirection(forward, -obstacleSideAngle);
        Vector2 avoidance = Vector2.zero;

        RaycastHit2D centerHit = FindObstacleHit(forward);
        RaycastHit2D leftHit = FindObstacleHit(left);
        RaycastHit2D rightHit = FindObstacleHit(right);

        if (centerHit.collider != null)
            avoidance += centerHit.normal * GetAvoidanceAmount(centerHit);

        // A hit on one side encourages the guardian to turn toward the other side.
        if (leftHit.collider != null)
            avoidance += RotateDirection(forward, -90f) * GetAvoidanceAmount(leftHit);
        if (rightHit.collider != null)
            avoidance += RotateDirection(forward, 90f) * GetAvoidanceAmount(rightHit);

        Vector2 adjustedDirection = forward + avoidance * obstacleAvoidanceStrength;
        return adjustedDirection.sqrMagnitude > 0.001f ? adjustedDirection.normalized : forward;
    }

    private RaycastHit2D FindObstacleHit(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(body.position, direction, obstacleDetectionDistance);
        RaycastHit2D closestHit = default;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || hit.collider == bodyCollider || !HasObstacleTag(hit.collider.transform))
                continue;

            if (closestHit.collider == null || hit.distance < closestHit.distance)
                closestHit = hit;
        }

        return closestHit;
    }

    private bool HasObstacleTag(Transform obstacle)
    {
        // Check parent objects too, in case the tagged object holds a child collider.
        while (obstacle != null)
        {
            if (obstacle.tag == obstacleTag)
                return true;
            obstacle = obstacle.parent;
        }

        return false;
    }

    private float GetAvoidanceAmount(RaycastHit2D hit)
    {
        return 1f - Mathf.Clamp01(hit.distance / obstacleDetectionDistance);
    }

    private static Vector2 RotateDirection(Vector2 direction, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cosine = Mathf.Cos(radians);
        float sine = Mathf.Sin(radians);
        return new Vector2(
            direction.x * cosine - direction.y * sine,
            direction.x * sine + direction.y * cosine);
    }

    private void ChoosePatrolTarget()
    {
        // Pick a random point near the guardian's starting position.
        Vector2 offset = Random.insideUnitCircle * patrolRadius;
        patrolTarget = ClampToGarden(startingPosition + offset);
        patrolTimer = patrolPointInterval;
    }

    private Vector2 ClampToGarden(Vector2 position)
    {
        // Clamp the guardian's position so its collider stays inside the garden bounds.
        if (gardenBounds == null)
            return startingPosition;

        Bounds bounds = gardenBounds.bounds;
        Vector2 extents = bodyCollider != null ? bodyCollider.bounds.extents : Vector2.zero;
        float minX = bounds.min.x + extents.x;
        float maxX = bounds.max.x - extents.x;
        float minY = bounds.min.y + extents.y;
        float maxY = bounds.max.y - extents.y;

        if (minX > maxX)
            minX = maxX = bounds.center.x;
        if (minY > maxY)
            minY = maxY = bounds.center.y;

        return new Vector2(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY));
    }

    private float GetDistanceToAria()
    {
        // Treat missing Aria as far away so the guardian does not chase blindly.
        return aria != null ? Vector2.Distance(body.position, aria.position) : maxChaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
        // Show the adjustable movement and behavior ranges when this guardian is selected.
        Vector3 patrolCenter = Application.isPlaying ? (Vector3)startingPosition : transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, safeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxChaseDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        if (gardenBounds != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gardenBounds.bounds.center, gardenBounds.bounds.size);
        }
    }
}

internal static class GuardianFuzzyDecision
{
    // The output sets describe how much the guardian should act in each way.
    private enum Set
    {
        Low,
        Medium,
        High
    }

    public static void Evaluate(
        float health,
        float distance,
        float threat,
        out float aggression,
        out float fleeing)
    {
        // Health, distance, and threat can belong partly to more than one set.
        float lowHealth = Trapezoid(health, 0f, 0f, 0.15f, 0.4f);
        float mediumHealth = Triangle(health, 0.2f, 0.5f, 0.8f);
        float highHealth = Trapezoid(health, 0.6f, 0.75f, 1f, 1f);

        float near = NearMembership(distance);
        float mediumDistance = Triangle(distance, 0.18f, 0.5f, 0.82f);
        float far = FarMembership(distance);

        float lowThreat = Trapezoid(threat, 0f, 0f, 0.25f, 0.5f);
        float mediumThreat = Triangle(threat, 0.2f, 0.5f, 0.8f);
        float highThreat = Trapezoid(threat, 0.5f, 0.75f, 1f, 1f);

        // Each rule adds a strength to low, medium, or high behavior.
        float[] aggressionRules = new float[3];
        float[] fleeRules = new float[3];

        // Healthy guardians mostly stay passive, but can get aggressive when threatened nearby.
        Add(aggressionRules, Set.Low, And(highHealth, lowThreat));
        Add(aggressionRules, Set.Medium, And(highHealth, mediumThreat, near));
        Add(aggressionRules, Set.High, And(highHealth, highThreat, near));
        Add(aggressionRules, Set.Medium, And(mediumHealth, near, mediumThreat));
        Add(aggressionRules, Set.Medium, And(mediumHealth, near, lowThreat));
        Add(aggressionRules, Set.High, And(mediumHealth, near, highThreat));
        Add(aggressionRules, Set.Low, And(mediumHealth, far, lowThreat));
        Add(aggressionRules, Set.Medium, And(mediumHealth, mediumDistance, highThreat));
        Add(aggressionRules, Set.Low, And(lowHealth, near, lowThreat));
        Add(aggressionRules, Set.Low, And(lowHealth, far, lowThreat));

        // Weak guardians are more likely to flee, especially when Aria is close.
        Add(fleeRules, Set.High, And(lowHealth, near));
        Add(fleeRules, Set.Medium, And(lowHealth, mediumDistance));
        Add(fleeRules, Set.High, And(lowHealth, highThreat));
        Add(fleeRules, Set.Medium, And(mediumHealth, near, highThreat));
        Add(fleeRules, Set.Low, And(highHealth, lowThreat));

        // Turn the fuzzy rule strengths into smooth numbers between 0 and 1.
        aggression = Centroid(aggressionRules);
        fleeing = Centroid(fleeRules);
    }

    public static float NearMembership(float distance)
    {
        // Near is strongest close to Aria and fades as distance increases.
        return Trapezoid(distance, 0f, 0f, 0.18f, 0.42f);
    }

    public static float FarMembership(float distance)
    {
        // Far is strongest when Aria is near the edge of the chase range.
        return Trapezoid(distance, 0.58f, 0.82f, 1f, 1f);
    }

    private static float And(float a, float b) => Mathf.Min(a, b);
    private static float And(float a, float b, float c) => Mathf.Min(a, Mathf.Min(b, c));

    private static void Add(float[] rules, Set set, float strength)
    {
        // Keep the strongest matching rule for this output set.
        int index = (int)set;
        rules[index] = Mathf.Max(rules[index], strength);
    }

    private static float Centroid(float[] rules)
    {
        // Sample the output shape and find its weighted average (the centroid).
        float weightedTotal = 0f;
        float membershipTotal = 0f;
        const int sampleCount = 100;

        for (int i = 0; i <= sampleCount; i++)
        {
            float value = i / (float)sampleCount;
            float membership = Mathf.Max(
                Mathf.Min(rules[(int)Set.Low], Trapezoid(value, 0f, 0f, 0f, 0.01f)),
                Mathf.Max(
                    Mathf.Min(rules[(int)Set.Medium], Triangle(value, 0.2f, 0.5f, 0.8f)),
                    Mathf.Min(rules[(int)Set.High], Trapezoid(value, 0.58f, 0.8f, 1f, 1f))));

            weightedTotal += value * membership;
            membershipTotal += membership;
        }

        if (membershipTotal <= 0f)
            return 0f;

        return weightedTotal / membershipTotal;
    }

    private static float Triangle(float value, float start, float peak, float end)
    {
        // Triangle membership rises to the peak, then falls back to zero.
        if (value <= start || value >= end)
            return 0f;
        return value <= peak
            ? Mathf.InverseLerp(start, peak, value)
            : Mathf.InverseLerp(end, peak, value);
    }

    private static float Trapezoid(float value, float start, float innerStart, float innerEnd, float end)
    {
        // Trapezoid membership rises, stays at 1 in the middle, then falls.
        if (value < start || value > end)
            return 0f;
        if (value >= innerStart && value <= innerEnd)
            return 1f;
        if (value < innerStart)
            return innerStart > start ? Mathf.InverseLerp(start, innerStart, value) : 1f;
        return end > innerEnd ? Mathf.InverseLerp(end, innerEnd, value) : 1f;
    }
}
