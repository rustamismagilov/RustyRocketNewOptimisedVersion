using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    float pickupDelay= 3f;
    bool isPickedUp = false;
    public float soundVolume = 1.5f;
    
    public AudioClip pickupSound;
    
    void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.CompareTag("Player"))
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }

            isPickedUp = true;
            Invoke("Pickup", pickupDelay); 
            Debug.Log("Player picked up key");
        }
    }
    void Pickup()
    {
        Debug.Log("Key picked up!");
        Destroy(gameObject);
    }
}