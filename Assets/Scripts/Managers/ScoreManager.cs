using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eggsCollectedText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;

    //Eggs
    private int eggsCollected = 0;
    private int totalEggs = 10;
    
    //Enemies
    private int enemiesKilled = 0;
    private int totalEnemies = 100;

    public void AddEgg()
    {
        eggsCollected++;
        if (eggsCollected > totalEggs) eggsCollected = totalEggs;
        UpdateEggsText();
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
        if (enemiesKilled > totalEnemies) enemiesKilled = totalEnemies;
        UpdateEnemiesText();
    }

    private void UpdateEggsText()
    {
        if (eggsCollectedText != null)
            eggsCollectedText.text = $"{eggsCollected}/{totalEggs}";
    }

    private void UpdateEnemiesText()
    {
        if (enemiesKilledText != null)
            enemiesKilledText.text = $"{enemiesKilled}/{totalEnemies}";
    }

    void Start()
    {
        UpdateEggsText();
        UpdateEnemiesText();
    }
}