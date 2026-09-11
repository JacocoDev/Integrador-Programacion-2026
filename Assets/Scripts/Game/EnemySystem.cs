using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Zombie zombiePrefab;
    [SerializeField] private SectorSystem sectorSystem;

    [Header("Spawn Configuration")]
    [SerializeField] private float spawnDistance = 12f;

    private List<Zombie> zombies = new();

    private void Update()
    {
        UpdateDeadZombies();
    }

    public void SpawnZombie()
    {
        int sectorCount = sectorSystem.GetSectorCount();
        int sectorIndex = Random.Range(0, sectorCount);

        float sectorAngle = 360f / sectorCount;
        float sectorCenterAngle = sectorIndex * sectorAngle;

        Vector3 spawnPosition =
            GetSpawnPosition(sectorCenterAngle);

        Transform sector =
            sectorSystem.GetSectorTransform(sectorIndex);

        Zombie zombie = Instantiate(
            zombiePrefab,
            spawnPosition,
            Quaternion.identity,
            sector
        );

        zombie.Initialize(
            player,
            playerHealth,
            gameManager
        );

        zombies.Add(zombie);
    }

    public void ShootSector(int sectorIndex)
    {
        Zombie zombie = GetZombieInSector(sectorIndex);

        if (zombie == null)
            return;

        ZombieAnimation zombieAnimation =
            zombie.GetComponent<ZombieAnimation>();

        if (zombieAnimation.IsDying())
            return;

        scoreManager.AddZombiePoints(
            "Normal Zombie",
            1
        );

        ZombieAudio zombieAudio =
            zombie.GetComponent<ZombieAudio>();

        if (zombieAudio != null)
        {
            zombieAudio.PlayDeathSound();
        }

        zombieAnimation.StartDeathAnimation();
    }

    private void UpdateDeadZombies()
    {
        for (int i = zombies.Count - 1; i >= 0; i--)
        {
            Zombie zombie = zombies[i];

            if (zombie == null)
            {
                zombies.RemoveAt(i);
                continue;
            }

            if (!zombie.IsReadyToBeDestroyed())
                continue;

            zombies.RemoveAt(i);
            Destroy(zombie.gameObject);
        }
    }

    private Vector3 GetSpawnPosition(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        float x = Mathf.Sin(radians) * spawnDistance;
        float z = Mathf.Cos(radians) * spawnDistance;

        return new Vector3(x, 0f, z);
    }

    private Zombie GetZombieInSector(int sectorIndex)
    {
        Transform sector =
            sectorSystem.GetSectorTransform(sectorIndex);

        for (int i = 0; i < zombies.Count; i++)
        {
            Zombie zombie = zombies[i];

            if (zombie == null)
                continue;

            if (zombie.transform.parent == sector)
                return zombie;
        }

        return null;
    }

    public int GetAliveZombieCount()
    {
        zombies.RemoveAll(zombie => zombie == null);

        return zombies.Count;
    }
}