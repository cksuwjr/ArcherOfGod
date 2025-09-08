using System.Collections;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnTimerEnd += SpawnRock;
        GameManager.Instance.OnGameEnd += StopSpawn;
    }

    public void SpawnRock()
    {
        StartCoroutine("RockRain");
    }

    public void StopSpawn()
    {
        StopCoroutine("RockRain");
    }

    private IEnumerator RockRain()
    {
        while (true)
        {
            var rock = PoolManager.Instance.rockPool.GetPoolObject();
            rock.transform.position = new Vector3(Random.Range(-10, 10), 10, 0);
            yield return YieldInstructionCache.WaitForSeconds(1.5f);
        }
    }

    
}
