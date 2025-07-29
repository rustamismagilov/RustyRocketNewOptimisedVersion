using UnityEngine;
using UnityEngine.UI;

public class StoryTellingCanvas: MonoBehaviour
{
    [SerializeField] public GameObject[] storySlides;
    [SerializeField] public Button nextButton;
    [SerializeField] public Button backButton;

    private int currentIndex = 0;

    public void OnNextClicked()
    {
        if (currentIndex < storySlides.Length - 1)
        {
            currentIndex++;
            ShowSlide();
        }
    }

    public void OnBackClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowSlide();
        }
    }

    void Start()
    {
        ShowSlide();
    }

    void ShowSlide()
    {
        for (int i = 0; i < storySlides.Length; i++)
        {
            storySlides[i].SetActive(i == currentIndex);
        }

        backButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < storySlides.Length - 1;
    }
}