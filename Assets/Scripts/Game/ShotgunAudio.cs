using UnityEngine;

public class ShotgunAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shotgun shotgun;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip loadShellClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip chamberClip;

    [Header("Chamber")]
    [SerializeField] private float chamberDelay = 0.6f;

    [Header("Error")]
    [SerializeField] private float errorDoubleInterval = 0.15f;

    private int previousShotCount;
    private int previousErrorCount;
    private Shotgun.ReloadPhase previousReloadPhase;

    private float chamberTimer;
    private bool chamberPending;

    private float errorDoubleTimer;
    private bool errorDoublePending;

    private void Awake()
    {
        previousShotCount = shotgun.GetShotCount();
        previousErrorCount = shotgun.GetErrorCount();
        previousReloadPhase = shotgun.GetReloadPhase();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;

        if (chamberDelay < 0f)
            chamberDelay = 0f;

        if (errorDoubleInterval < 0f)
            errorDoubleInterval = 0f;
    }

    private void Update()
    {
        UpdateShootSound();
        UpdateReloadSound();
        UpdateErrorSound();
        UpdateChamberSound();
        UpdateErrorDoubleSound();

        previousShotCount = shotgun.GetShotCount();
        previousErrorCount = shotgun.GetErrorCount();
        previousReloadPhase = shotgun.GetReloadPhase();
    }

    private void UpdateShootSound()
    {
        int currentShotCount = shotgun.GetShotCount();

        if (currentShotCount <= previousShotCount)
            return;

        if (shootClip != null)
            audioSource.PlayOneShot(shootClip);

        chamberTimer = chamberDelay;
        chamberPending = true;
    }

    private void UpdateReloadSound()
    {
        Shotgun.ReloadPhase currentPhase = shotgun.GetReloadPhase();

        if (currentPhase == previousReloadPhase)
            return;

        if (currentPhase == Shotgun.ReloadPhase.Lowering)
        {
            if (reloadClip != null)
                audioSource.PlayOneShot(reloadClip);
        }

        if (currentPhase == Shotgun.ReloadPhase.Loading)
        {
            if (loadShellClip != null)
                audioSource.PlayOneShot(loadShellClip);
        }
    }

    private void UpdateErrorSound()
    {
        int currentErrorCount = shotgun.GetErrorCount();

        if (currentErrorCount <= previousErrorCount)
            return;

        if (shotgun.GetCurrentAmmo() <= 0)
        {
            if (errorClip != null)
                audioSource.PlayOneShot(errorClip);

            errorDoublePending = false;
            return;
        }

        if (shotgun.GetCurrentAmmo() >= shotgun.GetMaxAmmo())
        {
            if (errorClip != null)
                audioSource.PlayOneShot(errorClip);

            errorDoubleTimer = errorDoubleInterval;
            errorDoublePending = true;
        }
    }

    private void UpdateErrorDoubleSound()
    {
        if (!errorDoublePending)
            return;

        errorDoubleTimer -= Time.deltaTime;

        if (errorDoubleTimer > 0f)
            return;

        errorDoublePending = false;

        if (errorClip != null)
            audioSource.PlayOneShot(errorClip);
    }

    private void UpdateChamberSound()
    {
        if (!chamberPending)
            return;

        chamberTimer -= Time.deltaTime;

        if (chamberTimer > 0f)
            return;

        chamberPending = false;

        if (chamberClip != null)
            audioSource.PlayOneShot(chamberClip);
    }
}