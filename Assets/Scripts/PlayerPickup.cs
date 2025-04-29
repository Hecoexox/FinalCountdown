using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;
    public float pickupDistance = 3f;
    private GameObject heldItem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldItem == null)
            {
                TryPickup();
            }
            else
            {
                TryPlace();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            ItemInstance item = hit.collider.GetComponent<ItemInstance>();
            if (item != null)
            {
                heldItem = item.gameObject;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.transform.SetParent(holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void TryPlace()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            // Palet mi?
            Pallet pallet = hit.collider.GetComponent<Pallet>();
            if (pallet != null)
            {
                ItemInstance instance = heldItem.GetComponent<ItemInstance>();
                if (pallet.TryAddItem(instance.itemData))
                {
                    Destroy(heldItem); // fiziksel item yok oldu, palete stacklendi
                    heldItem = null;
                    return;
                }
            }

            // Eðer baþka bir yer (örn. sipariþ alaný) vs varsa ekle
        }

        // Eðer geçerli yere býrakýlmýyorsa elden býrakma
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.transform.SetParent(null);
        heldItem = null;
    }
}