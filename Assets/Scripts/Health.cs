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
        Debug.Log("Health:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    } 

    void Die()
    {
        Debug.Log("crashed!");
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}

