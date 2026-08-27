using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = true;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (!isGameActive)
            return;

        isGameActive = false;
        Time.timeScale = 0f;
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}