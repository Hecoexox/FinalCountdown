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
            if (col.gameObject != triggerBox.gameObject && col.tag == "Item") // Assuming your spawned items are tagged as "Item"
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
            GameObject prefabToSpawn = spawnablePrefabs[Random.Range(0, spawnablePrefabs.Count)];
            Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        }
        isSpawning = false;
    }
}