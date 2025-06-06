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

    private void SpawnEnemies(int enemyCount, List<Tuple<GameObject, float>> availableEnemies, List<Vector3> spawnLocations, HashSet<Vector3> takenLocations)
    {
        foreach(var availableEnemy in availableEnemies) {
            Debug.Log($"{availableEnemy.Item1} has a weight of {availableEnemy.Item2}: Total spawns should be {enemyCount * availableEnemy.Item2}");
            for(int i = 1; i < enemyCount * availableEnemy.Item2; i++)
            {
                Vector3 spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count - 1)];
                while(takenLocations.Contains(spawnLocation)) spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count - 1)];
                Instantiate(availableEnemy.Item1, spawnLocation, Quaternion.identity);
                takenLocations.Add(spawnLocation);
            }
        }
    }
}
