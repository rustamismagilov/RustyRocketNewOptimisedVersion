using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    [SerializeField] int currentItem = 0;

    private List<GameObject> collectedItems = new List<GameObject>();


    void Start()
    {
        SetItemActive();
    }

    void Update()
    {
        if (collectedItems.Count == 0) return;

        int previousItem = currentItem;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousItem != currentItem)
        {
            SetItemActive();
        }
    }

    public void AddItem(GameObject newItem)
    {
        if (!collectedItems.Contains(newItem))
        {
            collectedItems.Add(newItem);
            newItem.SetActive(false);

            if (collectedItems.Count == 1)
            {
                currentItem = 0;
                SetItemActive();
            }
        }
    }

    void SetItemActive()
    {
        int itemIndex = 0;

        foreach (GameObject item in collectedItems)
        {
            item.SetActive(itemIndex == currentItem);
            itemIndex++;
        }
    }

    void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentItem >= collectedItems.Count - 1)
            {
                currentItem = 0;
            }
            else
            {
                currentItem++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentItem <= 0)
            {
                currentItem = collectedItems.Count - 1;
            }
            else
            {
                currentItem--;
            }
        }
    }

    void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && collectedItems.Count > 0)
        {
            currentItem = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && collectedItems.Count > 1)
        {
            currentItem = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && collectedItems.Count > 2)
        {
            currentItem = 2;
        }
    }

    public GameObject GetCurrentItem()
    {
        if (collectedItems.Count == 0) return null;
        return collectedItems[currentItem];
    }

    public void RemoveItem(GameObject item)
    {
        if (collectedItems.Contains(item))
        {
            collectedItems.Remove(item);
            if (currentItem >= collectedItems.Count)
            {
                currentItem = Mathf.Max(0, collectedItems.Count - 1);
            }
            SetItemActive();
        }
    }
}
