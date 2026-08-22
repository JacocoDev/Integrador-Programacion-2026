using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private GameManager gameManager;

    [Header("UI")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text gameOverText;

    private void Awake()
    {
        gameOverText.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateWaveUI();
        UpdateGameOverUI();
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = $"{shotgun.GetCurrentAmmo()}/{shotgun.GetMaxAmmo()} Ammo";
    }

    private void UpdateHealthUI()
    {
        healthText.text = $"{playerHealth.GetCurrentHealth()} HP";
    }

    private void UpdateWaveUI()
    {
        if (waveSystem.IsWaitingForNextWave())
        {
            int remainingSeconds = waveSystem.GetRemainingCooldownSeconds();
            waveText.text = $"¡Oleada Superada!\nProxima Oleada en {remainingSeconds} s";
            return;
        }

        int wave = waveSystem.GetCurrentWave();
        int killedZombies = waveSystem.GetKilledZombies();
        int totalZombies = waveSystem.GetTotalZombies();

        waveText.text = $"Oleada {wave}\n(Zombies {killedZombies}/{totalZombies})";
    }

    private void UpdateGameOverUI()
    {
        if (gameManager.IsGameActive())
            return;

        gameOverText.gameObject.SetActive(true);
    }
}