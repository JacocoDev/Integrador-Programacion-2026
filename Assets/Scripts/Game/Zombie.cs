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

    [Header("Attack Animation")]
    [SerializeField] private float attackTiltAngle = 30f;
    [SerializeField] private float attackTiltDuration = 0.2f;
    [SerializeField] private float attackReturnDuration = 0.3f;

    private float attackCooldownTimer;
    private float attackAnimationTimer;

    private bool isAttacking;
    private bool isReturningFromAttack;

    private Quaternion originalRotation;
    private Quaternion attackRotation;

    private void Awake()
    {
        movementSpeed = GameSettings.zombieSpeed;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        UpdateAttackCooldown();
        UpdateAttackAnimation();

        if (isAttacking || isReturningFromAttack)
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

        transform.position += direction.normalized * movementSpeed * Time.deltaTime;
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
        StartAttackAnimation();
    }

    private void StartAttackAnimation()
    {
        isAttacking = true;
        isReturningFromAttack = false;
        attackAnimationTimer = 0f;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude <= 0f)
        {
            attackRotation = Quaternion.Euler(
                transform.eulerAngles.x - attackTiltAngle,
                transform.eulerAngles.y,
                transform.eulerAngles.z
            );

            return;
        }

        Quaternion playerRotation = Quaternion.LookRotation(directionToPlayer);
        attackRotation = playerRotation * Quaternion.Euler(attackTiltAngle, 0f, 0f);
    }

    private void UpdateAttackAnimation()
    {
        if (!isAttacking && !isReturningFromAttack)
            return;

        if (isAttacking)
        {
            UpdateAttackTilt();
            return;
        }

        UpdateAttackReturn();
    }

    private void UpdateAttackTilt()
    {
        attackAnimationTimer += Time.deltaTime;

        float progress = attackAnimationTimer / attackTiltDuration;

        if (progress >= 1f)
        {
            progress = 1f;
            playerHealth.TakeDamage();

            isAttacking = false;
            isReturningFromAttack = true;
            attackAnimationTimer = 0f;
        }

        transform.rotation = Quaternion.Slerp(originalRotation, attackRotation, progress);
    }

    private void UpdateAttackReturn()
    {
        attackAnimationTimer += Time.deltaTime;

        float progress = attackAnimationTimer / attackReturnDuration;

        if (progress >= 1f)
        {
            progress = 1f;
            isReturningFromAttack = false;
            attackAnimationTimer = 0f;
        }

        transform.rotation = Quaternion.Slerp(attackRotation, originalRotation, progress);
    }

    public void Initialize(Transform targetPlayer, PlayerHealth targetPlayerHealth, GameManager targetGameManager)
    {
        player = targetPlayer;
        playerHealth = targetPlayerHealth;
        gameManager = targetGameManager;
    }
}