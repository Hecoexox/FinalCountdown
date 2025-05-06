using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    private Vector3 targetPosition;
    private bool isMoving = true;
    public float speed = 5f;

    void Start()
    {
        // Hedef pozisyonu belirle
        targetPosition = transform.position + Vector3.left * 3;

        // Ba�lang��ta layer'� Default yap
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void Update()
    {
        if (isMoving)
        {
            // Hareket et
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Layer'� Default olarak ayarla
            gameObject.layer = LayerMask.NameToLayer("Default");

            // Hedefe ula�t�ysa hareketi durdur
            if (transform.position == targetPosition)
            {
                isMoving = false;

                // Layer'� Water olarak de�i�tir
                gameObject.layer = LayerMask.NameToLayer("Water");
            }
        }
    }
}
