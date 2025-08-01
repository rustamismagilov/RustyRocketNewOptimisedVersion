using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] float maxHealth = 10f;
    private float currentHealth;
    private bool isDead = false;

    [Header("Audio Settings")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] float hitVolume = 0.4f;
    private AudioSource audioSource;

    private ScoreManager scoreManager;

    void Start()
    {
        currentHealth = maxHealth;
        scoreManager = FindFirstObjectByType<ScoreManager>();
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = hitVolume;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        PlayHitSound();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (scoreManager != null)
            scoreManager.AddEnemyKill();

        Destroy(gameObject, 1.5f);
    }
}