using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip deathClip;

    private int previousHealth;

    private void Awake()
    {
        previousHealth = playerHealth.GetCurrentHealth();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        UpdateHealthSounds();
    }

    private void UpdateHealthSounds()
    {
        int currentHealth = playerHealth.GetCurrentHealth();

        if (currentHealth >= previousHealth)
        {
            previousHealth = currentHealth;
            return;
        }

        if (currentHealth <= 0)
        {
            if (deathClip != null)
                audioSource.PlayOneShot(deathClip);
        }
        else
        {
            if (damageClip != null)
                audioSource.PlayOneShot(damageClip);
        }

        previousHealth = currentHealth;
    }
}