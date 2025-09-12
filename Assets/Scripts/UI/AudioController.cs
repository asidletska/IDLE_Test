using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public void ToggleMusic(bool isOn)
    {
        audioMixer.SetFloat("MusicVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt("MusicToggle", isOn ? 1 : 0);
    }

    public void ToggleSFX(bool isOn)
    {
        audioMixer.SetFloat("SFXVolume", isOn ? 0f : -80f);
        PlayerPrefs.SetInt("SFxToggle", isOn ? 1 : 0);
    }

    public void SliderMusic()
    {
        float volume = musicSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("MusicVolume", dB);

        bool isOn = volume > 0.001f;
        PlayerPrefs.SetInt("MusicToggle", isOn ? 1 : 0);
    }
    public void SliderSFX()
    {
        float volume = sfxSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("SFXVolume", dB);

        bool isOn = volume > 0.001f;
        PlayerPrefs.SetInt("SFxToggle", isOn ? 1 : 0);
    }
    private void Start()
    {
        bool musicOn = PlayerPrefs.GetInt("MusicToggle", 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt("SFxToggle", 1) == 1;
        ToggleMusic(musicOn);
        ToggleSFX(sfxOn);

        musicSlider.value = musicOn ? 1f : 0f;
        sfxSlider.value = sfxOn ? 1f : 0f;
    }

}
