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
    [SerializeField] private ScoreManager scoreManager;

    [Header("UI")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scorePopupText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text scoreSummaryText;
    [SerializeField] private GameObject leaderboardsButton;

    [Header("Score Popup")]
    [SerializeField] private float scorePopupDuration = 1.5f;

    private float scorePopupTimer;
    private bool isShowingScorePopup;
    private bool hasShownGameOverUI;

    private void Awake()
    {
        gameOverText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        scoreSummaryText.gameObject.SetActive(false);
        scorePopupText.gameObject.SetActive(false);
        leaderboardsButton.SetActive(false);
    }

    private void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateWaveUI();
        UpdateScoreUI();
        UpdateScorePopupUI();
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

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {scoreManager.GetCurrentScore()}";
    }

    private void UpdateScorePopupUI()
    {
        if (isShowingScorePopup)
        {
            scorePopupTimer -= Time.deltaTime;

            if (scorePopupTimer <= 0f)
            {
                isShowingScorePopup = false;
                scorePopupText.gameObject.SetActive(false);
            }

            return;
        }

        if (!scoreManager.HasScoreNotification())
            return;

        scorePopupText.text = scoreManager.GetNextScoreNotification();
        scorePopupText.gameObject.SetActive(true);

        scorePopupTimer = scorePopupDuration;
        isShowingScorePopup = true;
    }

    private void UpdateGameOverUI()
    {
        if (gameManager.IsGameActive())
            return;

        if (hasShownGameOverUI)
            return;

        hasShownGameOverUI = true;

        gameOverText.gameObject.SetActive(true);
        finalScoreText.gameObject.SetActive(true);
        scoreSummaryText.gameObject.SetActive(true);
        leaderboardsButton.SetActive(true);

        finalScoreText.text = $"Final Score: {scoreManager.GetFinalScore()}";
        scoreSummaryText.text = scoreManager.GetScoreSummary();
    }

    public void OpenLeaderboards()
    {
        LeaderboardManager.SetPendingResult(
            scoreManager.GetFinalScore(),
            GameSettings.isHardDifficulty,
            GameSettings.sectorCount,
            scoreManager.GetGameStartDate()
        );

        Time.timeScale = 1f;
        SceneManager.LoadScene("Leaderboards");
    }
}