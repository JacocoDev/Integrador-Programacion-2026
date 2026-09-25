using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Wave Configuration")]
    [SerializeField] private int startingEnemyCount = 3;
    [SerializeField] private int enemiesAddedPerWave = 2;

    [SerializeField] private float minimumSpawnDelay = 4f;
    [SerializeField] private float maximumSpawnDelay = 8f;
    [SerializeField] private float waveCooldown = 5f;

    private int currentWave;
    private int totalEnemies;
    private int spawnedEnemies;

    private float spawnTimer;
    private float waveCooldownTimer;

    private bool isWaitingForNextWave;

    private void Start()
    {
        minimumSpawnDelay = GameSettings.minimumSpawnDelay;
        maximumSpawnDelay = GameSettings.maximumSpawnDelay;

        ValidateConfiguration();
        StartNextWave();
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        if (isWaitingForNextWave)
        {
            UpdateWaveCooldown();
            return;
        }

        UpdateWaveSpawning();
        UpdateWaveCompletion();
    }

    private void UpdateWaveSpawning()
    {
        if (spawnedEnemies >= totalEnemies)
            return;

        if (enemySystem.GetAliveEnemyCount() > 0)
            return;

        if (spawnedEnemies > 0 && spawnTimer <= 0f)
            spawnTimer = GetNextSpawnDelay();

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        enemySystem.SpawnEnemy(currentWave);
        spawnedEnemies++;
    }

    private void UpdateWaveCompletion()
    {
        if (spawnedEnemies < totalEnemies)
            return;

        if (enemySystem.GetAliveEnemyCount() > 0)
            return;

        StartWaveCooldown();
    }

    private void UpdateWaveCooldown()
    {
        waveCooldownTimer -= Time.deltaTime;

        if (waveCooldownTimer > 0f)
            return;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWave++;

        totalEnemies =
            startingEnemyCount +
            (currentWave - 1) * enemiesAddedPerWave;

        spawnedEnemies = 0;

        spawnTimer = 0f;
        isWaitingForNextWave = false;
    }

    private void StartWaveCooldown()
    {
        isWaitingForNextWave = true;
        waveCooldownTimer = waveCooldown;

        scoreManager.AddWavePoints();
    }

    private float GetNextSpawnDelay()
    {
        return Random.Range(
            minimumSpawnDelay,
            maximumSpawnDelay
        );
    }

    private void ValidateConfiguration()
    {
        if (startingEnemyCount < 1)
            startingEnemyCount = 1;

        if (enemiesAddedPerWave < 0)
            enemiesAddedPerWave = 0;

        if (minimumSpawnDelay < 0f)
            minimumSpawnDelay = 0f;

        if (maximumSpawnDelay < minimumSpawnDelay)
            maximumSpawnDelay = minimumSpawnDelay;

        if (waveCooldown < 0f)
            waveCooldown = 0f;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetKilledEnemies()
    {
        return spawnedEnemies -
            enemySystem.GetAliveEnemyCount();
    }

    public int GetTotalEnemies()
    {
        return totalEnemies;
    }

    public int GetRemainingCooldownSeconds()
    {
        return Mathf.CeilToInt(waveCooldownTimer);
    }

    public bool IsWaitingForNextWave()
    {
        return isWaitingForNextWave;
    }
}