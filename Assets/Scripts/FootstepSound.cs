using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.8f;

    private CharacterController controller;
    private float stepTimer = 0f;

    // Yürüme hızını referans alıyoruz
    private float walkSpeedReference = 8f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        footstepAudioSource.clip = footstepClip;
        footstepAudioSource.loop = true;
        footstepAudioSource.playOnAwake = false;
    }

    void Update()
    {
        float currentSpeed = controller.velocity.magnitude;
        bool isWalking = controller.isGrounded && currentSpeed > 0.1f;

        if (isWalking)
        {
            // Pitch'i hıza göre ayarlıyoruz
            float targetPitch = currentSpeed / walkSpeedReference;
            footstepAudioSource.pitch = Mathf.Clamp(targetPitch, 0.8f, 2f); // Aşırı abartmasın diye sınır koyduk

            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }
        }
        else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }
}
