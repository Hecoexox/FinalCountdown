using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMenu : MonoBehaviour
{
    public Transform pointA;  // A pozisyonu
    public Transform pointB;  // B pozisyonu
    public float moveSpeed = 1f;  // Hareketin h�z�
    public float waitTimeAtB = 2f;  // B'de bekleme s�resi

    private void Start()
    {
        // Ba�lang��ta A'ya yerle�mesini sa�la
        transform.position = pointA.position;

        // Hareketi ba�lat
        StartCoroutine(MoveBackAndForth());
    }

    IEnumerator MoveBackAndForth()
    {
        while (true)
        {
            // A'dan B'ye hareket et
            yield return StartCoroutine(MoveToPosition(pointB.position));

            // B'de bekle
            yield return new WaitForSeconds(waitTimeAtB);

            // B'den A'ya hareket et
            yield return StartCoroutine(MoveToPosition(pointA.position));

            // A'da bekle (iste�e ba�l�)
            yield return new WaitForSeconds(waitTimeAtB);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        // Hedef pozisyona yava��a hareket et
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        // Tam olarak hedef pozisyona git
        transform.position = targetPosition;
    }
}