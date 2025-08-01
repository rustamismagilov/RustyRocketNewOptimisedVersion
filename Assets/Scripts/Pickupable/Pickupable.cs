using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public enum CategoryType
    {
        Category1, // trash
        Category2, // food
        Category3  // explosives
    }

    public CategoryType category = CategoryType.Category1;

    void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();
        if (pickup != null)
        {
            pickup.RegisterNearbyPickup(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();
        if (pickup != null)
        {
            pickup.UnregisterNearbyPickup(gameObject);
        }
    }
}
