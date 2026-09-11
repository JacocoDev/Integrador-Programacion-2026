using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Zombie zombie;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private AudioSource groanSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource deathSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip footstepsClip;
    [SerializeField] private AudioClip groanClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip deathClip;

    [Header("Footsteps")]
    [SerializeField] private float minimumFootstepInterval = 0.35f;
    [SerializeField] private float maximumFootstepInterval = 0.55f;

    [Header("Groans")]
    [SerializeField] private float minimumGroanInterval = 3f;
    [SerializeField] private float maximumGroanInterval = 7f;

    private float footstepsTimer;
    private float groanTimer;

    private bool previousAttacking;

    private void Awake()
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

        footstepsTimer = Random.Range(
            minimumFootstepInterval,
            maximumFootstepInterval
        );

        groanTimer = Random.Range(
            minimumGroanInterval,
            maximumGroanInterval
        );

        previousAttacking = zombie.IsAttacking();
    }

    private void Update()
    {
        if (zombie.IsDying())
        {
            previousAttacking = zombie.IsAttacking();
            return;
        }

        UpdateFootsteps();
        UpdateGroans();
        UpdateAttackSound();

        previousAttacking = zombie.IsAttacking();
    }

    private void UpdateFootsteps()
    {
        if (footstepsClip == null)
            return;

        if (zombie.IsAttacking())
            return;

        footstepsTimer -= Time.deltaTime;

        if (footstepsTimer > 0f)
            return;

        footstepsSource.PlayOneShot(footstepsClip);

        footstepsTimer = Random.Range(
            minimumFootstepInterval,
            maximumFootstepInterval
        );
    }

    private void UpdateGroans()
    {
        if (groanClip == null)
            return;

        if (zombie.IsAttacking())
            return;

        groanTimer -= Time.deltaTime;

        if (groanTimer > 0f)
            return;

        groanSource.PlayOneShot(groanClip);

        groanTimer = Random.Range(
            minimumGroanInterval,
            maximumGroanInterval
        );
    }

    private void UpdateAttackSound()
    {
        bool currentAttacking = zombie.IsAttacking();

        if (currentAttacking == previousAttacking)
            return;

        if (!currentAttacking)
            return;

        if (attackClip != null)
            attackSource.PlayOneShot(attackClip);
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

        if (deathClip == null)
            return;

        deathSource.PlayOneShot(deathClip);
    }
}