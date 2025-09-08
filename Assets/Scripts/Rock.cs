using UnityEngine;

public class Rock : PoolObject
{
    private void OnEnable()
    {
        Invoke("Dissappear", 5);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Dissappear()
    {
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.TryGetComponent<PlayerController>(out var pc))
                pc.GetDamage(100);
        }
    }
}
