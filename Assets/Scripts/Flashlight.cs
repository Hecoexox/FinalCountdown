using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight; // Unity'de bu alana fener objeni s�r�kle

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeSelf); // Aktiflik durumunu tersine �evir
        }
    }
}