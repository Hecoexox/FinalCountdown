using UnityEngine;
using System.Collections;

public class LeverInteraction : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip declineSound;
    public float interactionDistance = 3f;
    public Transform leverHandle;
    public float tiltAngle = 30f;
    public float tiltSpeed = 5f;

    private Transform player;
    private Quaternion originalRotation;
    private Coroutine tiltCoroutine;

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
            // Ses her basışta çalsın
            audioSource.PlayOneShot(declineSound);

            // Tilt coroutine çalışıyorsa durdur, yeniden başlat
            if (leverHandle != null)
            {
                if (tiltCoroutine != null)
                    StopCoroutine(tiltCoroutine);

                tiltCoroutine = StartCoroutine(TiltLever());
            }
        }
    }

    IEnumerator TiltLever()
    {
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
    }
}