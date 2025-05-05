using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light spotlight;          // Spot light objesini burada sürükle
    public AudioClip flashlightSound; // Ses dosyasını buraya sürükle
    private AudioSource audioSource;  // Ses kaynağı (AudioSource)

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource bileşenini al

        // Eğer audioSource veya flashlightSound eksikse hata mesajı ver
        if (audioSource == null)
        {
            Debug.LogError("AudioSource bileşeni eksik! Lütfen AudioSource bileşenini Spotlight objenize ekleyin.");
        }
        if (flashlightSound == null)
        {
            Debug.LogError("flashlightSound (ses dosyası) eksik! Lütfen bir ses dosyası bağlayın.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))  // F tuşuna basıldığında
        {
            spotlight.enabled = !spotlight.enabled;  // Spot light'ı aç/kapa yap

            if (audioSource != null && flashlightSound != null) // Eğer ses kaynağı ve ses dosyası varsa
            {
                if (audioSource.isPlaying) // Eğer ses zaten çalıyorsa, durdur ve yeniden başlat
                {
                    audioSource.Stop(); // Önce sesi durdur
                }
                audioSource.PlayOneShot(flashlightSound);  // Ses çal
            }
        }
    }
}