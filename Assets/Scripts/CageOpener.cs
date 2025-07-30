using UnityEngine;

public class CageOpener : MonoBehaviour
{
    public GameObject player;
    public float openRange = 2f;
    public bool isKey = false;
    private bool isOpened = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        if (isOpened || !isKey || player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= openRange)
        {
            animator.SetBool("isOpen", true);
            isOpened = true;
            Debug.Log("Cage opened!");
        }
    }
}