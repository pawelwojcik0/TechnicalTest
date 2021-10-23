using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ObjectPool
{
    public Ball prefab;
    public int poolInitCount;
    public Transform poolTransform;

    private List<Ball> pooledObjects = new List<Ball>();

    public void Initialize()
    {
        for (int i = 0; i < poolInitCount; i++)
        {
            Ball obj = GameObject.Instantiate(prefab, poolTransform);
            obj.gameObject.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public void Initialize(Ball _prefab, int _InitCount)
    {
        prefab = _prefab;
        poolInitCount = _InitCount;
        Initialize();
    }

    public Ball GetObject()
    {
        Ball ToReturn = pooledObjects.LastOrDefault();
        if (ToReturn != null)
        {
            pooledObjects.Remove(ToReturn);
            return ToReturn;
        }
        else
        {
            ToReturn = GameObject.Instantiate(prefab, poolTransform);
            return ToReturn;
        }
    }

    public void ReturnToPool(Ball obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolTransform);
    }
}

