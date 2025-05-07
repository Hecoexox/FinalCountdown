using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    public Transform flap1, flap2, flap3, flap4;
    public float rotationSpeed = 2f; // Flaplarýn dönme hýzýný ayarlamak için

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

        // Ýlk iki flap'ýn kapanmasý (dýþarýya doðru açýlacak þekilde)
        StartCoroutine(RotateFlap(flap1, new Vector3(0, 0, -180))); // flap1 sola doðru     

        StartCoroutine(RotateFlap(flap2, new Vector3(0, 0, 180))); // flap2 sola doðru
        yield return new WaitForSeconds(0.5f); // Biraz bekle

        // Son iki flap'ýn kapanmasý (dýþarýya doðru açýlacak þekilde)
        StartCoroutine(RotateFlap(flap3, new Vector3(0, 90, 180))); // flap3 sola doðru
        
        StartCoroutine(RotateFlap(flap4, new Vector3(0, 90, -180))); // flap4 sola doðru

        yield return new WaitForSeconds(1f); // Animasyon bitene kadar bekle

        isAnimating = false;
    }

    // Flap döndürme fonksiyonu
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

        flap.rotation = endRotation; // Kesin dönüþ saðla
    }
}
