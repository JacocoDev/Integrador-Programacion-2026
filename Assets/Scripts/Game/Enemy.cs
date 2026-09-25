using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Preparing,
        Moving,
        Attacking,
        Stunned,
        Dying
    }

    [Header("References")]
    [SerializeField] private EnemyAnimation enemyAnimation;
    [SerializeField] private EnemyAudio enemyAudio;

    private Transform player;
    private PlayerHealth playerHealth;
    private GameManager gameManager;
    private EnemyData enemyData;

    private EnemyState currentState;

    private int currentHealth;

    private float attackCooldownTimer;
    private float stateTimer;

    private bool readyToBeDestroyed;

    private float attackDistance = 2f;

    public void Initialize(
        Transform targetPlayer,
        PlayerHealth targetPlayerHealth,
        GameManager targetGameManager,
        EnemyData targetEnemyData
    )
    {
        player = targetPlayer;
        playerHealth = targetPlayerHealth;
        gameManager = targetGameManager;
        enemyData = targetEnemyData;

        currentHealth = enemyData.GetHealth();
        currentState = EnemyState.Moving;

        attackCooldownTimer = 0f;
        stateTimer = 0f;
        readyToBeDestroyed = false;

        enemyAnimation.Initialize(this);
        enemyAudio.Initialize(this, enemyData);

        ApplyEnemyColor();

        if (enemyData.HasSpawnScream())
        {
            currentState = EnemyState.Preparing;
            stateTimer = enemyData.GetSpawnPreparationDuration();
            enemyAudio.PlaySpawnScream();
        }
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        if (currentState == EnemyState.Dying)
            return;

        if (currentState == EnemyState.Preparing)
        {
            UpdatePreparing();
            return;
        }

        if (currentState == EnemyState.Stunned)
        {
            UpdateStunned();
            return;
        }

        UpdateAttackCooldown();

        if (currentState == EnemyState.Attacking)
            return;

        MoveTowardsPlayer();
        TryAttack();
    }

    private void UpdatePreparing()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer > 0f)
            return;

        currentState = EnemyState.Moving;
    }

    private void UpdateStunned()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer > 0f)
            return;

        currentState = EnemyState.Moving;
    }

    private void UpdateAttackCooldown()
    {
        if (attackCooldownTimer <= 0f)
            return;

        attackCooldownTimer -= Time.deltaTime;
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= attackDistance)
            return;

        transform.position +=
            direction.normalized *
            enemyData.GetMovementSpeed() *
            Time.deltaTime;
    }

    private void TryAttack()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > attackDistance)
            return;

        if (attackCooldownTimer > 0f)
            return;

        attackCooldownTimer = enemyData.GetAttackCooldown();
        currentState = EnemyState.Attacking;
    }

    private void ApplyEnemyColor()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = enemyData.GetEnemyColor();
        }
    }

    public bool TakeDamage()
    {
        if (currentState == EnemyState.Dying)
            return false;

        currentHealth--;

        if (currentHealth <= 0)
        {
            StartDeath();
            return true;
        }

        if (enemyData.HasHitStun())
        {
            currentState = EnemyState.Stunned;
            stateTimer = enemyData.GetHitStunDuration();
            enemyAudio.PlayHitStun();
        }

        return false;
    }

    public void FinishAttack()
    {
        if (currentState != EnemyState.Attacking)
            return;

        currentState = EnemyState.Moving;
    }

    public void DealAttackDamage()
    {
        playerHealth.TakeDamage();
    }

    private void StartDeath()
    {
        currentState = EnemyState.Dying;
        attackCooldownTimer = 0f;
        stateTimer = 0f;
        readyToBeDestroyed = false;

        enemyAnimation.StartDeathAnimation();
        enemyAudio.PlayDeathSound();
    }

    public void SetReadyToBeDestroyed()
    {
        readyToBeDestroyed = true;
    }

    public bool IsAttacking()
    {
        return currentState == EnemyState.Attacking;
    }

    public bool IsDying()
    {
        return currentState == EnemyState.Dying;
    }

    public bool IsReadyToBeDestroyed()
    {
        return readyToBeDestroyed;
    }

    public EnemyData GetEnemyData()
    {
        return enemyData;
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}