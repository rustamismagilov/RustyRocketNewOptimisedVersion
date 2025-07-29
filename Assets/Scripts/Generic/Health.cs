using UnityEngine;

public class Health: MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField] int maxFuel = 100;
    [SerializeField] int currentFuel;

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

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetFuel()
    {
        return currentFuel;
    }
}