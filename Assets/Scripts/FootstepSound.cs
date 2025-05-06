using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepSound : MonoBehaviour
{
    public AudioClip footstepClip;
    public AudioClip jumpClip;
    public float velocityThreshold = 0.1f;
    public float sprintSpeed = 13f;
    public float walkSpeed = 8f;

    private AudioSource footstepAudioSource; // Yürüyüş için
    private AudioSource sfxAudioSource;      // Jump gibi tek seferlik efektler için
    private CharacterController controller;
    private bool wasMovingLastFrame = false;
    private float originalPitch;

    void Start()
    {
        // İki ses kaynağı (footstep ve jump) ekleyelim
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            sfxAudioSource = sources[0];
        }

        footstepAudioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

        footstepAudioSource.clip = footstepClip;
        footstepAudioSource.loop = false;
        footstepAudioSource.playOnAwake = false;

        sfxAudioSource.playOnAwake = false;

        originalPitch = footstepAudioSource.pitch;
    }

    void Update()
    {
        

        // Hareket kontrolü
        bool isMoving = controller.velocity.magnitude > velocityThreshold && controller.isGrounded;

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                footstepAudioSource.pitch = originalPitch * (sprintSpeed / walkSpeed);
            else
                footstepAudioSource.pitch = originalPitch;

            if (!wasMovingLastFrame)
            {
                footstepAudioSource.Stop();
                footstepAudioSource.Play();
            }
        }
        else
        {
            if (footstepAudioSource.isPlaying)
                footstepAudioSource.Stop();
        }

        wasMovingLastFrame = isMoving;
    }
}
