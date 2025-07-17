using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script manages enemy waves. Current functionality is temporary and will be updated with proper wave behavior in the future, and currently only serves to make
/// the player win or lose upon defeating all enemies.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverScreen endScreen;

    public void Awake()
    {
        //endScreen = GetComponent<GameOverScreen>();
    }
    /// <summary>
    /// Signals that the player has defeated all the waves in this level
    /// </summary>
    public void WinGame()
    {
        Debug.Log("Act won!");
        if (SceneManager.GetActiveScene().name == "Act3")
        {
            FindFirstObjectByType<StoryScreen>(FindObjectsInactive.Include).RevealStoryScreen();
            FindFirstObjectByType<PlayerController>().enabled = false;
            Destroy(FindFirstObjectByType<Ship>().gameObject);
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Signals that the player has lost the current wave
    /// </summary>
    public void FailGame()
    {
        Debug.Log("Game lost!");
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.gameObject.SetActive(false);
        endScreen.EndGame();
    }

}
