using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// Properties of an enemy wave, including available enemies, enemy counts, and enemy weights
/// </summary>
public class Wave
{
    [SerializeField] private int boatEnemyCount;
    [SerializeField] private int seaEnemyCount;
    [SerializeField] private List<GameObject> boatEnemyTypes;
    [SerializeField] private List<float> boatEnemyWeights;
    [SerializeField] private List<GameObject> seaEnemyTypes;
    [SerializeField] private List<float> seaEnemyWeights;

    /// <summary>
    /// The number of enemies that spawn on the ship
    /// </summary>
    public int BoatEnemyCount { get { return boatEnemyCount; } private set { boatEnemyCount = value; } }

    /// <summary>
    /// The number of enemies that spawn outside the ship
    /// </summary>
    public int SeaEnemyCount { get { return seaEnemyCount; } private set { seaEnemyCount = value; } }

    /// <summary>
    /// The list of available ship enemies, including what percentage of the boat enemy count they should make up
    /// </summary>
    public List<Tuple<GameObject, float>> BoatEnemies { 
        get {
            List<Tuple<GameObject, float>> boatEnemies = new List<Tuple<GameObject, float>>();
            for (int i = 0; i < boatEnemyTypes.Count; i++) boatEnemies.Add(new Tuple<GameObject, float>(boatEnemyTypes[i], boatEnemyWeights[i]));
            return boatEnemies;
        }}

    /// <summary>
    /// The list of available sea enemies, including what percentage of the sea enemy count they should make up
    /// </summary>
    public List<Tuple<GameObject, float>> SeaEnemies { 
        get {
            List<Tuple<GameObject, float>> seaEnemies = new List<Tuple<GameObject, float>>();
            for (int i = 0; i < seaEnemyTypes.Count; i++) seaEnemies.Add(new Tuple<GameObject, float>(seaEnemyTypes[i], seaEnemyWeights[i]));
            return seaEnemies;
        }
    }

    /// <summary>
    /// Creates a new enemy wave
    /// </summary>
    /// <param name="boatEnemyCount">How many enemies should spawn on the boat</param>
    /// <param name="seaEnemyCount">How many enemies should spawn outside of the boat</param>
    /// <param name="boatEnemies">The available enemy types that spawn on the boat plus the percentage of the boat enemy count they should make up</param>
    /// <param name="seaEnemies">The available enemy types that spawn outside the boat plus the percentage of the sea enemy count they should make up</param>

    //public Wave(int boatEnemyCount, int seaEnemyCount, List<Tuple<GameObject, float>> boatEnemies, List<Tuple<GameObject, float>> seaEnemies)
    //{
    //    BoatEnemyCount = boatEnemyCount;
    //    SeaEnemyCount = seaEnemyCount;
    //    BoatEnemies = boatEnemies;
    //    SeaEnemies = seaEnemies;
    //}
}
