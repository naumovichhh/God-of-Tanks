using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler ShellPooler;
    public static ObjectPooler HealthbarPooler;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    public string objectType;
    private List<GameObject> pooledObjects;

    private void Awake()
    {
        if (objectToPool.GetComponent<Shell>() != null)
            ShellPooler = this;
        if (objectToPool.GetComponent<HealthBar>() != null)
            HealthbarPooler = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        bool isShell = objectToPool.GetComponent<Shell>() != null;
        for (uint i = 0; i < amountToPool; ++i)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.transform.parent = transform;
            obj.SetActive(false);
            if (isShell)
            {
                foreach (var o in pooledObjects)
                {
                    Physics.IgnoreCollision(o.GetComponentInChildren<Collider>(), obj.GetComponentInChildren<Collider>());
                }
            }

            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
