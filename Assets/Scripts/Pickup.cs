using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupSoundVolume = 1.5f;

    private GameObject heldPickup = null;
    private GameObject nearbyPickup = null;

    public GameObject GetHeldPickup()
    {
        return heldPickup;
    }

    public void RemoveHeldPickup()
    {
        heldPickup = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldPickup == null && nearbyPickup != null)
            {
                PickUp(nearbyPickup);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (heldPickup != null)
            {
                Drop();
            }
        }
    }

    void PickUp(GameObject obj)
    {
        heldPickup = obj;

        heldPickup.transform.SetParent(holdPoint);
        heldPickup.transform.localPosition = Vector3.zero;
        heldPickup.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldPickup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = heldPickup.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);
        }
    }

    void Drop()
    {
        heldPickup.transform.SetParent(null);
        heldPickup.transform.position = transform.position + transform.forward * 2f;

        Rigidbody rb = heldPickup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Collider col = heldPickup.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        heldPickup = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pickupable>() != null)
        {
            nearbyPickup = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pickupable>() != null)
        {
            if (other.gameObject == nearbyPickup)
            {
                nearbyPickup = null;
            }
        }
    }
}
