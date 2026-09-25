using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Game/Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class DifficultyValues
    {
        [Header("Movement")]
        public float movementSpeed = 1f;

        [Header("Attack")]
        public float attackCooldown = 1.5f;

        [Header("Special Behavior")]
        public float spawnPreparationDuration = 0f;
        public float hitStunDuration = 0f;
    }

    [Header("Identity")]
    [SerializeField] private string enemyName = "Normal Zombie";
    [SerializeField] private Color enemyColor = Color.white;

    [Header("Combat")]
    [SerializeField] private int health = 1;
    [SerializeField] private int scorePoints = 1;

    [Header("Behavior")]
    [SerializeField] private bool hasSpawnScream;
    [SerializeField] private bool hasHitStun;

    [Header("Easy Difficulty")]
    [SerializeField] private DifficultyValues easyValues;

    [Header("Hard Difficulty")]
    [SerializeField] private DifficultyValues hardValues;

    [Header("Audio")]
    [SerializeField] private AudioClip footstepsClip;
    [SerializeField] private AudioClip groanClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip spawnScreamClip;
    [SerializeField] private AudioClip hitStunClip;

    [Header("Audio Intervals")]
    [SerializeField] private float minimumFootstepInterval = 0.35f;
    [SerializeField] private float maximumFootstepInterval = 0.55f;

    [SerializeField] private float minimumGroanInterval = 3f;
    [SerializeField] private float maximumGroanInterval = 7f;

    public string GetEnemyName()
    {
        return enemyName;
    }

    public Color GetEnemyColor()
    {
        return enemyColor;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetScorePoints()
    {
        return scorePoints;
    }

    public bool HasSpawnScream()
    {
        return hasSpawnScream;
    }

    public bool HasHitStun()
    {
        return hasHitStun;
    }

    public DifficultyValues GetDifficultyValues()
    {
        if (GameSettings.isHardDifficulty)
            return hardValues;

        return easyValues;
    }

    public float GetSpawnPreparationDuration()
    {
        DifficultyValues values = GetDifficultyValues();
        return values.spawnPreparationDuration;
    }

    public float GetHitStunDuration()
    {
        DifficultyValues values = GetDifficultyValues();
        return values.hitStunDuration;
    }

    public float GetMovementSpeed()
    {
        DifficultyValues values = GetDifficultyValues();
        return values.movementSpeed;
    }

    public float GetAttackCooldown()
    {
        DifficultyValues values = GetDifficultyValues();
        return values.attackCooldown;
    }

    public AudioClip GetFootstepsClip()
    {
        return footstepsClip;
    }

    public AudioClip GetGroanClip()
    {
        return groanClip;
    }

    public AudioClip GetAttackClip()
    {
        return attackClip;
    }

    public AudioClip GetDeathClip()
    {
        return deathClip;
    }

    public AudioClip GetSpawnScreamClip()
    {
        return spawnScreamClip;
    }

    public AudioClip GetHitStunClip()
    {
        return hitStunClip;
    }

    public float GetMinimumFootstepInterval()
    {
        return minimumFootstepInterval;
    }

    public float GetMaximumFootstepInterval()
    {
        return maximumFootstepInterval;
    }

    public float GetMinimumGroanInterval()
    {
        return minimumGroanInterval;
    }

    public float GetMaximumGroanInterval()
    {
        return maximumGroanInterval;
    }
}