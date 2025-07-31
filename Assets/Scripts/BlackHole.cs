using UnityEngine;
using TMPro;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private TextMeshPro messageText;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem burgerEffect;
    [SerializeField] private ParticleSystem explosiveEffect;
    [SerializeField] private ParticleSystem trashEffect;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 spawnPosition = other.transform.position;

        switch (other.tag)
        {
            case "Burger":
                SpawnEffect(burgerEffect, spawnPosition);
                Destroy(other.gameObject);

                if (messageText != null)
                    messageText.text = "Yum! Give me more";

                Debug.Log("Burger eaten by black hole");
                break;

            case "Explosive":
                SpawnEffect(explosiveEffect, spawnPosition);
                Destroy(other.gameObject);

                if (messageText != null)
                    messageText.text = "Don't throw explosives in here!";

                Debug.Log("Explosives");
                break;

            case "Trash":
                SpawnEffect(trashEffect, spawnPosition);
                Destroy(other.gameObject);

                if (messageText != null)
                    messageText.text = "Yuck!";

                Debug.Log("Trash");
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
}