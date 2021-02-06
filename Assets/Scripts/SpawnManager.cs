using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float xRange = 13;
    private float zRange = 6;
    // Spawn manager should contain four poolers:
    // for enemies, strong enemies, shells, medicines,
    // which are stored in dictionary
    private Dictionary<string, ObjectPooler> poolers = new Dictionary<string, ObjectPooler>();

    private void Start()
    {
        // Every object pooler script, attached to Spawn Manager gameobject
        // is put to dictionary in order to be accessed by key
        foreach (var pooler in GetComponents<ObjectPooler>())
        {
            poolers.Add(pooler.objectType, pooler);
        }

        StartCoroutine(SpawnMedicines());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnStrongEnemies());
    }

    private IEnumerator SpawnMedicines()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(23, 30));
            Spawn("Medicine");
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 15));
            Spawn("Enemy");
        }
    }

    private IEnumerator SpawnStrongEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 45));
            Spawn("Strong Enemy");
        }
    }

    private void Spawn(string poolerKey)
    {
        var pooledObject = poolers[poolerKey].GetPooledObject();
        do
        {
            float xPosition = Random.Range(-xRange, xRange);
            float zPosition = Random.Range(-zRange, zRange);
            Vector3 position = new Vector3(xPosition, pooledObject.transform.position.y, zPosition);
            pooledObject.transform.position = position;
        } while (pooledObject.GetComponent<ObjectToSpawn>().IsOverlapped());
        // All the objects are pooled, so they should just
        // be set active
        pooledObject.SetActive(true);
    }
}
