using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardClickSimple : MonoBehaviour
{
    public AudioSource audioSource;
    public float activationDistance = 2f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= activationDistance && Input.GetKeyDown(KeyCode.E))
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
