using UnityEngine;
using System.Collections.Generic;

public class PickupTest : MonoBehaviour
{
    [SerializeField] public Transform pickupsParent;
    [SerializeField] private List<GameObject> pickups = new(5); // List to keep children      
    [SerializeField] public float pickupRange = 3f;
    
    private GameObject heldPickup = null;

    void Start()
    {
        foreach (Transform child in pickupsParent)
        {
            pickups.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldPickup == null)
        {
            PickUp();
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldPickup != null)
        {
            Drop();
        }
    }

    void PickUp()
    {
        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeInHierarchy && Vector3.Distance(transform.position, pickup.transform.position) <= pickupRange)
            {
                heldPickup = pickup;
                heldPickup.SetActive(false);
                break;
            }
        }
    }

    void Drop()
    {
        heldPickup.transform.position = transform.position + transform.forward * 2f;
        heldPickup.SetActive(true);
        heldPickup = null;
    }
}