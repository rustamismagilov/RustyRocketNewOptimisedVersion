using TMPro;
using UnityEngine;

public class ItemsCollectedCount : MonoBehaviour
{
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI burgersCountText;
    [SerializeField] private TextMeshProUGUI explosivesCountText;
    [SerializeField] private TextMeshProUGUI trashCountText;

    private CategoryItemSwitcher categoryItemSwitcher;

    void Start()
    {
        categoryItemSwitcher = FindFirstObjectByType<CategoryItemSwitcher>();
        UpdateAllCounts();
    }

    public void UpdateAllCounts()
    {
        if (categoryItemSwitcher == null) return;

        UpdateCountText(burgersCountText, categoryItemSwitcher.GetItemCount(Pickupable.CategoryType.Category2));
        UpdateCountText(explosivesCountText, categoryItemSwitcher.GetItemCount(Pickupable.CategoryType.Category3));
        UpdateCountText(trashCountText, categoryItemSwitcher.GetItemCount(Pickupable.CategoryType.Category1));
    }

    private void UpdateCountText(TextMeshProUGUI textField, int count)
    {
        if (textField != null)
            textField.text = count.ToString();
    }
}
