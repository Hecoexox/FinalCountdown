using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        if (Camera.main != null)
            mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Kameraya bak
            transform.LookAt(mainCamera);

            // Y ekseninde 180 derece döndür (mirrored fix)
            transform.Rotate(0f, 180f, 0f);
        }
    }
}