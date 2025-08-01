using UnityEngine;
using System.Collections.Generic;

public class Pickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupSoundVolume = 1.5f;

    private GameObject nearbyPickup = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyPickup != null)
            {
                PickUp(nearbyPickup);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    void Drop()
    {
        var categorySwitcher = GetComponentInChildren<CategoryItemSwitcher>();
        if (categorySwitcher == null)
        {
            Debug.LogError("CategoryItemSwitcher not found!");
            return;
        }

        GameObject itemToDrop = categorySwitcher.GetCurrentlyHeldItem();
        if (itemToDrop == null)
        {
            Debug.Log("No item to drop");
            return;
        }

        Debug.Log($"Dropping item: {itemToDrop.name}");

        categorySwitcher.RemoveItem(itemToDrop);

        itemToDrop.transform.SetParent(null);
        itemToDrop.transform.SetPositionAndRotation(transform.position + transform.forward * 2f, Quaternion.identity);

        Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            // Add a small forward force to push it away from player
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
        }

        Collider col = itemToDrop.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        itemToDrop.SetActive(true);
    }

    void PickUp(GameObject obj)
    {
        Debug.Log($"Picking up: {obj.name}");

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);

        var categorySwitcher = GetComponentInChildren<CategoryItemSwitcher>();
        if (categorySwitcher != null)
        {
            categorySwitcher.AddItem(obj);
        }
        else
        {
            Debug.LogError("CategoryItemSwitcher not found!");
        }

        // Clear the nearby pickup since we picked it up
        nearbyPickup = null;
    }

    public void RegisterNearbyPickup(GameObject obj)
    {
        nearbyPickup = obj;
    }

    public void UnregisterNearbyPickup(GameObject obj)
    {
        if (nearbyPickup == obj)
        {
            nearbyPickup = null;
        }
    }
}
