using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleTrigger : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] subtitles;             // Altyazılar dizisi
    public float[] subtitleDurations;     // Her altyazı için süre (saniye cinsinden)

    public TMP_Text subtitleUI;           // UI'daki TMP Text objesi
    public AudioSource voiceAudio;        // Ses çalacak kaynak
    public AudioClip[] voiceClips;        // Altyazılara eş sesler

    private bool triggered = false;       // Sadece bir kez çalışması için

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(PlaySubtitles());
        }
    }

    IEnumerator PlaySubtitles()
    {
        subtitleUI.gameObject.SetActive(true);

        for (int i = 0; i < subtitles.Length; i++)
        {
            // Ses varsa çal
            if (voiceClips.Length > i && voiceClips[i] != null)
            {
                voiceAudio.clip = voiceClips[i];
                voiceAudio.Play();
            }

            subtitleUI.text = subtitles[i];

            yield return new WaitForSeconds(subtitleDurations[i]);
        }

        subtitleUI.gameObject.SetActive(false);
    }
}