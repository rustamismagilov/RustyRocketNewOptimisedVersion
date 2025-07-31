using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Health: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("crashed!");
        // death animation!!!
    }
}
