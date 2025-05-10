using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    public AudioSource ambienceAudioSource; // Ambiyans sesini çalacak AudioSource
    public AudioClip ambienceClip; // Ambiyans ses dosyasını buraya at
    public float volume = 0.3f; // Ses seviyesini kontrol et (isteğe bağlı)

    void Start()
    {
        // Ambiyans sesini ayarlıyoruz
        if (ambienceAudioSource == null)
        {
            ambienceAudioSource = GetComponent<AudioSource>(); // Eğer elle atamadıysan
        }
        
        if (ambienceClip != null)
        {
            ambienceAudioSource.clip = ambienceClip;
            ambienceAudioSource.volume = volume;
            ambienceAudioSource.loop = true;  // Sürekli çalacak
            ambienceAudioSource.Play(); // Sesin başlamasını sağla
        }
    }

    void Update()
    {
        // Ambiyans sesinin her zaman çaldığından emin olun
        if (!ambienceAudioSource.isPlaying)
        {
            ambienceAudioSource.Play();
        }
    }
}
