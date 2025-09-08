using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolObject poolObject;
    [SerializeField] private int poolCount = 10;

    private Queue<PoolObject> poolObjects = new Queue<PoolObject>();

    public void Init()
    {
        Allocate();
    }

    private void Allocate()
    {
        for (int i = 0; i < poolCount; i++)
        {
            var obj = Instantiate(poolObject, transform);
            obj.gameObject.SetActive(false);
            poolObjects.Enqueue(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        if (poolObjects.Count < 1)
            Allocate();
        var obj = poolObjects.Dequeue();
        obj.Init(this);

        return obj.gameObject;
    }

    public void ReturnPoolObject(PoolObject returnObject)
    {
        returnObject.gameObject.SetActive(false);
        returnObject.transform.SetParent(this.transform);
        poolObjects.Enqueue(returnObject);
    }

}