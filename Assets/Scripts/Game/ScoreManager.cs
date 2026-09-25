using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Configuration")]
    [SerializeField] private float easyMultiplier = 1f;
    [SerializeField] private float hardMultiplier = 1.5f;
    [SerializeField] private int wavePoints = 25;

    private int baseScore;
    private int completedWaves;

    private Dictionary<string, int> enemyKillCounts = new();
    private Dictionary<string, int> enemyPoints = new();
    private Queue<string> scoreNotifications = new();

    private float scoreMultiplier;
    private string gameStartDate;

    private void Awake()
    {
        SetScoreMultiplier();
        gameStartDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }

    private void SetScoreMultiplier()
    {
        if (GameSettings.isHardDifficulty)
        {
            scoreMultiplier = hardMultiplier;
            return;
        }

        scoreMultiplier = easyMultiplier;
    }

    public void AddEnemyPoints(string enemyName, int points)
    {
        baseScore += points;

        if (enemyKillCounts.ContainsKey(enemyName))
        {
            enemyKillCounts[enemyName]++;
            enemyPoints[enemyName] += points;
        }
        else
        {
            enemyKillCounts.Add(enemyName, 1);
            enemyPoints.Add(enemyName, points);
        }

        scoreNotifications.Enqueue(
            $"+{points} {enemyName}"
        );
    }

    public void AddWavePoints()
    {
        baseScore += wavePoints;
        completedWaves++;

        scoreNotifications.Enqueue(
            $"+{wavePoints} Wave"
        );
    }

    public int GetCurrentScore()
    {
        return baseScore;
    }

    public int GetFinalScore()
    {
        return Mathf.RoundToInt(
            baseScore * scoreMultiplier
        );
    }

    public int GetCompletedWaves()
    {
        return completedWaves;
    }

    public string GetGameStartDate()
    {
        return gameStartDate;
    }

    public bool HasScoreNotification()
    {
        return scoreNotifications.Count > 0;
    }

    public string GetNextScoreNotification()
    {
        if (scoreNotifications.Count == 0)
            return "";

        return scoreNotifications.Dequeue();
    }

    public string GetScoreSummary()
    {
        StringBuilder summary = new();

        if (completedWaves > 0)
        {
            int totalWavePoints =
                completedWaves * wavePoints;

            summary.Append(
                $"+{totalWavePoints} Waves Completed (x{completedWaves})"
            );
        }

        foreach (
            KeyValuePair<string, int> enemyEntry
            in enemyKillCounts
        )
        {
            string enemyName = enemyEntry.Key;
            int killCount = enemyEntry.Value;
            int totalEnemyPoints = enemyPoints[enemyName];

            if (summary.Length > 0)
                summary.Append("\n");

            summary.Append(
                $"+{totalEnemyPoints} {enemyName}s (x{killCount})"
            );
        }

        return summary.ToString();
    }
}