using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    private Pallet currentPallet;

    public void SetPallet(Pallet pallet)
    {
        currentPallet = pallet;
    }

    public void Interact(PlayerInteraction player)
    {
        if (player.heldItem == null)
        {
            player.PickUpItem(gameObject);
            if (currentPallet != null)
            {
                currentPallet.RemoveItem(gameObject);
                currentPallet = null;
            }
        }
    }
}