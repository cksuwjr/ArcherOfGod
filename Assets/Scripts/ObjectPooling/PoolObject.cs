using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private Pool pool;

    public void Init(Pool pool)
    {
        this.pool = pool;
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        pool.ReturnPoolObject(this);
    }
}