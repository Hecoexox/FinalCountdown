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
    private CharacterController controller;
    public FlashlightController flashlightController; // FlashlightController ekliyoruz

    private void Start()
    {
        controller = player.GetComponent<CharacterController>();
        // Oyuna başlarken ekran siyah olmasın
        if (blackScreen != null)
        {
            Color c = blackScreen.color;
            c.a = 0f;
            blackScreen.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {
        // ŞAK DİYE KAPANSIN
        Color c = blackScreen.color;
        c.a = 1f;
        blackScreen.color = c;

        yield return new WaitForSeconds(0.1f); // çok kısa bekle

        // Disable movement - Bu, oyuncu hareket edemesin diye.
        controller.enabled = false;

        // Spawn point'e git
        player.transform.position = spawnPoint.position;

        // Fade ile tekrar görünür olsun
        yield return StartCoroutine(FadeFromBlack());

        // Enable movement - Fade bitince tekrar hareket etsin
        controller.enabled = true;

        // Flashlight'ı tekrar kontrol edilebilir hale getir
        flashlightController.enabled = true; // Flashlight kontrolünü tekrar aktif ettik
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

        // Alpha'yı kesin sıfıra sabitle
        c.a = 0f;
        blackScreen.color = c;
    }
}
