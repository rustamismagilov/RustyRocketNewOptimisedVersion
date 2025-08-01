using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eggsCollectedText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI cagesOpenedText;
    [SerializeField] private GameObject finishButton;

    private int eggsCollected = 0;
    private int totalEggs = 3;

    private int enemiesKilled = 0;
    private int totalEnemies = 10;

    private int cagesOpenedCount = 0;
    private int totalCages = 5;

    void Start()
    {
        UpdateEggsText();
        UpdateEnemiesText();
        UpdateCagesText();

        if (finishButton != null)
            finishButton.SetActive(false);
    }

    public void AddEgg()
    {
        eggsCollected++;
        if (eggsCollected > totalEggs) eggsCollected = totalEggs;
        UpdateEggsText();
        CheckWinCondition();
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
        if (enemiesKilled > totalEnemies) enemiesKilled = totalEnemies;
        UpdateEnemiesText();
        CheckWinCondition();
    }

    public void AddCageOpened()
    {
        cagesOpenedCount++;
        if (cagesOpenedCount > totalCages) cagesOpenedCount = totalCages;
        UpdateCagesText();
        CheckWinCondition();
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

    private void UpdateCagesText()
    {
        if (cagesOpenedText != null)
            cagesOpenedText.text = $"{cagesOpenedCount}/{totalCages}";
    }

    private void CheckWinCondition()
    {
        if (enemiesKilled >= totalEnemies && cagesOpenedCount >= totalCages)
        {
            if (finishButton != null)
                finishButton.SetActive(true);
        }
    }

    public void LoadFinalScene()
    {
        SceneManager.LoadScene("GameFinished");
    }
}
