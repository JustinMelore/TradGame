using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles behavior for the main menu's buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private StoryScreen storyScreen;
    
    /// <summary>
    /// Starts the story "cutscene" prior to starting the game
    /// </summary>
    public void StartGame()
    {
        storyScreen.RevealStoryScreen();
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
