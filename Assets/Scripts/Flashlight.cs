using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light spotlight;          // Spot light objesini burada sürükle
    public AudioClip flashlightSound; // Ses dosyasını buraya sürükle
    private AudioSource audioSource;  // Ses kaynağı (AudioSource)

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource bileşenini al
    }

    void Update()
    {
        // Oyun donmasi durumu olabilir, bu yuzden timeScale'ı etkilemiyoruz.

        if (Input.GetKeyDown(KeyCode.F))  // F tusuna basildiginda
        {
            spotlight.enabled = !spotlight.enabled;  // Spot light'ı aç/kapa yap

            if (audioSource != null && flashlightSound != null) // Eğer ses kaynagi ve ses dosyasi varsa
            {
                audioSource.PlayOneShot(flashlightSound);  // Ses cal
            }
            else
            {
                Debug.LogWarning("AudioSource veya ses dosyası eksik!");
            }
        }
    }
}