using TMPro;
using UnityEngine;

public class LeaderboardRow : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private TMP_Text dateText;

    public void SetData(LeaderboardEntry entry, int position)
    {
        positionText.text = $"#{position}";
        nameText.text = entry.playerName;
        scoreText.text = entry.score.ToString();

        if (entry.isHardDifficulty)
            difficultyText.text = "Hard";
        else
            difficultyText.text = "Easy";

        dateText.text = entry.date;
    }
}