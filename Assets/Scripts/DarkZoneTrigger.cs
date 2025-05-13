using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkZoneTrigger : MonoBehaviour
{
    public Transform spawnPoint;
    public Image blackScreen;
    public float fadeDuration = 1.5f;
    public GameObject player;
    public FlashlightController flashlightController;


    public AudioSource alarmSound;
    public AudioClip alarmClip;

    private CharacterController controller;
    private bool isProcessing = false; // Yeni: işlemde mi kontrolü

    private void Start()
    {
        controller = player.GetComponent<CharacterController>();
        if (blackScreen != null)
        {
            Color c = blackScreen.color;
            c.a = 0f;
            blackScreen.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessing)
        {
            StartCoroutine(HandleDarkZone());
        }
    }

    IEnumerator HandleDarkZone()
    {
        isProcessing = true; // Artık yeniden tetiklenemez

        // Hareketi kapat
        controller.enabled = false;

        // Yakalanma sesi
        if (alarmSound != null && alarmClip != null)
        {
            alarmSound.PlayOneShot(alarmClip);
        }
        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // 3 saniye bekle
        yield return new WaitForSeconds(3f);

        // Spawn'a ışınla
        player.transform.position = spawnPoint.position;

        // Hareketi geri aç
        controller.enabled = true;

        // Flashlight'ı da tekrar aç
        if (flashlightController != null)
        {
            flashlightController.enabled = true;
        }

        // Fade from black
        yield return StartCoroutine(FadeFromBlack());

        // Her şey tamam
        isProcessing = false;
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color c = blackScreen.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            blackScreen.color = c;
            yield return null;
        }
        c.a = 1f;
        blackScreen.color = c;
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color c = blackScreen.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            blackScreen.color = c;
            yield return null;
        }
        c.a = 0f;
        blackScreen.color = c;
    }
}
