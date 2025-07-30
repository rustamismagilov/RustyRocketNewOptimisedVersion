using UnityEngine;
using System.Collections.Generic;

public class PickupTest : MonoBehaviour
{
    [SerializeField] public Transform pickupsParent;
    //TODO: rework list
    [SerializeField] private List<GameObject> pickups;
    [SerializeField] public float pickupRange = 3f;
    [SerializeField] private Transform holdPoint;

    [SerializeField] private ItemSwitcher itemSwitcher;

    private GameObject heldPickup = null;
    public ItemsCollectedCount itemsCollectedCount;

    private GameObject burgerItem = null;
    private GameObject explosiveItem = null;
    private GameObject trashItem = null;

    void Start()
    {
        pickups.Clear(); // for duplicates

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

        // switch through tags

        if (Input.GetKeyDown(KeyCode.Alpha1) && burgerItem != null)
        {
            ShowItem(burgerItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && explosiveItem != null)
        {
            ShowItem(explosiveItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && trashItem != null)
        {
            ShowItem(trashItem);
        }

        ConnectItemWithSwitcher();
    }

    void PickUp()
    {
        //TODO: rework this part

        foreach (GameObject pickup in pickups)
        {
            if (Vector3.Distance(transform.position, pickup.transform.position) <= pickupRange)
            {
                heldPickup = pickup;

                // Disable physics
                var rb = heldPickup.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;


                if (itemSwitcher != null)
                {
                    itemSwitcher.AddItem(heldPickup);
                }

                // Count items
                if (pickup.CompareTag("Burger"))
                    itemsCollectedCount.AddBurger();
                else if (pickup.CompareTag("Explosive"))
                    itemsCollectedCount.AddExplosive();
                else if (pickup.CompareTag("Trash"))
                    itemsCollectedCount.AddTrash();

                break;
            }
        }
    }

    void Drop()
    {
        if (heldPickup != null)
        {
            heldPickup.transform.SetParent(null);

            // Drop a bit in front
            heldPickup.transform.position = transform.position + transform.forward * 2f;

            // Enable physics and collider
            var rb = heldPickup.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = false;

            var col = heldPickup.GetComponent<Collider>();
            if (col) col.enabled = true;

            if (itemSwitcher != null)
            {
                itemSwitcher.RemoveItem(heldPickup);
            }

            heldPickup = null;
        }
    }

    void ConnectItemWithSwitcher()
    {
        if (itemSwitcher == null) return;

        GameObject newItem = itemSwitcher.GetCurrentItem();

        if (newItem != heldPickup)
        {
            heldPickup = newItem;
            if (heldPickup != null)
            {
                heldPickup.transform.SetParent(holdPoint);
                heldPickup.transform.localPosition = Vector3.zero;
                heldPickup.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void ShowItem(GameObject item)
    {
        if (heldPickup != null)
        {
            heldPickup.SetActive(false);
            heldPickup.transform.SetParent(null);
        }

        heldPickup = item;
        heldPickup.SetActive(true);
        heldPickup.transform.SetParent(holdPoint);
        heldPickup.transform.localPosition = Vector3.zero;
        heldPickup.transform.localRotation = Quaternion.identity;
    }

    public GameObject GetHeldPickup()
    {
        return heldPickup;
    }

    public void RemoveHeldPickup()
    {
        heldPickup = null;
    }
}
