using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameManager gameManager;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float attackDistance = 1f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    private float attackCooldownTimer;

    private bool isAttacking;
    private bool isDying;
    private bool readyToBeDestroyed;

    private void Awake()
    {
        movementSpeed = GameSettings.zombieSpeed;
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        if (isDying)
            return;

        UpdateAttackCooldown();

        if (isAttacking)
            return;

        MoveTowardsPlayer();
        TryAttack();
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
            movementSpeed *
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

        attackCooldownTimer = attackCooldown;
        isAttacking = true;
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    public void DealAttackDamage()
    {
        playerHealth.TakeDamage();
    }

    public void StartDeath()
    {
        if (isDying)
            return;

        isDying = true;
        isAttacking = false;
        readyToBeDestroyed = false;
    }

    public void SetReadyToBeDestroyed()
    {
        readyToBeDestroyed = true;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsDying()
    {
        return isDying;
    }

    public bool IsReadyToBeDestroyed()
    {
        return readyToBeDestroyed;
    }

    public void Initialize(
        Transform targetPlayer,
        PlayerHealth targetPlayerHealth,
        GameManager targetGameManager
    )
    {
        player = targetPlayer;
        playerHealth = targetPlayerHealth;
        gameManager = targetGameManager;
    }

    public Transform GetPlayer()
    {
        return player;
    }
}