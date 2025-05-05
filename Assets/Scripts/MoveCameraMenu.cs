using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMenu : MonoBehaviour
{
    public Transform pointA;  // A pozisyonu
    public Transform pointB;  // B pozisyonu
    public float moveSpeed = 1f;  // Hareketin hýzý
    public float waitTimeAtB = 2f;  // B'de bekleme süresi

    private void Start()
    {
        // Baþlangýçta A'ya yerleþmesini saðla
        transform.position = pointA.position;

        // Hareketi baþlat
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

            // A'da bekle (isteðe baðlý)
            yield return new WaitForSeconds(waitTimeAtB);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        // Hedef pozisyona yavaþça hareket et
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