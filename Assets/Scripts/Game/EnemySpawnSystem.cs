using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [System.Serializable]
    private class EnemyProbabilitySettings
    {
        [SerializeField] private EnemyData enemyData;

        [Header("Easy Difficulty")]
        [SerializeField] private float easyPeakWave = 1f;

        [Header("Hard Difficulty")]
        [SerializeField] private float hardPeakWave = 1f;

        [Header("Spread")]
        [SerializeField] private float spread = 2f;

        public EnemyData GetEnemyData()
        {
            return enemyData;
        }

        public float GetPeakWave()
        {
            if (GameSettings.isHardDifficulty)
                return hardPeakWave;

            return easyPeakWave;
        }

        public float GetSpread()
        {
            return spread;
        }
    }

    [Header("Enemy Probability Curves")]
    [SerializeField] private EnemyProbabilitySettings normalZombie;
    [SerializeField] private EnemyProbabilitySettings bruteZombie;
    [SerializeField] private EnemyProbabilitySettings fastZombie;
    [SerializeField] private EnemyProbabilitySettings tankZombie;

    [Header("Current Probabilities")]
    [SerializeField] private float currentNormalProbability;
    [SerializeField] private float currentBruteProbability;
    [SerializeField] private float currentFastProbability;
    [SerializeField] private float currentTankProbability;

    private int calculatedWave = -1;
    private bool calculatedHardDifficulty;

    public EnemyData GetEnemyDataForWave(int wave)
    {
        UpdateProbabilities(wave);

        float randomValue = Random.value;

        if (randomValue < currentNormalProbability)
            return normalZombie.GetEnemyData();

        randomValue -= currentNormalProbability;

        if (randomValue < currentBruteProbability)
            return bruteZombie.GetEnemyData();

        randomValue -= currentBruteProbability;

        if (randomValue < currentFastProbability)
            return fastZombie.GetEnemyData();

        return tankZombie.GetEnemyData();
    }

    private void UpdateProbabilities(int wave)
    {
        if (
            calculatedWave == wave &&
            calculatedHardDifficulty == GameSettings.isHardDifficulty
        )
            return;

        calculatedWave = wave;
        calculatedHardDifficulty = GameSettings.isHardDifficulty;

        float normalValue = CalculateGaussian(
            wave,
            normalZombie.GetPeakWave(),
            normalZombie.GetSpread()
        );

        float bruteValue = CalculateGaussian(
            wave,
            bruteZombie.GetPeakWave(),
            bruteZombie.GetSpread()
        );

        float fastValue = CalculateGaussian(
            wave,
            fastZombie.GetPeakWave(),
            fastZombie.GetSpread()
        );

        float tankValue = CalculateGaussian(
            wave,
            tankZombie.GetPeakWave(),
            tankZombie.GetSpread()
        );

        float totalValue =
            normalValue +
            bruteValue +
            fastValue +
            tankValue;

        currentNormalProbability =
            normalValue / totalValue;

        currentBruteProbability =
            bruteValue / totalValue;

        currentFastProbability =
            fastValue / totalValue;

        currentTankProbability =
            tankValue / totalValue;
    }

    private float CalculateGaussian(
        float wave,
        float peakWave,
        float spread
    )
    {
        float difference = wave - peakWave;

        return Mathf.Exp(
            -(difference * difference) /
            (2f * spread * spread)
        );
    }

    public float GetNormalProbability()
    {
        return currentNormalProbability;
    }

    public float GetBruteProbability()
    {
        return currentBruteProbability;
    }

    public float GetFastProbability()
    {
        return currentFastProbability;
    }

    public float GetTankProbability()
    {
        return currentTankProbability;
    }
}