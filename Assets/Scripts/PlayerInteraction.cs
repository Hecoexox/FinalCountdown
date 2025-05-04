using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 5f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI itemNameText;
    public Transform handPosition;
    public Camera playerCamera;

    [HideInInspector]
    public GameObject heldItem = null;

    void Update()
    {
        HandleInteraction();
        ShowLookedAtItemName();
    }

    void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // Elimizde item varsa � sadece palet ya da output gibi yerlere izin ver
                if (heldItem != null)
                {
                    if (interactable is Pallet)
                    {
                        interactable.Interact(this);
                    }
                }
                else
                {
                    // Elimiz bo�sa her �eye t�klayabilirsin (item, bilgisayar, vs)
                    if (interactable != null)
                    {
                        interactable.Interact(this);
                    }
                }
            }
        }
    }

    void ShowLookedAtItemName()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                itemNameText.text = item.itemName ;
                return;
            }
        }

        // Hi�bir �ey bak�lm�yorsa bo�alt
        itemNameText.text = "";
    }

    public void PickUpItem(GameObject item)
    {
        heldItem = item;

        // Collider ve Rigidbody ayarlar�
        Collider itemCollider = item.GetComponent<Collider>();
        if (itemCollider != null)
            itemCollider.enabled = false;

        // El pozisyonuna ta��
        item.transform.SetParent(handPosition);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public void DropHeldItem()
    {
        if (heldItem == null) return;

        // Collider ve Rigidbody geri a�
        Collider itemCollider = heldItem.GetComponent<Collider>();
        if (itemCollider != null)
            itemCollider.enabled = true;

        // Serbest b�rak
        heldItem.transform.SetParent(null);
        heldItem = null;
    }
}
