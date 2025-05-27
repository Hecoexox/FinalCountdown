using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Transform spawnPosition;
    public Collider triggerBox;
    public List<GameObject> spawnablePrefabs;

    private bool isSpawning = false;

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

        if (spawnablePrefabs.Count > 0)
        {
            GameObject prefabToSpawn = GetWeightedRandomPrefab();
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }

        isSpawning = false;
    }

    GameObject GetWeightedRandomPrefab()
    {
        // Her prefab i�in sahnedeki mevcut say�y� say
        List<int> counts = new List<int>();
        for (int i = 0; i < spawnablePrefabs.Count; i++)
        {
            int count = CountItemsInScene(spawnablePrefabs[i].name);
            counts.Add(count);
        }

        // A��rl�klar� ters orant�l� yap: ne kadar az, o kadar b�y�k a��rl�k
        // weight = 1 / (count + 1) ile s�f�ra b�l�nme �nlenir
        List<float> weights = new List<float>();
        float totalWeight = 0f;
        for (int i = 0; i < counts.Count; i++)
        {
            float weight = 1f / (counts[i] + 1);
            weights.Add(weight);
            totalWeight += weight;
        }

        // A��rl�kl� rastgele se�im
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

        // Normal �artlarda buraya gelmemeli ama sorun olursa ilk prefabi d�nd�r
        return spawnablePrefabs[0];
    }

    int CountItemsInScene(string prefabName)
    {
        // Sahnedeki "Item" tag'li objeler aras�nda ismi prefabName ile ayn� olanlar� say
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        int count = 0;
        foreach (var item in items)
        {
            if (item.name.Contains(prefabName)) // prefab ad� ile obje ad�n� kar��la�t�r�yoruz
            {
                count++;
            }
        }
        return count;
    }
}
