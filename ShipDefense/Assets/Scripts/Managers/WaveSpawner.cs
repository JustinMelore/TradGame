using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles the spawning of enemy waves. Each wave has a specific number of enemies that spawn, with each available enemy type making up a specific percentage of the enemy
/// count
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> enemyWaves;
    [SerializeField] private float waveDowntimeInterval;
    [SerializeField] private Tilemap shipTiles;
    [SerializeField] private Tilemap seaTiles;
    [SerializeField] private GameManager gameManager;

    private int currentWaveIndex;
    private List<Vector3> shipSpawnLocations;
    private List<Vector3> seaSpawnLocations;
    private bool wavesRemaining;
    private int currentWaveEnemyCount;
    private bool isSpawningWave;

    private void Awake()
    {
        currentWaveIndex = -1;
        wavesRemaining = true;
        isSpawningWave = false;
        shipSpawnLocations = GetTilePositions(shipTiles);
        seaSpawnLocations = GetTilePositions(seaTiles);
    }

    private void Update()
    {
        if (!wavesRemaining) return;
        if (currentWaveEnemyCount <= 0 && !isSpawningWave)
        {
            Debug.Log("Enemy wave defeated");
            currentWaveIndex++;
            if (currentWaveIndex >= enemyWaves.Count)
            {
                wavesRemaining = false;
                Debug.Log("All waves defeated");
                gameManager.WinGame();
            }
            else StartCoroutine(SpawnWave());
        }
    }

    /// <summary>
    /// Spawns the next wave of enemies after the waveDowntimeInterval expires
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave()
    {
        isSpawningWave = true;
        yield return new WaitForSeconds(waveDowntimeInterval);
        Debug.Log("Spawning wave");
        HashSet<Vector3> takenSpawnPoints = new HashSet<Vector3>();
        currentWaveEnemyCount = enemyWaves[currentWaveIndex].BoatEnemyCount + enemyWaves[currentWaveIndex].SeaEnemyCount;
        SpawnEnemies(enemyWaves[currentWaveIndex].BoatEnemyCount, enemyWaves[currentWaveIndex].BoatEnemies, shipSpawnLocations, takenSpawnPoints);
        SpawnEnemies(enemyWaves[currentWaveIndex].SeaEnemyCount, enemyWaves[currentWaveIndex].SeaEnemies, seaSpawnLocations, takenSpawnPoints);
        isSpawningWave = false;
    }

    /// <summary>
    /// Reduces the current enemy count by 1
    /// </summary>
    public void DecrementEnemyCount()
    {
        currentWaveEnemyCount--;
        Debug.Log("Enemy defeated. New enemy count: " + currentWaveEnemyCount);
    }

    /// <summary>
    /// Gets the world positions of all tiles in a given tilemap
    /// </summary>
    /// <param name="tileMap">The tilemap to get the tile positions from</param>
    /// <returns>A list of all the world positions of each tile</returns>
    private List<Vector3> GetTilePositions(Tilemap tileMap)
    {
        List<Vector3> tilePositions = new List<Vector3>();
        foreach(var position in tileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int cellPos = new Vector3Int(position.x, position.y, position.z);
            Vector3 worldPos = tileMap.CellToWorld(cellPos);
            if (tileMap.HasTile(cellPos)) tilePositions.Add(worldPos);
        }
        return tilePositions;
    }

    /// <summary>
    /// Spawns enemies from a list based on their spawn weight
    /// </summary>
    /// <param name="enemyCount">The total number of enemies to spawn</param>
    /// <param name="availableEnemies">A list of all available enemy types</param>
    /// <param name="spawnLocations">The list of all possible spawn locations</param>
    /// <param name="takenLocations">A set containing spawn locations that are already in use</param>
    private void SpawnEnemies(int enemyCount, List<Tuple<GameObject, float>> availableEnemies, List<Vector3> spawnLocations, HashSet<Vector3> takenLocations)
    {
        foreach(var availableEnemy in availableEnemies) {
            //Debug.Log($"{availableEnemy.Item1} has a weight of {availableEnemy.Item2}: Total spawns should be {enemyCount * availableEnemy.Item2}");
            Debug.Log($"{enemyCount * availableEnemy.Item2}");
            for(int i = 0; i < enemyCount * availableEnemy.Item2; i++)
            {
                Debug.Log($"{i} < {enemyCount * availableEnemy.Item2}: {i < enemyCount * availableEnemy.Item2}");
                Vector3 spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count - 1)];
                while(takenLocations.Contains(spawnLocation)) spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count - 1)];
                Instantiate(availableEnemy.Item1, spawnLocation, Quaternion.identity);
                takenLocations.Add(spawnLocation);
            }
        }
    }
}
