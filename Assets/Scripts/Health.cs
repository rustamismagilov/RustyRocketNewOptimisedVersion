using UnityEngine;

public class Health: MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    } 

    void Die()
    {
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}

