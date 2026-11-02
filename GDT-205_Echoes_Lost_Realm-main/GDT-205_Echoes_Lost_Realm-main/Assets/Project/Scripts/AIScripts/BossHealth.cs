using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maximumHealth = 20;
    [SerializeField] private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaximumHealth => maximumHealth;
    public bool IsSecondPhase => currentHealth <= maximumHealth / 2;

    private void Awake()
    {
        maximumHealth = Mathf.Max(1, maximumHealth);
        currentHealth = maximumHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        LogHealth();

        if (currentHealth == maximumHealth / 2)
        {
            Debug.Log("Boss entered Phase 2. Minion summons are doubled.", this);
        }

        if (currentHealth == 0)
        {
            Debug.Log("Boss defeated.", this);
            gameObject.SetActive(false);
        }
    }

    public void TakeProjectileHit()
    {
        TakeDamage(1);
    }

    private void LogHealth()
    {
        string phase = IsSecondPhase ? "Phase 2" : "Phase 1";
        Debug.Log($"Boss health: {currentHealth}/{maximumHealth} - {phase}.", this);
    }
}
