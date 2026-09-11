using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Zombie zombie;

    [Header("Attack Animation")]
    [SerializeField] private float attackTiltAngle = 30f;
    [SerializeField] private float attackTiltDuration = 0.2f;
    [SerializeField] private float attackReturnDuration = 0.3f;

    [Header("Death Animation")]
    [SerializeField] private float deathDuration = 0.5f;
    [SerializeField] private float deathGroundDelay = 1f;
    [SerializeField] private float deathGroundHeight = 0.5f;
    [SerializeField] private float deathFallAngle = -90f;

    private float attackAnimationTimer;
    private float deathAnimationTimer;

    private bool previousAttacking;
    private bool isAttacking;
    private bool isReturningFromAttack;
    private bool isDying;
    private bool deathAnimationFinished;

    private Quaternion originalRotation;
    private Quaternion attackRotation;

    private Quaternion deathStartRotation;
    private Quaternion deathEndRotation;

    private Vector3 deathStartPosition;
    private Vector3 deathEndPosition;

    private void Awake()
    {
        originalRotation = transform.rotation;
        previousAttacking = zombie.IsAttacking();

        if (attackTiltDuration <= 0f)
            attackTiltDuration = 0.01f;

        if (attackReturnDuration <= 0f)
            attackReturnDuration = 0.01f;

        if (deathDuration <= 0f)
            deathDuration = 0.01f;

        if (deathGroundDelay < 0f)
            deathGroundDelay = 0f;
    }

    private void Update()
    {
        if (isDying)
        {
            UpdateDeathAnimation();
            return;
        }

        UpdateAttackAnimation();

        bool currentAttacking = zombie.IsAttacking();

        if (currentAttacking && !previousAttacking)
            StartAttackAnimation();

        previousAttacking = currentAttacking;
    }

    private void StartAttackAnimation()
    {
        isAttacking = true;
        isReturningFromAttack = false;
        attackAnimationTimer = 0f;

        Vector3 directionToPlayer =
            zombie.GetPlayer().position - transform.position;

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

        Quaternion playerRotation =
            Quaternion.LookRotation(directionToPlayer);

        attackRotation = playerRotation *
            Quaternion.Euler(attackTiltAngle, 0f, 0f);
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

        float progress =
            attackAnimationTimer / attackTiltDuration;

        if (progress >= 1f)
        {
            progress = 1f;

            zombie.DealAttackDamage();

            isAttacking = false;
            isReturningFromAttack = true;
            attackAnimationTimer = 0f;

            zombie.FinishAttack();
        }

        transform.rotation = Quaternion.Slerp(
            originalRotation,
            attackRotation,
            progress
        );
    }

    private void UpdateAttackReturn()
    {
        attackAnimationTimer += Time.deltaTime;

        float progress =
            attackAnimationTimer / attackReturnDuration;

        if (progress >= 1f)
        {
            progress = 1f;
            isReturningFromAttack = false;
            attackAnimationTimer = 0f;
        }

        transform.rotation = Quaternion.Slerp(
            attackRotation,
            originalRotation,
            progress
        );
    }

    public void StartDeathAnimation()
    {
        if (isDying)
            return;

        isDying = true;
        deathAnimationFinished = false;
        deathAnimationTimer = 0f;

        isAttacking = false;
        isReturningFromAttack = false;

        zombie.StartDeath();

        deathStartPosition = transform.position;

        deathEndPosition = deathStartPosition;
        deathEndPosition.y = deathGroundHeight;

        deathStartRotation = transform.rotation;

        Vector3 directionToPlayer =
            zombie.GetPlayer().position - transform.position;

        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude <= 0f)
        {
            deathEndRotation =
                deathStartRotation *
                Quaternion.Euler(deathFallAngle, 0f, 0f);

            return;
        }

        directionToPlayer.Normalize();

        Vector3 fallAxis =
            Vector3.Cross(Vector3.up, directionToPlayer);

        deathEndRotation = Quaternion.AngleAxis(
            deathFallAngle,
            fallAxis
        ) * deathStartRotation;
    }

    private void UpdateDeathAnimation()
    {
        deathAnimationTimer += Time.deltaTime;

        if (deathAnimationTimer < deathDuration)
        {
            float progress =
                deathAnimationTimer / deathDuration;

            transform.position = Vector3.Lerp(
                deathStartPosition,
                deathEndPosition,
                progress
            );

            transform.rotation = Quaternion.Slerp(
                deathStartRotation,
                deathEndRotation,
                progress
            );

            return;
        }

        transform.position = deathEndPosition;
        transform.rotation = deathEndRotation;

        float groundTimer =
            deathAnimationTimer - deathDuration;

        if (groundTimer < deathGroundDelay)
            return;

        if (deathAnimationFinished)
            return;

        deathAnimationFinished = true;

        zombie.SetReadyToBeDestroyed();
    }

    public bool IsDying()
    {
        return isDying;
    }

    public bool IsDeathAnimationFinished()
    {
        return deathAnimationFinished;
    }
}