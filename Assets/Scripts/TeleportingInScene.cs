using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportingInScene: MonoBehaviour
{
    [SerializeField] Transform teleportTarget;
    [SerializeField] GameObject player;
    [SerializeField] GameObject alienFriend;

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportTarget.position;
        alienFriend.transform.position = teleportTarget.position;
    }
}
