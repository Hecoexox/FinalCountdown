using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallet : MonoBehaviour
{
    public ItemData palletItemType;
    public int currentCount = 0;
    public int maxStack = 4; // örnek

    public bool TryAddItem(ItemData newItem)
    {
        if (palletItemType == null)
        {
            palletItemType = newItem;
            currentCount = 1;
            Debug.Log("Yeni stack baþlatýldý.");
            return true;
        }
        else if (palletItemType == newItem && currentCount < maxStack)
        {
            currentCount++;
            Debug.Log("Stacke eklendi. Mevcut sayi: " + currentCount);
            return true;
        }

        Debug.Log("Bu item buraya eklenemez.");
        return false;
    }
}