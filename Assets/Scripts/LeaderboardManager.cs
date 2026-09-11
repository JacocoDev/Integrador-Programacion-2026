using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public bool isHardDifficulty;
    public int sectorCount;
    public string date;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new();
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard Configuration")]
    [SerializeField] private int maxDisplayedEntries = 25;

    private LeaderboardData leaderboardData = new();
    private string filePath;

    private static bool hasPendingResult;
    private static int pendingScore;
    private static bool pendingIsHardDifficulty;
    private static int pendingSectorCount;
    private static string pendingDate;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderboard();
    }

    private void LoadLeaderboard()
    {
        if (!File.Exists(filePath))
        {
            leaderboardData = new LeaderboardData();
            return;
        }

        string json = File.ReadAllText(filePath);
        leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);

        if (leaderboardData == null)
            leaderboardData = new LeaderboardData();

        if (leaderboardData.entries == null)
            leaderboardData.entries = new List<LeaderboardEntry>();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardData, true);
        File.WriteAllText(filePath, json);
    }

    public void AddEntry(string playerName)
    {
        LeaderboardEntry entry = new();

        entry.playerName = playerName;
        entry.score = pendingScore;
        entry.isHardDifficulty = pendingIsHardDifficulty;
        entry.sectorCount = pendingSectorCount;
        entry.date = pendingDate;

        leaderboardData.entries.Add(entry);

        SortEntries();
        SaveLeaderboard();

        hasPendingResult = false;
    }

    private void SortEntries()
    {
        for (int i = 0; i < leaderboardData.entries.Count - 1; i++)
        {
            for (int j = i + 1; j < leaderboardData.entries.Count; j++)
            {
                LeaderboardEntry firstEntry = leaderboardData.entries[i];
                LeaderboardEntry secondEntry = leaderboardData.entries[j];

                if (secondEntry.score > firstEntry.score)
                {
                    leaderboardData.entries[i] = secondEntry;
                    leaderboardData.entries[j] = firstEntry;
                    continue;
                }

                if (secondEntry.score != firstEntry.score)
                    continue;

                DateTime firstDate = DateTime.ParseExact(firstEntry.date, "dd/MM/yyyy HH:mm", null);
                DateTime secondDate = DateTime.ParseExact(secondEntry.date, "dd/MM/yyyy HH:mm", null);

                if (secondDate < firstDate)
                {
                    leaderboardData.entries[i] = secondEntry;
                    leaderboardData.entries[j] = firstEntry;
                }
            }
        }
    }

    public List<LeaderboardEntry> GetEntries(int sectorCount, string searchText)
    {
        List<LeaderboardEntry> filteredEntries = new();

        string normalizedSearch = searchText.ToLower();

        for (int i = 0; i < leaderboardData.entries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardData.entries[i];

            if (entry.sectorCount != sectorCount)
                continue;

            if (!entry.playerName.ToLower().Contains(normalizedSearch))
                continue;

            filteredEntries.Add(entry);

            if (filteredEntries.Count >= maxDisplayedEntries)
                break;
        }

        return filteredEntries;
    }

    public int GetTotalEntryCount(int sectorCount)
    {
        int count = 0;

        for (int i = 0; i < leaderboardData.entries.Count; i++)
        {
            if (leaderboardData.entries[i].sectorCount == sectorCount)
                count++;
        }

        return count;
    }

    public int GetSearchResultCount(int sectorCount, string searchText)
    {
        int count = 0;
        string normalizedSearch = searchText.ToLower();

        for (int i = 0; i < leaderboardData.entries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardData.entries[i];

            if (entry.sectorCount != sectorCount)
                continue;

            if (!entry.playerName.ToLower().Contains(normalizedSearch))
                continue;

            count++;
        }

        return count;
    }

    public int GetEntryPosition(LeaderboardEntry targetEntry, int sectorCount)
    {
        int position = 1;

        for (int i = 0; i < leaderboardData.entries.Count; i++)
        {
            LeaderboardEntry entry = leaderboardData.entries[i];

            if (entry.sectorCount != sectorCount)
                continue;

            if (entry.score > targetEntry.score)
            {
                position++;
                continue;
            }

            if (entry.score == targetEntry.score)
                break;

            break;
        }

        return position;
    }

    public bool HasPendingResult()
    {
        return hasPendingResult;
    }

    public int GetPendingScore()
    {
        return pendingScore;
    }

    public bool GetPendingDifficulty()
    {
        return pendingIsHardDifficulty;
    }

    public int GetPendingSectorCount()
    {
        return pendingSectorCount;
    }

    public static void SetPendingResult(int score, bool isHardDifficulty, int sectorCount, string date)
    {
        pendingScore = score;
        pendingIsHardDifficulty = isHardDifficulty;
        pendingSectorCount = sectorCount;
        pendingDate = date;
        hasPendingResult = true;
    }
}