public static class GameSettings
{
    public static int sectorCount = 4;

    public static bool isHardDifficulty = false;

    public static int playerHealth = 3;
    public static int shotgunAmmo = 6;

    public static float minimumSpawnDelay = 3f;
    public static float maximumSpawnDelay = 5f;

    public static void Reset()
    {
        sectorCount = 4;
        isHardDifficulty = false;

        playerHealth = 3;
        shotgunAmmo = 6;

        minimumSpawnDelay = 3f;
        maximumSpawnDelay = 5f;
    }

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

            minimumSpawnDelay = 1.5f;
            maximumSpawnDelay = 3f;

            return;
        }

        playerHealth = 3;
        shotgunAmmo = 6;

        minimumSpawnDelay = 3f;
        maximumSpawnDelay = 5f;
    }
}