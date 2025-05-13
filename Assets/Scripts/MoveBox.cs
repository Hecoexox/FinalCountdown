using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetPosition2;
    public bool isMoving = true;
    public float speed = 5f;
    public bool readyToGo = false;

    void Start()
    {
        targetPosition = transform.position + Vector3.right * 7;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                targetPosition2 = transform.position + Vector3.back * 6;
                gameObject.layer = LayerMask.NameToLayer("Water");
            }
        }
        else if (readyToGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition2, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition2) < 0.01f)
            {
                readyToGo = false;
                gameObject.layer = LayerMask.NameToLayer("Water");
                Destroy(gameObject);
            }
        }
    }
}