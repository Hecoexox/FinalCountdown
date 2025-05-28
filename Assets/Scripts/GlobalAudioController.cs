using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalAudioController : MonoBehaviour
{
    public List<AudioSource> musicSources; // Inspector’dan ekle veya otomatik bul
    public Slider musicSlider;
    private const string MusicVolumeKey = "MusicVolume";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        musicSlider.value = savedVolume;
        SetAllMusicVolumes(savedVolume);
        musicSlider.onValueChanged.AddListener(SetAllMusicVolumes);
    }

    public void SetAllMusicVolumes(float value)
    {
        foreach (var src in musicSources)
        {
            if (src != null)
                src.volume = value;
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }
}