using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Text")]
    [SerializeField] private TMPro.TMP_Text sensitivityText;
    [SerializeField] private TMPro.TMP_Text volumeText;

    private bool isLoadingSettings;

    private void Awake()
    {
        ConfigureSliders();
        LoadSettings();
    }

    private void ConfigureSliders()
    {
        sensitivitySlider.minValue = 0f;
        sensitivitySlider.maxValue = 10f;
        sensitivitySlider.wholeNumbers = true;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 10f;
        volumeSlider.wholeNumbers = true;
    }

    private void LoadSettings()
    {
        isLoadingSettings = true;

        int sensitivity = Settings.GetSensitivity();
        int volume = Settings.GetVolume();

        sensitivitySlider.value = sensitivity;
        volumeSlider.value = volume;

        isLoadingSettings = false;

        ApplyVolume(volume);

        UpdateSensitivityText(sensitivity);
        UpdateVolumeText(volume);
    }

    public void OnSensitivityChanged(float value)
    {
        if (isLoadingSettings)
            return;

        int sensitivity = Mathf.RoundToInt(value);

        Settings.SetSensitivity(sensitivity);

        UpdateSensitivityText(sensitivity);
    }

    public void OnVolumeChanged(float value)
    {
        if (isLoadingSettings)
            return;

        int volume = Mathf.RoundToInt(value);

        Settings.SetVolume(volume);

        ApplyVolume(volume);
        UpdateVolumeText(volume);
    }

    public void RestoreDefaultSettings()
    {
        Settings.ResetSettings();

        LoadSettings();
    }

    private void ApplyVolume(int value)
    {
        if (value <= 0)
        {
            audioMixer.SetFloat(
                "MasterVolume",
                -80f
            );

            return;
        }

        float normalizedValue = value / 10f;

        float decibels =
            Mathf.Log10(normalizedValue) * 20f;

        audioMixer.SetFloat(
            "MasterVolume",
            decibels
        );
    }

    private void UpdateSensitivityText(int value)
    {
        sensitivityText.text =
            "Sensibilidad: " + value;
    }

    private void UpdateVolumeText(int value)
    {
        volumeText.text =
            "Volumen: " + value;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}