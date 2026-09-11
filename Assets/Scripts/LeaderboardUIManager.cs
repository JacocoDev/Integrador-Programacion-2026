using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LeaderboardManager leaderboardManager;

    [Header("Registration Panel")]
    [SerializeField] private GameObject registrationPanel;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Text registrationScoreText;
    [SerializeField] private TMP_Text registrationDifficultyText;
    [SerializeField] private TMP_Text registrationDirectionsText;

    [Header("Search")]
    [SerializeField] private TMP_InputField searchInput;

    [Header("Leaderboard Contents")]
    [SerializeField] private Transform fourDirectionsContent;
    [SerializeField] private Transform sixDirectionsContent;
    [SerializeField] private Transform eightDirectionsContent;

    [Header("Leaderboard Scrollbars")]
    [SerializeField] private Scrollbar fourDirectionsScrollbar;
    [SerializeField] private Scrollbar sixDirectionsScrollbar;
    [SerializeField] private Scrollbar eightDirectionsScrollbar;

    [Header("Leaderboard Entry Counts")]
    [SerializeField] private TMP_Text fourDirectionsEntryCountText;
    [SerializeField] private TMP_Text sixDirectionsEntryCountText;
    [SerializeField] private TMP_Text eightDirectionsEntryCountText;

    [Header("Leaderboard Row")]
    [SerializeField] private LeaderboardRow leaderboardRowPrefab;

    private void Awake()
    {
        registrationPanel.SetActive(false);

        searchInput.onValueChanged.AddListener(OnSearchValueChanged);
    }

    private void Start()
    {
        UpdateRegistrationPanel();
        UpdateLeaderboardsUI();
    }

    private void UpdateRegistrationPanel()
    {
        if (!leaderboardManager.HasPendingResult())
        {
            registrationPanel.SetActive(false);
            return;
        }

        registrationPanel.SetActive(true);

        registrationScoreText.text = $"Final Score: {leaderboardManager.GetPendingScore()}";

        if (leaderboardManager.GetPendingDifficulty())
            registrationDifficultyText.text = "Difficulty: Hard";
        else
            registrationDifficultyText.text = "Difficulty: Easy";

        registrationDirectionsText.text = $"Directions: {leaderboardManager.GetPendingSectorCount()}";
    }

    private void UpdateLeaderboardsUI()
    {
        ClearLeaderboard(fourDirectionsContent);
        ClearLeaderboard(sixDirectionsContent);
        ClearLeaderboard(eightDirectionsContent);

        CreateLeaderboard(fourDirectionsContent, fourDirectionsEntryCountText, 4);
        CreateLeaderboard(sixDirectionsContent, sixDirectionsEntryCountText, 6);
        CreateLeaderboard(eightDirectionsContent, eightDirectionsEntryCountText, 8);

        ResetScrollbars();
    }

    private void CreateLeaderboard(Transform content, TMP_Text entryCountText, int sectorCount)
    {
        string searchText = searchInput.text;

        List<LeaderboardEntry> entries = leaderboardManager.GetEntries(sectorCount, searchText);

        if (searchText == "")
        {
            int totalEntries = leaderboardManager.GetTotalEntryCount(sectorCount);
            entryCountText.text = $"{totalEntries} Entries";
        }
        else
        {
            int resultCount = leaderboardManager.GetSearchResultCount(sectorCount, searchText);
            entryCountText.text = $"{resultCount} Results";
        }

        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardEntry entry = entries[i];
            int position = leaderboardManager.GetEntryPosition(entry, sectorCount);

            LeaderboardRow row = Instantiate(leaderboardRowPrefab, content);
            row.SetData(entry, position);
        }
    }

    private void ClearLeaderboard(Transform content)
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void ResetScrollbars()
    {
        fourDirectionsScrollbar.value = 1f;
        sixDirectionsScrollbar.value = 1f;
        eightDirectionsScrollbar.value = 1f;
    }

    private void OnSearchValueChanged(string searchText)
    {
        UpdateLeaderboardsUI();
    }

    public void SubmitScore()
    {
        string playerName = nameInput.text.Trim();

        if (playerName == "")
            return;

        leaderboardManager.AddEntry(playerName);

        nameInput.text = "";
        registrationPanel.SetActive(false);

        UpdateLeaderboardsUI();
    }

    public void SkipScore()
    {
        registrationPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}