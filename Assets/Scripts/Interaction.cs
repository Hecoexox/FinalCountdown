using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float interactDistance = 5f;
    public Transform itemHoldPosition;
    public bool elindeItemVar = false;
    public float moveSpeed = 15f;

    private GameObject heldItem;
    private Collider heldItemCollider;
    private Coroutine moveCoroutine;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (elindeItemVar)
                {
                    if (clickedObject.CompareTag("ItemDrop"))
                    {
                        BýrakItem(clickedObject.transform.position);
                        Debug.Log("Item býrakýldý: " + heldItem.name + " -> " + clickedObject.name);
                    }
                    else
                    {
                        Debug.Log("Elde item var ama buraya býrakýlamaz.");
                    }
                }
                else
                {
                    if (clickedObject.CompareTag("Item"))
                    {
                        heldItem = clickedObject;

                        heldItemCollider = heldItem.GetComponent<Collider>();
                        if (heldItemCollider != null)
                            heldItemCollider.enabled = false;

                        // Hareket coroutine baþlat
                        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                        moveCoroutine = StartCoroutine(MoveItemTo(heldItem, itemHoldPosition.position, itemHoldPosition.rotation, () =>
                        {
                            heldItem.transform.SetParent(itemHoldPosition);
                            elindeItemVar = true;
                            Debug.Log("Item alýndý: " + heldItem.name);
                        }));
                    }
                    else
                    {
                        Debug.Log("Bu obje bir item deðil.");
                    }
                }
            }
        }
    }

    void BýrakItem(Vector3 dropPosition)
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);

            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveItemTo(heldItem, dropPosition, Quaternion.identity, () =>
            {
                if (heldItemCollider != null)
                    heldItemCollider.enabled = true;

                heldItem = null;
                heldItemCollider = null;
                elindeItemVar = false;
            }));
        }
    }

    IEnumerator MoveItemTo(GameObject item, Vector3 targetPosition, Quaternion targetRotation, System.Action onComplete)
    {
        float t = 0f;
        Vector3 startPos = item.transform.position;
        Quaternion startRot = item.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            item.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            item.transform.rotation = Quaternion.Lerp(startRot, targetRotation, t);
            yield return null;
        }

        item.transform.position = targetPosition;
        item.transform.rotation = targetRotation;

        onComplete?.Invoke();
    }
}