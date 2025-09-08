using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>, IManager
{
    public Pool arrowPool;
    public Pool earthquakeArrowPool;
    public Pool fogArrowPool;
    public Pool particlePool;
    public Pool rockPool;

    public Pool damagePool;
    public void Init()
    {
        transform.GetChild(0).TryGetComponent<Pool>(out arrowPool);
        transform.GetChild(1).TryGetComponent<Pool>(out earthquakeArrowPool);
        transform.GetChild(2).TryGetComponent<Pool>(out fogArrowPool);
        transform.GetChild(3).TryGetComponent<Pool>(out particlePool);
        transform.GetChild(4).TryGetComponent<Pool>(out rockPool);


        GameObject.Find("DamageCanvas").TryGetComponent<Pool>(out damagePool);
    }
}
