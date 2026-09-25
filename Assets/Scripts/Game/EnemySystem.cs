using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private SectorSystem sectorSystem;
    [SerializeField] private EnemySpawnSystem enemySpawnSystem;

    [Header("Spawn Configuration")]
    [SerializeField] private float spawnDistance = 12f;

    private List<Enemy> enemies = new();

    private void Update()
    {
        UpdateDeadEnemies();
    }

    public void SpawnEnemy(int wave)
    {
        EnemyData enemyData =
            enemySpawnSystem.GetEnemyDataForWave(wave);

        if (enemyData == null)
            return;

        int sectorCount = sectorSystem.GetSectorCount();
        int sectorIndex = Random.Range(0, sectorCount);

        float sectorAngle = 360f / sectorCount;
        float sectorCenterAngle = sectorIndex * sectorAngle;

        Vector3 spawnPosition =
            GetSpawnPosition(sectorCenterAngle);

        Transform sector =
            sectorSystem.GetSectorTransform(sectorIndex);

        Enemy enemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity,
            sector
        );

        enemy.Initialize(
            player,
            playerHealth,
            gameManager,
            enemyData
        );

        enemies.Add(enemy);
    }

    public void ShootSector(int sectorIndex)
    {
        Enemy enemy = GetEnemyInSector(sectorIndex);

        if (enemy == null)
            return;

        if (enemy.IsDying())
            return;

        bool died = enemy.TakeDamage();

        if (!died)
            return;

        EnemyData enemyData = enemy.GetEnemyData();

        scoreManager.AddEnemyPoints(
            enemyData.GetEnemyName(),
            enemyData.GetScorePoints()
        );
    }

    private void UpdateDeadEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];

            if (enemy == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            if (!enemy.IsReadyToBeDestroyed())
                continue;

            enemies.RemoveAt(i);
            Destroy(enemy.gameObject);
        }
    }

    private Vector3 GetSpawnPosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        float x = Mathf.Sin(radians) * spawnDistance;
        float z = Mathf.Cos(radians) * spawnDistance;

        return new Vector3(x, 0f, z);
    }

    private Enemy GetEnemyInSector(int sectorIndex)
    {
        Transform sector =
            sectorSystem.GetSectorTransform(sectorIndex);

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];

            if (enemy == null)
                continue;

            if (enemy.transform.parent == sector)
                return enemy;
        }

        return null;
    }

    public int GetAliveEnemyCount()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
                enemies.RemoveAt(i);
        }

        return enemies.Count;
    }
}