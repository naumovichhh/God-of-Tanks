using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private float insideXMax = 16, insideZMax = 8;
    private float outsideXMax = 40, outsideZMax = 20;
    private float outsideXMin = 20, outsideZMin = 10;
    // Spawn manager should contain four poolers:
    // for enemies, strong enemies, shells, medicines,
    // which are stored in dictionary
    private Dictionary<string, ObjectPooler> poolers = new Dictionary<string, ObjectPooler>();
    private float enemySpawnFrequencyCoefficient = 1;

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
        StartCoroutine(ManageWaves());
    }

    private void Update()
    {
        enemySpawnFrequencyCoefficient /= (float)Math.Pow(1.007, Time.deltaTime);
    }

    private IEnumerator ManageWaves()
    {
        float waveTimer = 60;
        while (true)
        {
            yield return new WaitForSeconds(waveTimer);
            waveTimer *= 1.33f;
            enemySpawnFrequencyCoefficient = 1;
        }
    }

    private IEnumerator SpawnMedicines()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(23, 30));
            Spawn("Medicine", true);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 15) * enemySpawnFrequencyCoefficient);
            Spawn("Enemy", false);
        }
    }

    private IEnumerator SpawnStrongEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(30, 45) * enemySpawnFrequencyCoefficient);
            Spawn("Strong Enemy", false);
        }
    }

    private void Spawn(string poolerKey, bool insideVisibleField)
    {
        var pooledObject = poolers[poolerKey].GetPooledObject();
        do
        {
            float xPosition, zPosition;
            if (insideVisibleField)
            {
                (xPosition, zPosition) = GetPositionInsideVisible();
            }
            else
            {
                (xPosition, zPosition) = GetPositionOutsideVisible();
            }

            Vector3 position = new Vector3(xPosition, pooledObject.transform.position.y, zPosition);
            pooledObject.transform.position = position;
        } while (pooledObject.GetComponent<IObjectToSpawn>().IsOverlapped());
        // All the objects are pooled, so they should just
        // be set active
        pooledObject.SetActive(true);
    }

    private (float, float) GetPositionInsideVisible()
    {
        return (Random.Range(-insideXMax, insideXMax), Random.Range(-insideZMax, insideZMax));
    }

    private (float, float) GetPositionOutsideVisible()
    {
        float xPosition, zPosition;
        xPosition = Random.Range(-outsideXMax, outsideXMax);
        zPosition = Random.Range(-outsideZMax, outsideZMax);
        while (Math.Abs(xPosition) < outsideXMin && Math.Abs(zPosition) < outsideZMin)
        {
            xPosition *= 1.5f;
            zPosition *= 1.5f;
        }

        return (xPosition, zPosition);
    }
}
