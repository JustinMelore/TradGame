using UnityEngine;

/// <summary>
/// This script manages enemy waves. Current functionality is temporary and will be updated with proper wave behavior in the future, and currently only serves to make
/// the player win or lose upon defeating all enemies.
/// </summary>
public class WaveManager : MonoBehaviour
{
    //Hard coded value is temporary solution; will replace with proper wave system in future
    [SerializeField] private int currentWaveEnemies;
    [SerializeField] private GameOverScreen endScreen;

    /// <summary>
    /// Reduces the current enemy count by 1
    /// </summary>
    public void DecrementEnemyCount()
    {
        --currentWaveEnemies;
        Debug.Log("Enemy defeated. New enemy count: " + currentWaveEnemies);
        if (currentWaveEnemies <= 0) WinWave();
    }

    /// <summary>
    /// Signals that the player has defeated the current wave.
    /// </summary>
    private void WinWave()
    {
        Debug.Log("Wave defeated!");
        endScreen.WinGame();
    }

    /// <summary>
    /// Signals that the player has lost the current wave
    /// </summary>
    public void FailWave()
    {
        Debug.Log("Wave failed!");
        endScreen.EndGame();
    }

}
