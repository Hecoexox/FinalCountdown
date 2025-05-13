using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight; // Spotlight objesi
    public AudioSource flashlightAudioSource; // Ses için ayrı bir kaynak
    public AudioClip toggleSound; // Açma/kapama sesi

    private bool isOn = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.SetActive(isOn);
            flashlightAudioSource.PlayOneShot(toggleSound);
        }
    }
    public void ForceTurnOff()
    {
        isOn = false;
        flashlight.SetActive(false);
    }

}
    