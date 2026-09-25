using UnityEngine;

public static class Settings
{
    private static string sensitivityKey = "Sensitivity";
    private static string volumeKey = "Volume";

    private static int defaultSensitivity = 5;
    private static int defaultVolume = 10;

    public static int GetSensitivity()
    {
        return PlayerPrefs.GetInt(
            sensitivityKey,
            defaultSensitivity
        );
    }

    public static int GetVolume()
    {
        return PlayerPrefs.GetInt(
            volumeKey,
            defaultVolume
        );
    }

    public static void SetSensitivity(int value)
    {
        PlayerPrefs.SetInt(sensitivityKey, value);
        PlayerPrefs.Save();
    }

    public static void SetVolume(int value)
    {
        PlayerPrefs.SetInt(volumeKey, value);
        PlayerPrefs.Save();
    }

    public static void ResetSettings()
    {
        PlayerPrefs.SetInt(
            sensitivityKey,
            defaultSensitivity
        );

        PlayerPrefs.SetInt(
            volumeKey,
            defaultVolume
        );

        PlayerPrefs.Save();
    }
}