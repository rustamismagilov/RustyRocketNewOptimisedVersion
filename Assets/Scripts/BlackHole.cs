using UnityEngine;
using TMPro;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private TextMeshPro messageText;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem burgerEffect;
    [SerializeField] private ParticleSystem explosiveEffect;
    [SerializeField] private ParticleSystem trashEffect;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip burgerSound;
    [SerializeField] private AudioClip explosiveSound;
    [SerializeField] private AudioClip trashSound;
    [SerializeField] private float burgerVolume = 0.5f;
    [SerializeField] private float explosiveVolume = 0.7f;
    [SerializeField] private float trashVolume = 0.4f;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 spawnPosition = other.transform.position;

        switch (other.tag)
        {
            case "Burger":
                SpawnEffect(burgerEffect, spawnPosition);
                PlaySound(burgerSound, spawnPosition, burgerVolume);
                Destroy(other.gameObject);
                if (messageText != null) messageText.text = "Yum! Give me more";
                break;

            case "Explosive":
                SpawnEffect(explosiveEffect, spawnPosition);
                PlaySound(explosiveSound, spawnPosition, explosiveVolume);
                Destroy(other.gameObject);
                if (messageText != null) messageText.text = "Don't throw explosives in here!";
                break;

            case "Trash":
                SpawnEffect(trashEffect, spawnPosition);
                PlaySound(trashSound, spawnPosition, trashVolume);
                Destroy(other.gameObject);
                if (messageText != null) messageText.text = "Yuck!";
                break;
        }
    }

    private void SpawnEffect(ParticleSystem effect, Vector3 position)
    {
        if (effect != null)
        {
            Instantiate(effect, position, Quaternion.identity);
        }
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
}
