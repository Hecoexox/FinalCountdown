using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallet : MonoBehaviour, IInteractable
{
    public int maxCapacity = 6;
    public Transform[] itemSlots;
    private GameObject[] storedItems; // Slot bazlý

    private string storedItemName = null;

    private void Awake()
    {
        storedItems = new GameObject[maxCapacity];
    }

    public void Interact(PlayerInteraction player)
    {
        if (player.heldItem == null) return;

        string itemName = player.heldItem.GetComponent<Item>().itemName;

        // Ýlk kez item ekleniyorsa tipi belirle
        if (storedItemName == null)
        {
            storedItemName = itemName;
        }

        // Uyuþmayan item ise çýk
        if (storedItemName != itemName)
        {
            Debug.Log("Bu palete bu item konamaz.");
            return;
        }

        // Boþ slot bul
        int emptyIndex = -1;
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }

        if (emptyIndex == -1)
        {
            Debug.Log("Palet dolu.");
            return;
        }

        // Yerleþtir
        GameObject item = player.heldItem;
        storedItems[emptyIndex] = item;

        item.transform.SetParent(transform);
        item.transform.position = itemSlots[emptyIndex].position;
        item.transform.rotation = Quaternion.identity;

        item.GetComponent<Item>().SetPallet(this);

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        player.heldItem = null;
    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < storedItems.Length; i++)
        {
            if (storedItems[i] == item)
            {
                storedItems[i] = null;
                break;
            }
        }

        // Eðer palet tamamen boþsa türü sýfýrla
        bool isEmpty = true;
        foreach (var stored in storedItems)
        {
            if (stored != null)
            {
                isEmpty = false;
                break;
            }
        }
        if (isEmpty)
        {
            storedItemName = null;
        }
    }

    public string GetStoredItemName() => storedItemName;
}
