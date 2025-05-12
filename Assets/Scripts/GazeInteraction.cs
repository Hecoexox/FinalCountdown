using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeClickPlay : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource'u public yapıyoruz ki drag & drop ile atayalım
    public float interactionDistance = 3f; // Obje ile etkileşim mesafesi
    private bool isLooking = false;
    public float stopDistance = 5f; // Sesin duracağı mesafe (5 metre)

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Eğer manual atama yapılmadıysa, script objesinden AudioSource al
        }
    }

    void Update()
    {
        // Kameranın baktığı yönü ve mesafeyi raycast ile kontrol ediyoruz
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance)) // 3 metreye kadar bak
        {
            if (hit.collider.gameObject == this.gameObject) // Eğer bakılan obje bu radyo ise
            {
                if (!isLooking) // İlk bakmaya başladığında
                {
                    isLooking = true;
                    PlaySound(); // Ses çalmaya başla
                }
            }
            else
            {
                isLooking = false; // Radyo objesinden başka bir objeye bakıldığında
            }
        }
        else
        {
            isLooking = false; // Raycast objeyi görmezse
        }

        // Mesafe kontrolü ile uzaklaşınca sesi durdur
        if (audioSource.isPlaying && Vector3.Distance(Camera.main.transform.position, transform.position) > stopDistance)
        {
            StopSound();
        }
    }

    // Bakıldığında sesi çalan fonksiyon
    void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying) // Eğer ses zaten çalmıyorsa
        {
            audioSource.Play(); // Ses çal
        }
    }

    // Uzaklaşınca sesi durduran fonksiyon
    void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Ses durdurulacak
        }
    }
}
