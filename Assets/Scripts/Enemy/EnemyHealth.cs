using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;

    //[SerializeField] Animator animator;

    private float currentHealth;
    private bool isDead = false;

    private ScoreManager scoreManager;

    void Start()
    {
        currentHealth = maxHealth;
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        //if (animator != null)
         //animator.SetTrigger("Die");

        if (scoreManager != null)
            scoreManager.AddEnemyKill();
        
        Destroy(gameObject, 1.5f);
    }
}