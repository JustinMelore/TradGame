using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior for the story screen that shows up after pressing "start" in the main menu
/// </summary>
public class StoryScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject storyTextContainer;

    [Header("Story Cutscene Settings")]
    [SerializeField] private float fadeTime;
    [SerializeField] private float textRevealTime;
    [SerializeField] private float buttonRevealTime;

    private CanvasGroup[] storyTextList;

    private void Awake()
    {
        fade.alpha = 0f;
        continueButton.SetActive(false);
        storyTextList = storyTextContainer.GetComponentsInChildren<CanvasGroup>();
        foreach (CanvasGroup storyText in storyTextList) storyText.alpha = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads act 1 on hitting the "continue" button
    /// </summary>
    public void OnContinue()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") SceneManager.LoadScene("Act1");
        else SceneManager.LoadScene("MainMenu");
    }
       
    /// <summary>
    /// Reveals the story screen over time
    /// </summary>
    public void RevealStoryScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(RevealStoryScreenCoroutine());
    }

    /// <summary>
    /// Coroutine that reveals all of the elements in the story screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator RevealStoryScreenCoroutine()
    {
        yield return FadeCanvasGroup(fade, 1f, fadeTime);
        foreach (CanvasGroup storyText in storyTextList) yield return FadeCanvasGroup(storyText, 1f, textRevealTime / storyTextList.Length);
        yield return new WaitForSeconds(buttonRevealTime);
        continueButton.SetActive(true);
    }

    /// <summary>
    /// Fades a given CanvasGroup over an amount of time
    /// </summary>
    /// <param name="canvasGroup">The CanvasGroup to fade</param>
    /// <param name="finalAlpha">The alpha value the CanvasGroup should have by the end</param>
    /// <param name="cgFadeTime">How it should take to fully fade the CanvasGroup</param>
    /// <returns></returns>
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float finalAlpha, float cgFadeTime)
    {
        float currentFadeTimer = 0f;
        float startingAlphaValue = canvasGroup.alpha;
        while(currentFadeTimer < cgFadeTime)
        {
            currentFadeTimer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startingAlphaValue, finalAlpha, currentFadeTimer / cgFadeTime);
            yield return null;
        }
    }
}
