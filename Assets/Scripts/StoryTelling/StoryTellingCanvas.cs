using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryTellingCanvas : MonoBehaviour
{
    [Header("Slides and Navigation")]
    [SerializeField] private GameObject[] storySlides;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Canvas References")]
    [SerializeField] private GameObject storyCanvas;

    [SerializeField] private GameObject startCanvas;

    private int currentIndex = 0;

    void Start()
    {
        ShowSlide();
    }

    public void OnNextClicked()
    {
        currentIndex++;
        
        if (currentIndex >= storySlides.Length)
        {
            EndStory();
            return;
        }

        ShowSlide();
    }

    public void OnBackClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowSlide();
        }
    }

    public void OnStartClicked()
    {
        SceneManager.LoadScene("Planet1");
    }

    void ShowSlide()
    {
        for (int i = 0; i < storySlides.Length; i++)
        {
            storySlides[i].SetActive(i == currentIndex);
        }
        
        backButton.gameObject.SetActive(currentIndex > 0);
        
        nextButton.gameObject.SetActive(true);
    }

    void EndStory()
    {
        foreach (var slide in storySlides)
        {
            slide.SetActive(false);
        }
        
        nextButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        
        if (storyCanvas != null)
            storyCanvas.SetActive(false);
        
        if (startCanvas != null)
            startCanvas.SetActive(true);
    }
}
