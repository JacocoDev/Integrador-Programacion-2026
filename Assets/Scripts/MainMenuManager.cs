using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameConfigMenu");
    }

    public void OpenLeaderboards()
    {
        Debug.Log("Leaderboards todavía no implementado.");
    }

    public void OpenSettings()
    {
        Debug.Log("Configuración todavía no implementada.");
    }

    public void OpenCredits()
    {
        Debug.Log("Créditos todavía no implementados.");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}