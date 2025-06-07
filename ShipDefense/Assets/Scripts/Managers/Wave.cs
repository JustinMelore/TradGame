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
    [SerializeField] private int boatEnemyPoints;
    [SerializeField] private int seaEnemyPoints;
    [SerializeField] private List<GameObject> boatEnemyTypes;
    [SerializeField] private List<int> boatEnemyCosts;
    [SerializeField] private List<GameObject> seaEnemyTypes;
    [SerializeField] private List<int> seaEnemyCosts;

    /// <summary>
    /// The total amount of points available to "spend" on boat enemies for this wave
    /// </summary>
    public int BoatEnemyCount { get { return boatEnemyPoints; } private set { boatEnemyPoints = value; } }

    /// <summary>
    /// The total amount of points available to "spend" on sea enemies for this wave
    /// </summary>
    public int SeaEnemyCount { get { return seaEnemyPoints; } private set { seaEnemyPoints = value; } }

    /// <summary>
    /// The list of available ship enemies, including the cost of each enemy
    /// </summary>
    public List<Tuple<GameObject, int>> BoatEnemies { 
        get {
            List<Tuple<GameObject, int>> boatEnemies = new List<Tuple<GameObject, int>>();
            for (int i = 0; i < boatEnemyTypes.Count; i++) boatEnemies.Add(new Tuple<GameObject, int>(boatEnemyTypes[i], boatEnemyCosts[i]));
            return boatEnemies;
        }}

    /// <summary>
    /// The list of available sea enemies, including the cost of each enemy
    /// </summary>
    public List<Tuple<GameObject, int>> SeaEnemies { 
        get {
            List<Tuple<GameObject, int>> seaEnemies = new List<Tuple<GameObject, int>>();
            for (int i = 0; i < seaEnemyTypes.Count; i++) seaEnemies.Add(new Tuple<GameObject, int>(seaEnemyTypes[i], seaEnemyCosts[i]));
            return seaEnemies;
        }
    }
}
