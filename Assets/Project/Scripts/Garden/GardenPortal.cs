using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GardenPortal : MonoBehaviour
{
    // The portal stays closed until this many different guardians are defeated.
    [SerializeField, Min(1)] private int requiredGuardians = 2;
    [SerializeField] private Collider2D portalTrigger;
    [SerializeField] private SpriteRenderer portalRenderer;
    [SerializeField] private Color inactiveColor = new(0.25f, 0.25f, 0.3f, 0.55f);
    [SerializeField] private Color activeColor = new(0.3f, 1f, 0.75f, 1f);

    // HashSets prevent the same guardian from being counted more than once.
    private readonly HashSet<GuardianStatues> guardians = new();
    private readonly HashSet<GuardianStatues> defeatedGuardians = new();
    private bool isActive;

    // These properties let other scripts check the portal's progress.
    public bool IsActive => isActive;
    public int DefeatedGuardianCount => defeatedGuardians.Count;

    private void Awake()
    {
        // Find the portal's components if they were not assigned in the Inspector.
        if (portalTrigger == null)
            portalTrigger = GetComponent<Collider2D>();
        if (portalRenderer == null)
            portalRenderer = GetComponent<SpriteRenderer>();

        requiredGuardians = Mathf.Max(1, requiredGuardians);
        isActive = false;
        // The trigger is turned on later, when the guardians are defeated.
        portalTrigger.isTrigger = true;
        portalTrigger.enabled = false;
        SetVisualState(inactiveColor);
    }

    private void Start()
    {
        // Register scene guardians in case they did not register themselves.
        GuardianStatues[] sceneGuardians = FindObjectsByType<GuardianStatues>();
        foreach (GuardianStatues guardian in sceneGuardians)
            RegisterGuardian(guardian);

        if (guardians.Count < requiredGuardians)
        {
            // Report a setup issue instead of silently leaving the portal locked.
            Debug.LogError(
                $"The garden portal requires {requiredGuardians} guardians, but only {guardians.Count} are registered.",
                this);
        }
    }

    public void RegisterGuardian(GuardianStatues guardian)
    {
        // Ignore missing references and remember this guardian only once.
        if (guardian == null)
            return;

        guardians.Add(guardian);
        if (guardian.IsDefeated)
            defeatedGuardians.Add(guardian);
        TryActivatePortal();
    }

    public void NotifyGuardianDefeated(GuardianStatues guardian)
    {
        // Track the defeat, then check whether the portal can open.
        if (guardian == null)
            return;

        guardians.Add(guardian);
        defeatedGuardians.Add(guardian);
        TryActivatePortal();
    }

    private void TryActivatePortal()
    {
        // Both the registered and defeated counts must meet the requirement.
        if (!isActive && guardians.Count >= requiredGuardians && defeatedGuardians.Count >= requiredGuardians)
            ActivatePortal();
    }

    private void ActivatePortal()
    {
        // Open the trigger and change the color to show the portal is ready.
        isActive = true;
        portalTrigger.enabled = true;
        SetVisualState(activeColor);
        Debug.Log("Garden portal activated. Both guardians have been defeated.", this);
    }

    private void SetVisualState(Color color)
    {
        // A renderer is optional, so change its color only when one is present.
        if (portalRenderer != null)
            portalRenderer.color = color;
    }
}
