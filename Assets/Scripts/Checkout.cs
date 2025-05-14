using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : MonoBehaviour
{
    private List<string> receivedItems = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                receivedItems.Add(item.itemName);
                Debug.Log("Al�nan item: " + item.itemName);

                if (receivedItems.Count == 3)
                {
                    CheckOrder();
                }
            }
        }
    }

    private void CheckOrder()
    {
        List<string> tempOrder = new List<string>(Customer.orderList);
        bool allMatch = true;

        foreach (string item in receivedItems)
        {
            if (tempOrder.Contains(item))
            {
                tempOrder.Remove(item);
            }
            else
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch && tempOrder.Count == 0)
        {
            Debug.Log(" Sipari� do�ru! Yeni sipari� olu�turuluyor...");
            Customer.Instance.CreateNewOrder(); // Yeni sipari�
        }
        else
        {
            Debug.Log(" Yanl�� sipari�!");
        }

        receivedItems.Clear(); // Listeyi s�f�rla
    }
}