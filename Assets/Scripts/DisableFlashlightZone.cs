using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlashlightZone : MonoBehaviour
{
    public FlashlightController flashlightController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && flashlightController != null)
        {
            // Feneri kapat ve kontrolü devre dışı bırak
            flashlightController.ForceTurnOff(); // Yeni metod ekleyeceğiz!
            flashlightController.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && flashlightController != null)
        {
            flashlightController.enabled = true;  // Bu kısmı ekledik.
        }
    }

}
