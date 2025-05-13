using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioGazeSound : MonoBehaviour
{
    public AudioSource radioAudioSource;
    public float maxVolume = 1f;
    public float fadeSpeed = 1f;
    public float gazeDistance = 10f;

    private Camera mainCam;
    private bool isLooking = false;

    void Start()
    {
        mainCam = Camera.main;
        radioAudioSource.volume = 0f;
        radioAudioSource.loop = true;
        radioAudioSource.playOnAwake = false;
    }

    void Update()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, gazeDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isLooking = true;
                if (!radioAudioSource.isPlaying)
                {
                    radioAudioSource.Play();
                }
            }
            else
            {
                isLooking = false;
            }
        }
        else
        {
            isLooking = false;
        }

        // Sesin yumuşak geçişi
        if (isLooking)
        {
            radioAudioSource.volume = Mathf.MoveTowards(radioAudioSource.volume, maxVolume, fadeSpeed * Time.deltaTime);
        }
        else
        {
            radioAudioSource.volume = Mathf.MoveTowards(radioAudioSource.volume, 0f, fadeSpeed * Time.deltaTime);
            if (radioAudioSource.volume == 0f && radioAudioSource.isPlaying)
            {
                radioAudioSource.Stop();
            }
        }
    }
}
