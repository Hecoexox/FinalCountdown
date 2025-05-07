using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    public Transform flap1, flap2, flap3, flap4;
    public float rotationSpeed = 2f; // Flaplar�n d�nme h�z�n� ayarlamak i�in

    private bool isAnimating = false;
    public bool Animate = false;

    void Update()
    {
        if (Animate && !isAnimating)
        {
            StartCoroutine(AnimateFlaps());
        }
    }

    private System.Collections.IEnumerator AnimateFlaps()
    {
        isAnimating = true;

        // �lk iki flap'�n kapanmas� (d��ar�ya do�ru a��lacak �ekilde)
        StartCoroutine(RotateFlap(flap1, new Vector3(0, 0, -180))); // flap1 sola do�ru     

        StartCoroutine(RotateFlap(flap2, new Vector3(0, 0, 180))); // flap2 sola do�ru
        yield return new WaitForSeconds(0.5f); // Biraz bekle

        // Son iki flap'�n kapanmas� (d��ar�ya do�ru a��lacak �ekilde)
        StartCoroutine(RotateFlap(flap3, new Vector3(0, 90, 180))); // flap3 sola do�ru
        
        StartCoroutine(RotateFlap(flap4, new Vector3(0, 90, -180))); // flap4 sola do�ru

        yield return new WaitForSeconds(1f); // Animasyon bitene kadar bekle

        isAnimating = false;
    }

    // Flap d�nd�rme fonksiyonu
    private System.Collections.IEnumerator RotateFlap(Transform flap, Vector3 targetRotation)
    {
        Quaternion startRotation = flap.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);

        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            flap.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        flap.rotation = endRotation; // Kesin d�n�� sa�la
    }
}
