using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private AudioSource groanSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] private AudioSource specialSource;

    private Enemy enemy;
    private EnemyData enemyData;

    private float footstepsTimer;
    private float groanTimer;

    private bool previousAttacking;

    public void Initialize(
        Enemy targetEnemy,
        EnemyData targetEnemyData
    )
    {
        enemy = targetEnemy;
        enemyData = targetEnemyData;

        ConfigureAudioSources();

        footstepsTimer = Random.Range(
            enemyData.GetMinimumFootstepInterval(),
            enemyData.GetMaximumFootstepInterval()
        );

        groanTimer = Random.Range(
            enemyData.GetMinimumGroanInterval(),
            enemyData.GetMaximumGroanInterval()
        );

        previousAttacking = enemy.IsAttacking();
    }

    private void Awake()
    {
        ConfigureAudioSources();
    }

    private void Update()
    {
        if (enemy.IsDying())
        {
            previousAttacking = enemy.IsAttacking();
            return;
        }

        if (enemy.IsAttacking())
        {
            previousAttacking = enemy.IsAttacking();
            return;
        }

        UpdateFootsteps();
        UpdateGroans();
        UpdateAttackSound();

        previousAttacking = enemy.IsAttacking();
    }

    private void UpdateFootsteps()
    {
        AudioClip clip = enemyData.GetFootstepsClip();

        if (clip == null)
            return;

        footstepsTimer -= Time.deltaTime;

        if (footstepsTimer > 0f)
            return;

        footstepsSource.PlayOneShot(clip);

        footstepsTimer = Random.Range(
            enemyData.GetMinimumFootstepInterval(),
            enemyData.GetMaximumFootstepInterval()
        );
    }

    private void UpdateGroans()
    {
        AudioClip clip = enemyData.GetGroanClip();

        if (clip == null)
            return;

        groanTimer -= Time.deltaTime;

        if (groanTimer > 0f)
            return;

        groanSource.PlayOneShot(clip);

        groanTimer = Random.Range(
            enemyData.GetMinimumGroanInterval(),
            enemyData.GetMaximumGroanInterval()
        );
    }

    private void UpdateAttackSound()
    {
        bool currentAttacking = enemy.IsAttacking();

        if (currentAttacking == previousAttacking)
            return;

        if (!currentAttacking)
            return;

        AudioClip clip = enemyData.GetAttackClip();

        if (clip != null)
            attackSource.PlayOneShot(clip);
    }

    public void PlaySpawnScream()
    {
        AudioClip clip = enemyData.GetSpawnScreamClip();

        if (clip == null)
            return;

        specialSource.PlayOneShot(clip);
    }

    public void PlayHitStun()
    {
        AudioClip clip = enemyData.GetHitStunClip();

        if (clip == null)
            return;

        specialSource.PlayOneShot(clip);
    }

    public void StopNormalSounds()
    {
        footstepsSource.Stop();
        groanSource.Stop();
        attackSource.Stop();
    }

    public void PlayDeathSound()
    {
        StopNormalSounds();

        AudioClip clip = enemyData.GetDeathClip();

        if (clip == null)
            return;

        deathSource.PlayOneShot(clip);
    }

    private void ConfigureAudioSources()
    {
        footstepsSource.playOnAwake = false;
        footstepsSource.loop = false;
        footstepsSource.spatialBlend = 1f;

        groanSource.playOnAwake = false;
        groanSource.loop = false;
        groanSource.spatialBlend = 1f;

        attackSource.playOnAwake = false;
        attackSource.loop = false;
        attackSource.spatialBlend = 1f;

        deathSource.playOnAwake = false;
        deathSource.loop = false;
        deathSource.spatialBlend = 1f;

        specialSource.playOnAwake = false;
        specialSource.loop = false;
        specialSource.spatialBlend = 1f;
    }
}