using UnityEngine;

public class TeleportingInScene : MonoBehaviour
{
    [SerializeField] Transform teleportTarget;
    PlayerController player;
    Animator playerAnimator;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Reset speed of the player after TP
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        playerAnimator.SetTrigger("fade");
        Invoke(nameof(Teleport), 0.5f);
    }

    void Teleport()
    {
        player.transform.position = teleportTarget.position;
    }
}
