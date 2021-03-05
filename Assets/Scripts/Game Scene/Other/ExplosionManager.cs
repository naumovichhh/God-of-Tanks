using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;
    private ObjectPooler pooler;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    public void Explode(Vector3 position)
    {
        GameObject pooledObject = pooler.GetPooledObject();
        pooledObject.transform.position = position;
        ParticleSystem particles = pooledObject.GetComponent<ParticleSystem>();
        pooledObject.SetActive(true);
        StartCoroutine(DeactivateCoroutine(pooledObject));
    }

    private IEnumerator DeactivateCoroutine(GameObject pooledObject)
    {
        yield return new WaitForSeconds(5);
        pooledObject.SetActive(false);
    }
}
