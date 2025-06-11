using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the behavior for the end game screen. This functionality is temporary and the class itself may be removed/replaced in the near future.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text endScreenText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Causes the "You Win!" text to appear
    /// </summary>
    public void WinGame()
    {
        gameObject.SetActive(true);
        endScreenText.text = "You Win!";
    }

    /// <summary>
    /// Causes the "You Lose!" text to appear
    /// </summary>
    public void EndGame()
    {
        gameObject.SetActive(true);
        endScreenText.text = "You Lose";
    }

    /// <summary>
    /// Restarts the game upon hitting the "play again" button
    /// </summary>
    public void OnPlayAgain()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the level and returns the player to the main menu on hitting the "Quit" button
    /// </summary>
    public void OnQuitLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
