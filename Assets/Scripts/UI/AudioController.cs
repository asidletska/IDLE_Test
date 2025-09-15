using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private string savePath;
    private SaveSettings settings;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "audioSettings.json");
        LoadSettings();
    }

    public void ToggleMusic(bool isOn)
    {
        audioMixer.SetFloat("MusicVolume", isOn ? 0f : -80f);
        settings.musicOn = isOn;
        SaveSettings();
    }

    public void ToggleSFX(bool isOn)
    {
        audioMixer.SetFloat("SFXVolume", isOn ? 0f : -80f);
        settings.sfxOn = isOn;
        SaveSettings();
    }

    private void Start()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        audioMixer.SetFloat("MusicVolume", settings.musicOn ? 0f : -80f);
        audioMixer.SetFloat("SFXVolume", settings.sfxOn ? 0f : -80f);
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            settings = JsonUtility.FromJson<SaveSettings>(json);
        }
        else
        {
            settings = new SaveSettings(); 
            SaveSettings();
        }
    }
}
