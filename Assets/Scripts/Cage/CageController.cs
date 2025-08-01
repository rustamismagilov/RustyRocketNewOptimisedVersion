using UnityEngine;

public class CageController : MonoBehaviour
{
    private PlayerController player;
    private bool isOpened = false;
    private Animator animator;
    private ScoreManager scoreManager;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenCage();
        }
    }

    void OpenCage()
    {
        if (isOpened || player == null)
            return;

        animator.SetBool("isOpen", true);
        isOpened = true;

        if (scoreManager != null)
            scoreManager.AddCageOpened();

        Debug.Log($"Cage {name} opened!");
    }
}