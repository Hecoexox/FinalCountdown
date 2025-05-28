using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : MonoBehaviour
{
    public GameObject Correct;
    public GameObject Wrong;

    public Material Red;
    public Material Green;
    public Material White;

    public AudioSource successAudio;
    public AudioSource failAudio;

    private List<string> receivedItems = new List<string>();

    void Start()
    {
        SetMaterial(Correct, White);
        SetMaterial(Wrong, White);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                receivedItems.Add(item.itemName);
                Debug.Log("Alınan item: " + item.itemName);

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
            Debug.Log("Sipariş doğru! Yeni sipariş oluşturuluyor...");
            StartCoroutine(ShowFeedback(Correct, Green));
            Customer.Instance.CreateNewOrder();
            if (DaySystem.Instance != null)
                DaySystem.Instance.OnCorrectOrder();
        }
        else
        {
            Debug.Log("Yanlış sipariş!");
            StartCoroutine(ShowFeedback(Wrong, Red));
            if (DaySystem.Instance != null)
                DaySystem.Instance.OnWrongOrder();
        }

        receivedItems.Clear();
    }

    private void SetMaterial(GameObject obj, Material mat)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = mat;
        }
    }

    private IEnumerator ShowFeedback(GameObject obj, Material mat)
    {
        SetMaterial(obj, mat);

        // Ses efektini çal
        if (mat == Green && successAudio != null)
            successAudio.Play();
        else if (mat == Red && failAudio != null)
            failAudio.Play();

        yield return new WaitForSeconds(2f);
        SetMaterial(obj, White);
    }
}
