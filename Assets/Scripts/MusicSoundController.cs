using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSoundController : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;

    [Header("Sounds")]
    public AudioSource soundSource;

    [Header("UI Sliders")]
    public Slider soundSlider;
    public Slider musicSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    void Start()
    {
        // MUSIC - default 0.5
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        musicSource.volume = savedMusicVolume;
        musicSlider.value = savedMusicVolume;
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // SOUND - default 0.5
        float savedSoundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
        soundSource.volume = savedSoundVolume;
        soundSlider.value = savedSoundVolume;
        soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    void OnSoundVolumeChanged(float value)
    {
        soundSource.volume = value;
        PlayerPrefs.SetFloat(SoundVolumeKey, value);
        PlayerPrefs.Save();
    }
}
