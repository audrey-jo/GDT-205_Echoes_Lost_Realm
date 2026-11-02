using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuardianSpiritAI : MonoBehaviour
{
    // ARIA

    [Header("Aria")]

    // Drag Aria here
    [SerializeField] private Transform aria;

    // If the spirit gets farther than this,
    // it will seek Aria
    [SerializeField] private float ariaFollowRange = 5f;


    // MOVEMENT

    [Header("Movement")]

    // Speed while seeking Aria
    [SerializeField] private float maxSpeed = 4f;

    // Speed while wandering
    [SerializeField] private float wanderSpeed = 2f;

    // Speed when running from enemies
    [SerializeField] private float enemyRunSpeed = 6f;

    // How fast the spirit changes speed
    [SerializeField] private float acceleration = 6f;


    // WANDER

    [Header("Wander Around Aria")]

    // Closest the spirit should wander to Aria
    [SerializeField] private float minWanderDistance = 1.5f;

    // Farthest the spirit can wander from Aria
    [SerializeField] private float wanderRadius = 3f;

    // How often the spirit picks a new wander point
    [SerializeField] private float wanderChangeInterval = 2f;

    // How close it gets before picking
    // another wander point
    [SerializeField] private float wanderPointReachedDistance = 0.35f;


    // ENEMY AVOIDANCE

    [Header("Enemy Avoidance")]

    // Layer used by enemies
    [SerializeField] private LayerMask enemyLayer;

    // How close an enemy needs to be
    // before the spirit runs away
    [SerializeField] private float enemyAvoidanceDistance = 2.5f;

    // How strongly it moves away from enemies
    [SerializeField] private float enemyAvoidanceStrength = 3f;


    // OBSTACLE AVOIDANCE

    [Header("Obstacle Avoidance")]

    // Layer used for walls, trees, rocks, etc.
    [SerializeField] private LayerMask obstacleLayer;

    // How far ahead it checks
    [SerializeField] private float obstacleDetectionDistance = 0.9f;

    // How strongly it avoids obstacles
    [SerializeField] private float obstacleAvoidanceStrength = 0.8f;

    // Angle of left and right checks
    [SerializeField] private float sideRayAngle = 25f;


    // OTHER VARIABLES

    private Rigidbody2D rb;

    private Collider2D spiritCollider;

    private Collider2D ariaCollider;

    private Vector2 wanderTarget;

    private float wanderTimer;

    private bool hasWanderTarget = false;

    [SerializeField] private string currentBehavior = "Seek Aria";


    // START

    private void Awake()
    {
        // Get spirit Rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Get spirit collider
        spiritCollider = GetComponent<Collider2D>();
    }


    private void Start()
    {
        // Get Aria's collider
        if (aria != null)
        {
            ariaCollider = aria.GetComponent<Collider2D>();

            // Stop the spirit from pushing Aria
            if (spiritCollider != null && ariaCollider != null)
            {
                Physics2D.IgnoreCollision(
                    spiritCollider,
                    ariaCollider
                );
            }
        }
    }


    // MAIN AI

    private void FixedUpdate()
    {
        // Stop if Aria is not connected
        if (aria == null)
        {
            rb.linearVelocity = Vector2.zero;

            currentBehavior = "No Aria";

            return;
        }


        // Find distance to Aria
        float distanceToAria = Vector2.Distance(
            rb.position,
            aria.position
        );


        // Check if an enemy is close
        bool enemyNearby = IsEnemyNearby();


        Vector2 steeringDirection;

        float desiredSpeed;


        // ENEMY FLEE

        // Enemy avoidance has highest priority
        if (enemyNearby)
        {
            steeringDirection = AvoidEnemies();

            // Run faster from enemies
            desiredSpeed = enemyRunSpeed;

            currentBehavior = "Flee Enemy";
        }


        // SEEK ARIA

        // Seek Aria if too far away
        else if (distanceToAria > ariaFollowRange)
        {
            steeringDirection = SeekAria();

            desiredSpeed = maxSpeed;

            currentBehavior = "Seek Aria";

            // Pick a new wander point later
            hasWanderTarget = false;
        }


        // WANDER AROUND ARIA

        else
        {
            steeringDirection = WanderAroundAria();

            desiredSpeed = wanderSpeed;

            currentBehavior = "Wander Around Aria";
        }


        // AVOID OBSTACLES

        steeringDirection += AvoidObstacles(
            steeringDirection
        );


        // NORMALIZE MOVEMENT

        if (steeringDirection.sqrMagnitude > 0.001f)
        {
            steeringDirection.Normalize();
        }


        // FINAL MOVEMENT

        Vector2 desiredVelocity =
            steeringDirection * desiredSpeed;


        // Smooth movement
        rb.linearVelocity = Vector2.MoveTowards(
            rb.linearVelocity,
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );
    }


    // SEEK ARIA

    private Vector2 SeekAria()
    {
        // Direction toward Aria
        Vector2 directionToAria =
            (Vector2)aria.position - rb.position;


        return directionToAria.normalized;
    }


    // WANDER AROUND ARIA

    private Vector2 WanderAroundAria()
    {
        // Count down timer
        wanderTimer -= Time.fixedDeltaTime;


        // Check if wander point was reached
        bool reachedWanderTarget =
            hasWanderTarget &&
            Vector2.Distance(
                rb.position,
                wanderTarget
            ) <= wanderPointReachedDistance;


        // Check if wander point became
        // too far from Aria
        bool wanderPointTooFar =
            hasWanderTarget &&
            Vector2.Distance(
                wanderTarget,
                aria.position
            ) > wanderRadius;


        // Check if wander point became
        // too close to Aria
        bool wanderPointTooClose =
            hasWanderTarget &&
            Vector2.Distance(
                wanderTarget,
                aria.position
            ) < minWanderDistance;


        // Pick a new wander point
        if (!hasWanderTarget ||
            wanderTimer <= 0f ||
            reachedWanderTarget ||
            wanderPointTooFar ||
            wanderPointTooClose)
        {
            ChooseNewWanderTarget();
        }


        // Direction toward wander point
        Vector2 directionToTarget =
            wanderTarget - rb.position;


        return directionToTarget.normalized;
    }


    // CHOOSE WANDER POINT

    private void ChooseNewWanderTarget()
    {
        // Pick a random direction
        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;


        // Pick a distance between the
        // minimum and maximum wander range
        float randomDistance =
            Random.Range(
                minWanderDistance,
                wanderRadius
            );


        // Create wander point around Aria
        wanderTarget =
            (Vector2)aria.position +
            randomDirection * randomDistance;


        // Reset timer
        wanderTimer = wanderChangeInterval;


        hasWanderTarget = true;
    }


    // CHECK FOR ENEMIES

    private bool IsEnemyNearby()
    {
        // Look for an enemy inside the range
        Collider2D enemy =
            Physics2D.OverlapCircle(
                rb.position,
                enemyAvoidanceDistance,
                enemyLayer
            );


        // True if an enemy was found
        return enemy != null;
    }


    // AVOID ENEMIES

    private Vector2 AvoidEnemies()
    {
        // Start with no avoidance
        Vector2 avoidance = Vector2.zero;


        // Find all nearby enemies
        Collider2D[] nearbyEnemies =
            Physics2D.OverlapCircleAll(
                rb.position,
                enemyAvoidanceDistance,
                enemyLayer
            );


        // Check each enemy
        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy == null)
            {
                continue;
            }


            // Enemy position
            Vector2 enemyPosition =
                enemy.transform.position;


            // Direction away from enemy
            Vector2 awayFromEnemy =
                rb.position - enemyPosition;


            // Distance from enemy
            float distance =
                awayFromEnemy.magnitude;


            if (distance > 0.001f)
            {
                // Closer enemy means
                // stronger movement away
                float strength =
                    1f - Mathf.Clamp01(
                        distance / enemyAvoidanceDistance
                    );


                avoidance +=
                    awayFromEnemy.normalized
                    * strength
                    * enemyAvoidanceStrength;
            }
        }


        // If multiple enemies are around,
        // combine the directions
        return avoidance.normalized;
    }


    // AVOID OBSTACLES

    private Vector2 AvoidObstacles(
        Vector2 movementDirection
    )
    {
        // Do nothing if not moving
        if (movementDirection.sqrMagnitude < 0.01f)
        {
            return Vector2.zero;
        }


        // Current movement direction
        Vector2 forward =
            movementDirection.normalized;


        // Left check
        Vector2 leftDirection =
            RotateVector(
                forward,
                sideRayAngle
            );


        // Right check
        Vector2 rightDirection =
            RotateVector(
                forward,
                -sideRayAngle
            );


        // CHECK FORWARD

        RaycastHit2D centerHit =
            Physics2D.Raycast(
                rb.position,
                forward,
                obstacleDetectionDistance,
                obstacleLayer
            );


        // CHECK LEFT

        RaycastHit2D leftHit =
            Physics2D.Raycast(
                rb.position,
                leftDirection,
                obstacleDetectionDistance,
                obstacleLayer
            );


        // CHECK RIGHT

        RaycastHit2D rightHit =
            Physics2D.Raycast(
                rb.position,
                rightDirection,
                obstacleDetectionDistance,
                obstacleLayer
            );


        Vector2 avoidance =
            Vector2.zero;


        // OBSTACLE IN FRONT

        if (centerHit.collider != null)
        {
            avoidance +=
                centerHit.normal
                * obstacleAvoidanceStrength;
        }


        // OBSTACLE ON LEFT

        if (leftHit.collider != null)
        {
            // Turn right
            avoidance +=
                RotateVector(
                    forward,
                    -90f
                )
                * obstacleAvoidanceStrength;
        }


        // OBSTACLE ON RIGHT

        if (rightHit.collider != null)
        {
            // Turn left
            avoidance +=
                RotateVector(
                    forward,
                    90f
                )
                * obstacleAvoidanceStrength;
        }


        return avoidance;
    }


    // ROTATE DIRECTION

    private Vector2 RotateVector(
        Vector2 vector,
        float degrees
    )
    {
        // Change degrees to radians
        float radians =
            degrees * Mathf.Deg2Rad;


        float cos =
            Mathf.Cos(radians);


        float sin =
            Mathf.Sin(radians);


        // Return rotated direction
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }


    // DEBUG

    private void OnDrawGizmosSelected()
    {
        // ARIA RANGES

        if (aria != null)
        {
            // Follow range
            Gizmos.DrawWireSphere(
                aria.position,
                ariaFollowRange
            );


            // Maximum wander range
            Gizmos.DrawWireSphere(
                aria.position,
                wanderRadius
            );


            // Minimum wander range
            Gizmos.DrawWireSphere(
                aria.position,
                minWanderDistance
            );


            // Current wander point
            if (Application.isPlaying &&
                hasWanderTarget)
            {
                Gizmos.DrawWireSphere(
                    wanderTarget,
                    0.2f
                );


                Gizmos.DrawLine(
                    transform.position,
                    wanderTarget
                );
            }
        }


        // ENEMY RANGE

        Gizmos.DrawWireSphere(
            transform.position,
            enemyAvoidanceDistance
        );
    }
}