using UnityEngine;

/// <summary>
/// Tracks the boss's health and phase.
/// </summary>
public class BossHealth : MonoBehaviour
{
    [SerializeField, Min(1), Tooltip("Boss health at the start of play.")]
    private int maximumHealth = 20;
    [SerializeField, Tooltip("Runtime health. Initialized to Maximum Health when play starts.")]
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaximumHealth => maximumHealth;
    // The behavior tree uses this to check for phase two.
    public bool IsSecondPhase => currentHealth <= maximumHealth / 2;

    private void Awake()
    {
        // Start at full health.
        maximumHealth = Mathf.Max(1, maximumHealth);
        currentHealth = maximumHealth;
    }

    public void TakeDamage(int damage)
    {
        // Ignore invalid damage and hits after defeat.
        if (damage <= 0 || currentHealth <= 0) return;

        int previousHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        LogHealth();

        // Check if this hit crossed the halfway point.
        if (previousHealth > maximumHealth / 2 && currentHealth <= maximumHealth / 2)
        {
            Debug.Log("Boss entered Phase 2. Minion summons are doubled.", this);
        }

        if (currentHealth == 0)
        {
            Debug.Log("Boss defeated.", this);
            // Turn off the boss when its health reaches zero.
            gameObject.SetActive(false);
        }
    }

    public void TakeProjectileHit()
    {
        // Projectile hits deal one point of damage.
        TakeDamage(1);
    }

    private void LogHealth()
    {
        string phase = IsSecondPhase ? "Phase 2" : "Phase 1";
        Debug.Log($"Boss health: {currentHealth}/{maximumHealth} - {phase}.", this);
    }
}
