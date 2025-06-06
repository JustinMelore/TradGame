using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Properties of an enemy wave, including available enemies, enemy counts, and enemy weights
/// </summary>
public class Wave
{
    /// <summary>
    /// The number of enemies that spawn on the ship
    /// </summary>
    public int BoatEnemyCount { get; private set; }

    /// <summary>
    /// The number of enemies that spawn outside the ship
    /// </summary>
    public int SeaEnemyCount { get; private set; }
    
    /// <summary>
    /// The list of available ship enemies, including what percentage of the boat enemy count they should make up
    /// </summary>
    public List<Tuple<GameObject, float>> BoatEnemies { get; private set; }

    /// <summary>
    /// The list of available sea enemies, including what percentage of the sea enemy count they should make up
    /// </summary>
    public List<Tuple<GameObject, float>> SeaEnemies { get; private set; }

    /// <summary>
    /// Creates a new enemy wave
    /// </summary>
    /// <param name="boatEnemyCount">How many enemies should spawn on the boat</param>
    /// <param name="seaEnemyCount">How many enemies should spawn outside of the boat</param>
    /// <param name="boatEnemies">The available enemy types that spawn on the boat plus the percentage of the boat enemy count they should make up</param>
    /// <param name="seaEnemies">The available enemy types that spawn outside the boat plus the percentage of the sea enemy count they should make up</param>

    public Wave(int boatEnemyCount, int seaEnemyCount, List<Tuple<GameObject, float>> boatEnemies, List<Tuple<GameObject, float>> seaEnemies)
    {
        BoatEnemyCount = boatEnemyCount;
        SeaEnemyCount = seaEnemyCount;
        BoatEnemies = boatEnemies;
        SeaEnemies = seaEnemies;
    }
}
