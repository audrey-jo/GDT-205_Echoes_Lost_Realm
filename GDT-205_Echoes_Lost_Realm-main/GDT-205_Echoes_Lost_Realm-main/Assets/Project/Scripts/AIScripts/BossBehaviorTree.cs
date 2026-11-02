using System;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviorTree : MonoBehaviour
{
    private enum NodeResult
    {
        Success,
        Failure,
        Running
    }

    private abstract class BehaviorNode
    {
        public abstract NodeResult Tick();
    }

    private sealed class SelectorNode : BehaviorNode
    {
        private readonly BehaviorNode[] children;

        public SelectorNode(params BehaviorNode[] children)
        {
            this.children = children;
        }

        public override NodeResult Tick()
        {
            foreach (BehaviorNode child in children)
            {
                NodeResult result = child.Tick();
                if (result != NodeResult.Failure) return result;
            }

            return NodeResult.Failure;
        }
    }

    private sealed class SequenceNode : BehaviorNode
    {
        private readonly BehaviorNode[] children;

        public SequenceNode(params BehaviorNode[] children)
        {
            this.children = children;
        }

        public override NodeResult Tick()
        {
            foreach (BehaviorNode child in children)
            {
                NodeResult result = child.Tick();
                if (result != NodeResult.Success) return result;
            }

            return NodeResult.Success;
        }
    }

    private sealed class ConditionNode : BehaviorNode
    {
        private readonly Func<bool> condition;

        public ConditionNode(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override NodeResult Tick()
        {
            return condition() ? NodeResult.Success : NodeResult.Failure;
        }
    }

    private sealed class ActionNode : BehaviorNode
    {
        private readonly Func<NodeResult> action;

        public ActionNode(Func<NodeResult> action)
        {
            this.action = action;
        }

        public override NodeResult Tick()
        {
            return action();
        }
    }

    private sealed class SummonedMinion
    {
        public Minion minion;
        public int spawnPointIndex;

        public SummonedMinion(Minion minion, int spawnPointIndex)
        {
            this.minion = minion;
            this.spawnPointIndex = spawnPointIndex;
        }
    }

    [Header("References")]
    [SerializeField] private Transform aria;
    [SerializeField] private Minion minionPrefab;
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    [SerializeField] private Transform minionParent;
    [SerializeField] private Transform retreatPoint;

    [Header("Detection")]
    [SerializeField, Min(0f)] private float detectionRadius = 10f;
    [SerializeField, Min(0f)] private float dangerRadius = 4f;
    [SerializeField] private bool removeWhenAriaLeaves;

    [Header("Summoning")]
    [SerializeField, Min(1)] private int maximumMinions = 4;
    [SerializeField, Min(0f)] private float normalCooldown = 6f;
    [SerializeField, Min(0f)] private float dangerCooldown = 3f;
    [SerializeField, Min(0f)] private float spawnSpacing = 1.25f;

    [Header("Phase 2 Summoning")]
    [SerializeField, Min(1)] private int phaseTwoMaximumMinions = 8;
    [SerializeField, Min(1)] private int phaseTwoSummonMultiplier = 2;

    [Header("Adaptive Cooldown")]
    [SerializeField, Min(0f)] private float cooldownReduction = 0.5f;
    [SerializeField, Min(0f)] private float minimumCooldown = 1.5f;

    [Header("Behavior Tree")]
    [SerializeField, Min(0.05f)] private float evaluationInterval = 0.2f;

    [Header("Debug")]
    [SerializeField] private string currentDecision = "Starting";
    [SerializeField] private int activeSummonedMinionCount;
    [SerializeField] private int encounterCount;

    private readonly List<SummonedMinion> summonedMinions = new List<SummonedMinion>();
    private BehaviorNode behaviorTree;
    private BossHealth bossHealth;
    private float nextEvaluationTime;
    private float nextSummonTime;
    private bool ariaWasInDetectionRange;
    private bool ariaWasInDangerRange;
    private bool dangerEntryPending;
    private bool missingSetupWarningShown;

    public string CurrentDecision => currentDecision;
    public int ActiveSummonedMinionCount => activeSummonedMinionCount;
    public int EncounterCount => encounterCount;

    private void Awake()
    {
        if (minionParent == null) minionParent = transform;
        maximumMinions = Mathf.Max(1, maximumMinions);
        phaseTwoMaximumMinions = Mathf.Max(1, phaseTwoMaximumMinions);
        phaseTwoSummonMultiplier = Mathf.Max(1, phaseTwoSummonMultiplier);
        bossHealth = GetComponent<BossHealth>();
        behaviorTree = BuildBehaviorTree();
    }

    private void Start()
    {
        if (aria == null || minionPrefab == null || !HasAnySpawnPoint())
        {
            Debug.LogWarning(
                "BossBehaviorTree needs Aria, a Minion prefab, and at least one spawn point assigned.",
                this);
            missingSetupWarningShown = true;
        }
        else if (minionPrefab.obstacleLayer.value == 0)
        {
            Debug.LogWarning(
                "The Minion prefab has no obstacle layer mask; spawn points cannot be checked against walls.",
                this);
        }
    }

    private void Update()
    {
        if (Time.time < nextEvaluationTime) return;

        nextEvaluationTime = Time.time + Mathf.Max(0.05f, evaluationInterval);
        EvaluateBehaviorTree();
    }

    private BehaviorNode BuildBehaviorTree()
    {
        return new SelectorNode(
            new SequenceNode(
                new ConditionNode(() => !IsAriaInDetectionRange()),
                new ActionNode(SetIdleDecision)),
            new SequenceNode(
                new ConditionNode(() => IsAriaInDetectionRange() &&
                                        activeSummonedMinionCount >= GetCurrentMinionLimit()),
                new ActionNode(SetMinionLimitDecision)),
            new SequenceNode(
                new ConditionNode(() => IsAriaInDangerRange() && CanReinforce()),
                new ActionNode(() => SummonMinions(2, true))),
            new SequenceNode(
                new ConditionNode(() => IsAriaInDetectionRange() &&
                                        !IsAriaInDangerRange() &&
                                        CanSummon()),
                new ActionNode(() => SummonMinions(1, false))),
            new SequenceNode(
                new ConditionNode(() => IsAriaInDetectionRange() &&
                                        activeSummonedMinionCount < GetCurrentMinionLimit() &&
                                        !CanReinforce() &&
                                        !CanSummon()),
                new ActionNode(SetCooldownDecision)),
            new SequenceNode(
                new ConditionNode(() => IsAriaInDetectionRange()),
                new ActionNode(SetNoSpawnPointDecision)));
    }

    private void EvaluateBehaviorTree()
    {
        PruneInactiveMinions();

        if (aria == null)
        {
            currentDecision = "Missing Aria reference";
            if (!missingSetupWarningShown)
            {
                Debug.LogWarning("Assign Aria to the BossBehaviorTree component.", this);
                missingSetupWarningShown = true;
            }
            return;
        }

        float distanceSquared = ((Vector2)aria.position - (Vector2)transform.position).sqrMagnitude;
        bool isInDetectionRange = distanceSquared <= detectionRadius * detectionRadius;
        bool isInDangerRange = distanceSquared <= dangerRadius * dangerRadius;

        if (isInDetectionRange && !ariaWasInDetectionRange)
        {
            encounterCount++;
        }

        if (isInDangerRange && !ariaWasInDangerRange)
        {
            // A new danger-range entry can trigger one immediate reinforcement.
            dangerEntryPending = true;
        }

        if (!isInDetectionRange && ariaWasInDetectionRange && removeWhenAriaLeaves)
        {
            RemoveSummonedMinions();
        }

        if (!isInDangerRange)
        {
            dangerEntryPending = false;
        }

        ariaWasInDetectionRange = isInDetectionRange;
        ariaWasInDangerRange = isInDangerRange;

        behaviorTree.Tick();
    }

    private bool IsAriaInDetectionRange()
    {
        if (aria == null) return false;
        float distanceSquared = ((Vector2)aria.position - (Vector2)transform.position).sqrMagnitude;
        return distanceSquared <= detectionRadius * detectionRadius;
    }

    private bool IsAriaInDangerRange()
    {
        if (aria == null) return false;
        float distanceSquared = ((Vector2)aria.position - (Vector2)transform.position).sqrMagnitude;
        return distanceSquared <= dangerRadius * dangerRadius;
    }

    private bool CanSummon()
    {
        return activeSummonedMinionCount < GetCurrentMinionLimit() && Time.time >= nextSummonTime;
    }

    private bool CanReinforce()
    {
        return activeSummonedMinionCount < GetCurrentMinionLimit() &&
               (dangerEntryPending || Time.time >= nextSummonTime);
    }

    private int GetCurrentMinionLimit()
    {
        return bossHealth != null && bossHealth.IsSecondPhase
            ? phaseTwoMaximumMinions
            : maximumMinions;
    }

    private NodeResult SetIdleDecision()
    {
        currentDecision = "Idle";
        return NodeResult.Success;
    }

    private NodeResult SetMinionLimitDecision()
    {
        currentDecision = "Minion limit reached";
        return NodeResult.Success;
    }

    private NodeResult SetCooldownDecision()
    {
        currentDecision = "Cooldown";
        return NodeResult.Success;
    }

    private NodeResult SetNoSpawnPointDecision()
    {
        currentDecision = "No clear spawn point";
        return NodeResult.Success;
    }

    private NodeResult SummonMinions(int requestedCount, bool dangerSummon)
    {
        if (bossHealth != null && bossHealth.IsSecondPhase)
        {
            requestedCount *= phaseTwoSummonMultiplier;
        }

        int summonedCount = 0;
        for (int i = 0; i < requestedCount && activeSummonedMinionCount < GetCurrentMinionLimit(); i++)
        {
            if (!TrySpawnMinion()) break;
            summonedCount++;
        }

        if (summonedCount == 0)
        {
            currentDecision = "No clear spawn point";
            return NodeResult.Failure;
        }

        dangerEntryPending = false;
        float baseCooldown = dangerSummon ? dangerCooldown : normalCooldown;
        nextSummonTime = Time.time + GetAdaptiveCooldown(baseCooldown);
        currentDecision = dangerSummon ? "Reinforcements" : "Normal summoning";
        return NodeResult.Success;
    }

    private bool TrySpawnMinion()
    {
        if (minionPrefab == null || aria == null || !HasAnySpawnPoint())
        {
            if (!missingSetupWarningShown)
            {
                Debug.LogWarning(
                    "Assign the Minion prefab, Aria, and spawn points before the boss can summon.",
                    this);
                missingSetupWarningShown = true;
            }
            return false;
        }

        if (minionPrefab.GetComponent<Minion>() == null)
        {
            Debug.LogError("The assigned Minion prefab does not have a Minion component.", this);
            return false;
        }

        Vector3 spawnPosition;
        int spawnPointIndex;
        if (!TryFindSpawnPosition(out spawnPosition, out spawnPointIndex))
        {
            return false;
        }

        Transform parent = minionParent != null ? minionParent : transform;
        Minion spawnedMinion = Instantiate(minionPrefab, spawnPosition, Quaternion.identity, parent);

        // Assign targets before the instantiated Minion receives its first Start call.
        spawnedMinion.wanderTarget = transform;
        spawnedMinion.chaseTarget = aria;
        spawnedMinion.retreatTarget = retreatPoint != null ? retreatPoint : transform;
        spawnedMinion.spiritCollider = spawnedMinion.GetComponent<Collider2D>();
        spawnedMinion.ariaCollider = aria.GetComponent<Collider2D>();

        summonedMinions.Add(new SummonedMinion(spawnedMinion, spawnPointIndex));
        activeSummonedMinionCount++;
        return true;
    }

    private bool HasAnySpawnPoint()
    {
        if (spawnPoints == null) return false;

        int pointCount = Mathf.Min(spawnPoints.Length, 3);
        for (int i = 0; i < pointCount; i++)
        {
            if (spawnPoints[i] != null) return true;
        }

        return false;
    }

    private bool TryFindSpawnPosition(out Vector3 position, out int spawnPointIndex)
    {
        position = Vector3.zero;
        spawnPointIndex = -1;

        int pointCount = Mathf.Min(spawnPoints.Length, 3);
        int leastOccupied = int.MaxValue;
        int currentMinionLimit = GetCurrentMinionLimit();

        for (int i = 0; i < pointCount; i++)
        {
            if (spawnPoints[i] == null) continue;

            int occupiedCount = GetSpawnPointOccupancy(i);
            if (occupiedCount < leastOccupied) leastOccupied = occupiedCount;
        }

        if (leastOccupied == int.MaxValue) return false;

        for (int occupancy = leastOccupied; occupancy < currentMinionLimit; occupancy++)
        {
            for (int i = 0; i < pointCount; i++)
            {
                Transform spawnPoint = spawnPoints[i];
                if (spawnPoint == null || GetSpawnPointOccupancy(i) != occupancy) continue;

                for (int offsetIndex = occupancy; offsetIndex < currentMinionLimit; offsetIndex++)
                {
                    Vector3 candidate = GetSpawnPosition(spawnPoint.position, i, offsetIndex);
                    if (IsSpawnPositionClear(candidate))
                    {
                        position = candidate;
                        spawnPointIndex = i;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private int GetSpawnPointOccupancy(int spawnPointIndex)
    {
        int count = 0;
        foreach (SummonedMinion trackedMinion in summonedMinions)
        {
            if (trackedMinion.spawnPointIndex == spawnPointIndex) count++;
        }

        return count;
    }

    private Vector3 GetSpawnPosition(Vector3 point, int spawnPointIndex, int offsetIndex)
    {
        if (offsetIndex == 0) return point;

        float angle = (spawnPointIndex * 120f + offsetIndex * 137.5f) * Mathf.Deg2Rad;
        float distance = spawnSpacing * Mathf.Sqrt(offsetIndex);
        return point + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
    }

    private bool IsSpawnPositionClear(Vector3 position)
    {
        Collider2D prefabCollider = minionPrefab.GetComponent<Collider2D>();
        Vector2 size = prefabCollider != null ? prefabCollider.bounds.size : Vector2.one;
        size = new Vector2(Mathf.Max(size.x, 0.5f), Mathf.Max(size.y, 0.5f));

        LayerMask blockingLayers = minionPrefab.obstacleLayer;
        if (blockingLayers.value == 0) return true;

        return Physics2D.OverlapBox(position, size, 0f, blockingLayers) == null;
    }

    private float GetAdaptiveCooldown(float baseCooldown)
    {
        float reductions = Mathf.Max(0, encounterCount - 1) * cooldownReduction;
        return Mathf.Max(minimumCooldown, baseCooldown - reductions);
    }

    private void PruneInactiveMinions()
    {
        // Defeated minions are deactivated by their FSM rather than destroyed.
        for (int i = summonedMinions.Count - 1; i >= 0; i--)
        {
            Minion minion = summonedMinions[i].minion;
            if (minion == null || !minion.gameObject.activeInHierarchy)
            {
                summonedMinions.RemoveAt(i);
            }
        }

        activeSummonedMinionCount = summonedMinions.Count;
    }

    private void RemoveSummonedMinions()
    {
        foreach (SummonedMinion trackedMinion in summonedMinions)
        {
            if (trackedMinion.minion != null)
            {
                Destroy(trackedMinion.minion.gameObject);
            }
        }

        summonedMinions.Clear();
        activeSummonedMinionCount = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dangerRadius);

        Gizmos.color = Color.green;
        if (spawnPoints == null) return;

        int pointCount = Mathf.Min(spawnPoints.Length, 3);
        for (int i = 0; i < pointCount; i++)
        {
            if (spawnPoints[i] != null)
            {
                Gizmos.DrawSphere(spawnPoints[i].position, 0.2f);
            }
        }
    }
}
