using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles behavior for the main menu's buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Act1");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
