using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameConfigurationMenu : MonoBehaviour
{
    [Header("Direction Buttons")]
    [SerializeField] private Button fourDirectionsButton;
    [SerializeField] private Button sixDirectionsButton;
    [SerializeField] private Button eightDirectionsButton;

    [Header("Difficulty Buttons")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button hardButton;

    private void Awake()
    {
        fourDirectionsButton.onClick.AddListener(SelectFourDirections);
        sixDirectionsButton.onClick.AddListener(SelectSixDirections);
        eightDirectionsButton.onClick.AddListener(SelectEightDirections);

        easyButton.onClick.AddListener(SelectEasy);
        hardButton.onClick.AddListener(SelectHard);

        UpdateDirectionButtons();
        UpdateDifficultyButtons();
    }

    private void SelectFourDirections()
    {
        GameSettings.SetSectorCount(4);
        UpdateDirectionButtons();
    }

    private void SelectSixDirections()
    {
        GameSettings.SetSectorCount(6);
        UpdateDirectionButtons();
    }

    private void SelectEightDirections()
    {
        GameSettings.SetSectorCount(8);
        UpdateDirectionButtons();
    }

    private void SelectEasy()
    {
        GameSettings.SetDifficulty(false);
        UpdateDifficultyButtons();
    }

    private void SelectHard()
    {
        GameSettings.SetDifficulty(true);
        UpdateDifficultyButtons();
    }

    private void UpdateDirectionButtons()
    {
        fourDirectionsButton.interactable = GameSettings.sectorCount != 4;
        sixDirectionsButton.interactable = GameSettings.sectorCount != 6;
        eightDirectionsButton.interactable = GameSettings.sectorCount != 8;
    }

    private void UpdateDifficultyButtons()
    {
        easyButton.interactable = GameSettings.isHardDifficulty;
        hardButton.interactable = !GameSettings.isHardDifficulty;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void CustomizeParameters()
    {
        Debug.Log("Personalizar Parámetros todavía no implementado.");
    }
}