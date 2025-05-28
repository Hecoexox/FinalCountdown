using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform spawnPosition;
    public Collider triggerBox;
    public List<GameObject> spawnablePrefabs;

    private bool isSpawning = false;
    private int tutorialSpawnIndex = 0;

    void Update()
    {
        if (!IsItemInsideTrigger())
        {
            if (!isSpawning)
            {
                StartCoroutine(SpawnItem());
            }
        }
    }

    bool IsItemInsideTrigger()
    {
        Collider[] colliders = Physics.OverlapBox(triggerBox.bounds.center, triggerBox.bounds.extents, triggerBox.transform.rotation);
        foreach (var col in colliders)
        {
            if (col.gameObject != triggerBox.gameObject && col.tag == "Item")
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnItem()
    {
        isSpawning = true;
        yield return new WaitForSeconds(1f); // optional delay

        GameObject prefabToSpawn = null;
        // Tutorial için ilk 3 spawn
        if (Customer.Instance != null && tutorialSpawnIndex < Customer.Instance.tutorialOrder.Length)
        {
            string tutName = Customer.Instance.tutorialOrder[tutorialSpawnIndex];
            foreach (var prefab in spawnablePrefabs)
            {
                if (prefab.name == tutName)
                {
                    prefabToSpawn = prefab;
                    break;
                }
            }
            tutorialSpawnIndex++;
        }
        else if (spawnablePrefabs.Count > 0)
        {
            prefabToSpawn = GetWeightedRandomPrefab();
        }

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }

        isSpawning = false;
    }

    GameObject GetWeightedRandomPrefab()
    {
        // Her prefab için sahnedeki mevcut sayıyı say
        List<int> counts = new List<int>();
        for (int i = 0; i < spawnablePrefabs.Count; i++)
        {
            int count = CountItemsInScene(spawnablePrefabs[i].name);
            counts.Add(count);
        }

        // Ağırlıkları ters orantılı yap: ne kadar az, o kadar büyük ağırlık
        // weight = 1 / (count + 1) ile sıfıra bölünme önlenir
        List<float> weights = new List<float>();
        float totalWeight = 0f;
        for (int i = 0; i < counts.Count; i++)
        {
            float weight = 1f / (counts[i] + 1);
            weights.Add(weight);
            totalWeight += weight;
        }

        // Ağırlıklı rastgele seçim
        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
            {
                return spawnablePrefabs[i];
            }
        }

        // Normal şartlarda buraya gelmemeli ama sorun olursa ilk prefabi döndür
        return spawnablePrefabs[0];
    }

    int CountItemsInScene(string prefabName)
    {
        // Sahnedeki "Item" tag'li objeler arasında ismi prefabName ile aynı olanları say
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        int count = 0;
        foreach (var item in items)
        {
            if (item.name.Contains(prefabName)) // prefab adı ile obje adını karşılaştırıyoruz
            {
                count++;
            }
        }
        return count;
    }
}
