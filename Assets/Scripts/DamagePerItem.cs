using UnityEngine;

public class DamagePerItem : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] float damageAmount = 5f;
    [SerializeField] ParticleSystem hitEffect;

    [Header("Audio Settings")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] float volume = 0.3f;
    [SerializeField] bool use3DSound = true;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            }

            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound, volume);
            }

            Destroy(gameObject, 0.1f);
        }
    }
}