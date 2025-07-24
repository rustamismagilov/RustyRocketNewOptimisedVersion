using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private int collisionDamage = 10;
    [SerializeField] private int enemyDamageAmount = 10;
    [SerializeField] private float safeLandingSpeed = 5f;


    AudioSource audioSource;
    Health health;

    void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        health = GetComponent<Health>();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("Start");
                break;
            case "Finish":
                Debug.Log("Finish");
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Fuel");
                Destroy(other.gameObject);
                break;
            case "Enemy":
                Debug.Log("Enemy");
                HandleEnemyCollision(other);
                break;
            default:
                Debug.Log($"Bumped into {other.gameObject.name}");
                HandleCrash(other);
                break;

        }
    }

    void HandleCrash(Collision other)
    {
        if (other.relativeVelocity.magnitude < safeLandingSpeed) return;
        if (health != null)
        {
            health.TakeDamage(collisionDamage);
            audioSource.PlayOneShot(crashSound);

            if (health.GetHealth() <= 0)
            {
                audioSource.PlayOneShot(crashSound);
                GetComponent<PlayerController>().enabled = false;
                Invoke(nameof(ReloadLevel), levelLoadDelay);
            }
        }
    }

    void HandleEnemyCollision(Collision other)
    {
        health.TakeDamage(enemyDamageAmount);

        audioSource.PlayOneShot(crashSound);

        if (health.GetHealth() <= 0)
        {
            GetComponent<PlayerController>().enabled = false;
            Invoke(nameof(ReloadLevel), levelLoadDelay);
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartSuccessSequence()
    {
        audioSource.PlayOneShot(successSound);
        GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }
}
