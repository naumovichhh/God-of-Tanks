using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private const float insideXMax = 16, insideZMax = 8;
    private const float outsideXMax = 36, outsideZMax = 18;
    private const float outsideXMin = 20, outsideZMin = 10;
    [SerializeField] private WaveInfo waveInfo;
    // Spawn manager should contain four poolers:
    // for enemies, strong enemies, shells, medicines,
    // which are stored in dictionary
    private Dictionary<string, ObjectPooler> poolers = new Dictionary<string, ObjectPooler>();
    private float enemySpawnFrequencyCoefficient = 1;
    private bool enemySpawnPause;
    private int waveNumber = 1;
    private const float pauseDuration = 33;
    private float cleverPossibilityCoeff = 1;
    private float usualEnemyCleverPossibilityCoeff = 1;
    private float strongEnemyCleverPossibilityCoeff = 1;

    private void Start()
    {
        // Every object pooler script, attached to Spawn Manager gameobject
        // is put to dictionary in order to be accessed by key
        foreach (var pooler in GetComponents<ObjectPooler>())
        {
            poolers.Add(pooler.objectType, pooler);
        }

        StartCoroutine(SpawnToolkitsCoroutine());
        StartCoroutine(SpawnEnemiesCoroutine());
        StartCoroutine(SpawnStrongEnemiesCoroutine());
        StartCoroutine(ManageWaves());
    }

    private void Update()
    {
        enemySpawnFrequencyCoefficient /= (float)Math.Pow(1.006, Time.deltaTime);
    }

    private IEnumerator ManageWaves()
    {
        float waveTimer = 60;
        while (true)
        {
            yield return new WaitForSeconds(waveTimer);
            waveTimer *= 1.25f;
            enemySpawnFrequencyCoefficient = 1;
            enemySpawnPause = true;
            StartCoroutine(BreakPause());
            StartCoroutine(CompletelyPassWave());
        }
    }

    private IEnumerator BreakPause()
    {
        yield return new WaitForSeconds(pauseDuration);
        enemySpawnPause = false;
    }

    private IEnumerator CompletelyPassWave()
    {
        yield return new WaitForSeconds(20);
        ++waveNumber;
        waveInfo.WaveStarted(waveNumber);
    }

    private IEnumerator SpawnToolkitsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(23, 30));
            SpawnToolkit();
        }
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 15) * enemySpawnFrequencyCoefficient);
            if (enemySpawnPause)
                yield return new WaitForSeconds(pauseDuration);

            bool cleverAiming = GetCleverAiming(false);
            SpawnEnemy("Enemy", cleverAiming);
        }
    }

    private IEnumerator SpawnStrongEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(18, 27) * enemySpawnFrequencyCoefficient);
            if (enemySpawnPause)
                yield return new WaitForSeconds(pauseDuration);
            
            bool cleverAiming = GetCleverAiming(true);
            SpawnEnemy("Strong Enemy", cleverAiming);
        }
    }

    private void SpawnEnemy(string poolerKey, bool cleverAiming)
    {
        var pooledObject = poolers[poolerKey].GetPooledObject();
        do
        {
            float xPosition, zPosition;
            (xPosition, zPosition) = GetPositionOutsideVisible();

            Vector3 position = new Vector3(xPosition, pooledObject.transform.position.y, zPosition);
            pooledObject.transform.position = position;
        } while (pooledObject.GetComponent<IObjectToSpawn>().IsOverlapped());

        pooledObject.GetComponent<Enemy>().cleverAiming = cleverAiming;
        // All the objects are pooled, so they should just
        // be set active
        pooledObject.SetActive(true);
    }

    private void SpawnToolkit()
    {
        string poolerKey = "Toolkit";
        var pooledObject = poolers[poolerKey].GetPooledObject();
        do
        {
            float xPosition, zPosition;
            (xPosition, zPosition) = GetPositionInsideVisible();

            Vector3 position = new Vector3(xPosition, pooledObject.transform.position.y, zPosition);
            pooledObject.transform.position = position;
        } while (pooledObject.GetComponent<IObjectToSpawn>().IsOverlapped());
        // All the objects are pooled, so they should just
        // be set active
        pooledObject.SetActive(true);
    }

    private bool GetCleverAiming(bool strongEnemy)
    {
        bool result;
        result = Random.Range(0f, 100f) * cleverPossibilityCoeff * GetSecondCoeff() > 50;

        if (result)
        {
            if (strongEnemy)
            {
                cleverPossibilityCoeff /= 1.44f;
                strongEnemyCleverPossibilityCoeff /= 1.37f;
            }
            else
            {
                cleverPossibilityCoeff /= 1.2f;
                usualEnemyCleverPossibilityCoeff /= 1.37f;
            }
        }
        else
        {
            if (strongEnemy)
            {
                cleverPossibilityCoeff *= 1.44f;
                strongEnemyCleverPossibilityCoeff *= 1.37f;
            }
            else
            {
                cleverPossibilityCoeff *= 1.2f;
                usualEnemyCleverPossibilityCoeff *= 1.37f;
            }
        }

        return result;

        float GetSecondCoeff()
        {
            return strongEnemy ?
            strongEnemyCleverPossibilityCoeff : 
            usualEnemyCleverPossibilityCoeff;
        }
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
