using UnityEngine;

/// <summary>
/// This script manages enemy waves. Current functionality is temporary and will be updated with proper wave behavior in the future, and currently only serves to make
/// the player win or lose upon defeating all enemies.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverScreen endScreen;

    /// <summary>
    /// Signals that the player has defeated the current wave.
    /// </summary>
    public void WinGame()
    {
        Debug.Log("Game won!");
        endScreen.WinGame();
    }

    /// <summary>
    /// Signals that the player has lost the current wave
    /// </summary>
    public void FailGame()
    {
        Debug.Log("Game lost!");
        endScreen.EndGame();
    }

}
