public static class GameSettings
{
    public static int sectorCount = 4;

    public static bool isHardDifficulty = false;

    public static int playerHealth = 3;
    public static int shotgunAmmo = 6;
    public static float zombieSpeed = 1f;
    public static float minimumSpawnDelay = 4f;
    public static float maximumSpawnDelay = 8f;

    public static void SetSectorCount(int count)
    {
        sectorCount = count;
    }

    public static void SetDifficulty(bool hard)
    {
        isHardDifficulty = hard;

        if (hard)
        {
            playerHealth = 1;
            shotgunAmmo = 6;
            zombieSpeed = 1.5f;
            minimumSpawnDelay = 3f;
            maximumSpawnDelay = 6f;
            return;
        }

        playerHealth = 3;
        shotgunAmmo = 6;
        zombieSpeed = 1f;
        minimumSpawnDelay = 4f;
        maximumSpawnDelay = 8f;
    }
}