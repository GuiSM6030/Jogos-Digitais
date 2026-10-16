using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class EnemySpawner : MonoBehaviour
{
    private const int MaximumEnemies = 6;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private RuntimeAnimatorController attackAnimatorController;
    [SerializeField] private RuntimeAnimatorController deathAnimatorController;
    [SerializeField, Min(0.1f)] private float spawnInterval = 2f;
    [SerializeField, Min(0f)] private float spawnHeightOffset = 0.05f;

    private Tilemap spawnTilemap;
    private PlayerHealth playerHealth;
    private float nextSpawnTime;

    private void Awake()
    {
        spawnTilemap = GetComponent<Tilemap>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time < nextSpawnTime)
        {
            return;
        }

        nextSpawnTime = Time.time + spawnInterval;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnTilemap == null ||
            FindObjectsByType<SpiderEnemy>(FindObjectsSortMode.None).Length >= MaximumEnemies)
        {
            return;
        }

        List<Vector3Int> spawnCells = new List<Vector3Int>();
        BoundsInt cellBounds = spawnTilemap.cellBounds;
        foreach (Vector3Int cell in cellBounds.allPositionsWithin)
        {
            if (spawnTilemap.HasTile(cell))
            {
                spawnCells.Add(cell);
            }
        }

        if (spawnCells.Count == 0)
        {
            return;
        }

        Vector3Int spawnCell = spawnCells[Random.Range(0, spawnCells.Count)];
        Vector3 spawnPosition = spawnTilemap.GetCellCenterWorld(spawnCell);
        Vector3 cellHeight = spawnTilemap.CellToWorld(spawnCell + Vector3Int.up) -
            spawnTilemap.CellToWorld(spawnCell);
        spawnPosition.y += Mathf.Abs(cellHeight.y) * 0.5f + spawnHeightOffset;

        GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        SpiderEnemy enemy = enemyObject.GetComponent<SpiderEnemy>();
        if (enemy == null)
        {
            enemy = enemyObject.AddComponent<SpiderEnemy>();
        }

        enemy.Configure(attackAnimatorController, deathAnimatorController, playerHealth);
    }
}