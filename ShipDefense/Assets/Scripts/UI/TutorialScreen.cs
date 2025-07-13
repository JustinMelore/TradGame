using UnityEngine;

/// <summary>
/// Handles behavior for the TutorialScreen
/// </summary>
public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject tutorialScreen;

    private void Awake()
    {
        tutorialScreen.SetActive(false);
    }

    public void OnTutorialButtonPressed()
    {
        mainMenuScreen.SetActive(false);
        tutorialScreen.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        mainMenuScreen.SetActive(true);
        tutorialScreen.SetActive(false);
    }
}
