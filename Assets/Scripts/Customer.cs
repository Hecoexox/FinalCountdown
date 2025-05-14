using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public GameObject[] items;
    public TextMeshPro orderText; // Sahnedeki Text nesnesi
    public static List<string> orderList = new List<string>();
    public static Customer Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateNewOrder();
    }

    public void CreateNewOrder()
    {
        orderList.Clear();

        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, items.Length);
            string itemName = items[randomIndex].name;

            orderList.Add(itemName);

            if (itemCounts.ContainsKey(itemName))
                itemCounts[itemName]++;
            else
                itemCounts[itemName] = 1;
        }

        // Text'e yaz
        string displayText = "Hi, Can I Get\n";
        foreach (var pair in itemCounts)
        {
            displayText += $"{pair.Value}x {pair.Key}\n";
        }

        if (orderText != null)
            orderText.text = displayText;
    }
}
