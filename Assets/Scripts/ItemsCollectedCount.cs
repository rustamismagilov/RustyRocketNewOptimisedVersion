using TMPro;
using UnityEngine;

public class ItemsCollectedCount: MonoBehaviour
{
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI burgersCountText;
    [SerializeField] private TextMeshProUGUI explosivesCountText;
    [SerializeField] private TextMeshProUGUI trashCountText;

    private int burgersCount = 0;
    private int explosivesCount = 0;
    private int trashCount = 0;

    public void AddBurger()
    {
        burgersCount++;
        UpdateBurgersText();
    }

    public void AddExplosive()
    {
        explosivesCount++;
        UpdateExplosivesText();
    }

    public void AddTrash()
    {
        trashCount++;
        UpdateTrashText();
    }

    private void UpdateBurgersText()
    {
        if (burgersCountText != null)
            burgersCountText.text = $"Burgers: {burgersCount}";
    }

    private void UpdateExplosivesText()
    {
        if (explosivesCountText != null)
            explosivesCountText.text = $"Explosives: {explosivesCount}";
    }

    private void UpdateTrashText()
    {
        if (trashCountText != null)
            trashCountText.text = $"Trash: {trashCount}";
    }
}