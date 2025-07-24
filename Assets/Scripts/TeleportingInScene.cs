using UnityEngine;

public class TeleportingInScene : MonoBehaviour
{
    [SerializeField] Transform teleportTarget;
    PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportTarget.position;
    }
}
