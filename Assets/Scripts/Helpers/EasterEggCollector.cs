using UnityEngine;

public class EasterEggCollector : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        scoreManager.AddEgg();

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EasterEgg") && scoreManager != null)
        {
            scoreManager.AddEgg();
        }
    }
}