using UnityEngine;

public class WaveAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveSystem waveSystem;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip roundStartClip;
    [SerializeField] private AudioClip roundEndClip;

    private int previousWave;
    private bool previousWaitingForNextWave;

    private void Awake()
    {
        previousWave = waveSystem.GetCurrentWave();
        previousWaitingForNextWave = waveSystem.IsWaitingForNextWave();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        UpdateRoundSounds();

        previousWave = waveSystem.GetCurrentWave();
        previousWaitingForNextWave = waveSystem.IsWaitingForNextWave();
    }

    private void UpdateRoundSounds()
    {
        int currentWave = waveSystem.GetCurrentWave();
        bool currentWaitingForNextWave = waveSystem.IsWaitingForNextWave();

        if (currentWaitingForNextWave && !previousWaitingForNextWave)
        {
            PlayRoundEndSound();
        }

        if (currentWave != previousWave)
        {
            PlayRoundStartSound();
        }
    }

    private void PlayRoundStartSound()
    {
        if (roundStartClip == null)
            return;

        audioSource.PlayOneShot(roundStartClip);
    }

    private void PlayRoundEndSound()
    {
        if (roundEndClip == null)
            return;

        audioSource.PlayOneShot(roundEndClip);
    }
}