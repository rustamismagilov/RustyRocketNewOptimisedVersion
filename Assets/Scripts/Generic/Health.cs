using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private int maxFuel = 100;
    private int currentFuel;

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetFuel()
    {
        return currentFuel;
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentFuel = maxFuel;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddFuel(int amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel)
            currentFuel = maxFuel;

        Debug.Log("Fuel: " + currentFuel);
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Health: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("crashed!");
        // death animation!!!
    }
}
