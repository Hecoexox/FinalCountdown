using UnityEngine;
using System.Collections;

public class LeverInteraction : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip declineSound;
    public float interactionDistance = 3f;
    public Transform leverHandle; // Topuz kısmı
    public float tiltAngle = 30f; // Güncellendi
    public float tiltSpeed = 5f;

    private Transform player;
    private Quaternion originalRotation;
    private bool isTilting = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (leverHandle != null)
        {
            originalRotation = leverHandle.localRotation;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(declineSound);
            }

            if (leverHandle != null && !isTilting)
            {
                StartCoroutine(TiltLever());
            }
        }
    }

    IEnumerator TiltLever()
    {
        isTilting = true;

        Quaternion downRotation = originalRotation * Quaternion.Euler(tiltAngle, 0, 0);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * tiltSpeed;
            leverHandle.localRotation = Quaternion.Slerp(originalRotation, downRotation, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * tiltSpeed;
            leverHandle.localRotation = Quaternion.Slerp(downRotation, originalRotation, t);
            yield return null;
        }

        isTilting = false;
    }
}