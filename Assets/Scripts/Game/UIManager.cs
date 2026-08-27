using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject mainMenuButton;

    private void Awake()
    {
        gameOverText.gameObject.SetActive(false);
        mainMenuButton.SetActive(false);
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
            waveText.text = $"Wave Completed!\nNext Wave in {remainingSeconds} s";
            return;
        }

        int wave = waveSystem.GetCurrentWave();
        int killedZombies = waveSystem.GetKilledZombies();
        int totalZombies = waveSystem.GetTotalZombies();

        waveText.text = $"Wave {wave}\n(Zombies {killedZombies}/{totalZombies})";
    }

    private void UpdateGameOverUI()
    {
        if (gameManager.IsGameActive())
            return;

        gameOverText.gameObject.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}