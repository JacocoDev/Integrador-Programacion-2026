using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private GameManager gameManager;

    [Header("Wave Configuration")]
    [SerializeField] private int startingZombieCount = 5;
    [SerializeField] private float minimumSpawnDelay = 4f;
    [SerializeField] private float maximumSpawnDelay = 8f;
    [SerializeField] private float waveCooldown = 5f;

    private int currentWave;
    private int totalZombies;
    private int spawnedZombies;

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
        if (spawnedZombies >= totalZombies)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        enemySystem.SpawnZombie();
        spawnedZombies++;

        if (spawnedZombies < totalZombies)
            spawnTimer = GetNextSpawnDelay();
    }

    private void UpdateWaveCompletion()
    {
        if (spawnedZombies < totalZombies)
            return;

        if (enemySystem.GetAliveZombieCount() > 0)
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
        totalZombies = startingZombieCount + currentWave - 1;
        spawnedZombies = 0;
        spawnTimer = 0f;
        isWaitingForNextWave = false;
    }

    private void StartWaveCooldown()
    {
        isWaitingForNextWave = true;
        waveCooldownTimer = waveCooldown;
    }

    private float GetNextSpawnDelay()
    {
        return Random.Range(minimumSpawnDelay, maximumSpawnDelay);
    }

    private void ValidateConfiguration()
    {
        if (startingZombieCount < 1)
            startingZombieCount = 1;

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

    public int GetKilledZombies()
    {
        return spawnedZombies - enemySystem.GetAliveZombieCount();
    }

    public int GetTotalZombies()
    {
        return totalZombies;
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