using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuManager : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}