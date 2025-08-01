using UnityEngine;

public class CageController : MonoBehaviour
{
    private PlayerController player;

    private bool isOpened = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
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
        Debug.Log($"Cage {name} opened!");
    }
}