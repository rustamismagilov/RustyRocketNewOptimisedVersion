using System.Collections.Generic;
using UnityEngine;

public class CategoryItemSwitcher : MonoBehaviour
{
    [SerializeField] private Transform category1;
    [SerializeField] private Transform category2;
    [SerializeField] private Transform category3;

    private Dictionary<Pickupable.CategoryType, List<GameObject>> categorizedItems = new();
    private Dictionary<Pickupable.CategoryType, int> categoryIndexes = new();

    private Pickupable.CategoryType currentCategory;

    void Start()
    {
        foreach (Pickupable.CategoryType type in System.Enum.GetValues(typeof(Pickupable.CategoryType)))
        {
            categorizedItems[type] = new List<GameObject>();
            categoryIndexes[type] = 0;
        }

        SwitchCategory(Pickupable.CategoryType.Category1, category1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCategory(Pickupable.CategoryType.Category1, category1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCategory(Pickupable.CategoryType.Category2, category2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCategory(Pickupable.CategoryType.Category3, category3);

        ProcessScrollWheel();
    }

    void SwitchCategory(Pickupable.CategoryType type, Transform categoryTransform)
    {
        category1.gameObject.SetActive(categoryTransform == category1);
        category2.gameObject.SetActive(categoryTransform == category2);
        category3.gameObject.SetActive(categoryTransform == category3);

        currentCategory = type;
        UpdateVisibleItems();
    }

    void ProcessScrollWheel()
    {
        var items = categorizedItems[currentCategory];
        if (items.Count <= 1) return;

        int currentIndex = categoryIndexes[currentCategory];

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentIndex = (currentIndex + 1) % items.Count;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        }

        categoryIndexes[currentCategory] = currentIndex;
        UpdateVisibleItems();
    }

    void UpdateVisibleItems()
    {
        var items = categorizedItems[currentCategory];

        if (items.Count == 0)
        {
            // No items in this category = nothing is visible
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = items[i];
            if (item == null) continue;

            bool isCurrent = (i == categoryIndexes[currentCategory]);

            if (isCurrent)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void AddItem(GameObject item)
    {
        Debug.Log($"AddItem called with: {item.name}");

        var pickupable = item.GetComponent<Pickupable>();
        if (pickupable == null)
        {
            Debug.LogError($"No Pickupable component on {item.name}");
            return;
        }

        var category = pickupable.category;

        if (!categorizedItems.ContainsKey(category))
        {
            Debug.LogError($"Category {category} not found in dictionary");
            return;
        }

        Transform categoryTransform = GetCategoryTransform(category);
        item.transform.SetParent(categoryTransform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (!categorizedItems[category].Contains(item))
        {
            categorizedItems[category].Add(item);
        }

        categoryIndexes[category] = categorizedItems[category].IndexOf(item);

        SwitchCategory(category, categoryTransform);

        FindFirstObjectByType<ItemsCollectedCount>()?.UpdateAllCounts();

        Debug.Log($"Item {item.name} added to {category} category");
    }

    public void RemoveItem(GameObject item)
    {
        var pickupable = item.GetComponent<Pickupable>();
        if (pickupable == null) return;

        var category = pickupable.category;
        var items = categorizedItems[category];

        if (items.Contains(item))
        {
            int removedIndex = items.IndexOf(item);
            items.Remove(item);

            // Adjust the current index if necessary
            if (categoryIndexes[category] >= items.Count && items.Count > 0)
            {
                categoryIndexes[category] = items.Count - 1;
            }
            else if (items.Count == 0)
            {
                categoryIndexes[category] = 0;
            }

            // Update visible items after removal
            if (currentCategory == category)
            {
                UpdateVisibleItems();
            }

            FindFirstObjectByType<ItemsCollectedCount>()?.UpdateAllCounts();

            Debug.Log($"Item {item.name} removed from {category} category");
        }
    }

    public GameObject GetCurrentlyHeldItem()
    {
        var items = categorizedItems[currentCategory];
        if (items.Count == 0) return null;

        int currentIndex = categoryIndexes[currentCategory];
        if (currentIndex >= 0 && currentIndex < items.Count)
        {
            return items[currentIndex];
        }
        return null;
    }

    Transform GetCategoryTransform(Pickupable.CategoryType type)
    {
        return type switch
        {
            Pickupable.CategoryType.Category1 => category1,
            Pickupable.CategoryType.Category2 => category2,
            Pickupable.CategoryType.Category3 => category3,
            _ => null
        };
    }

    public int GetItemCount(Pickupable.CategoryType category)
    {
        if (categorizedItems.ContainsKey(category))
            return categorizedItems[category].Count;
        return 0;
    }
}
