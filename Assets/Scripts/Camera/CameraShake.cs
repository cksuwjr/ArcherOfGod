using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPos;

    float m_roughness = 5;
    float m_magnitude = 2;

    private Transform uiCamera;

    private void Awake()
    {
        originPos = transform.position;
        Debug.Log("나 여기있어");

        uiCamera = GameObject.FindGameObjectWithTag("UICamera").transform;
    }

    public void ShakeCamera(float time, float roughness = 5f, float magnitude = 2f)
    {
        m_roughness = roughness;
        m_magnitude = magnitude;

        StopCoroutine("ShakingCamera");
        StartCoroutine("ShakingCamera", time);
    }

    private IEnumerator ShakingCamera(float duration)
    {
        float halfDuration = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;
            tick += Time.deltaTime * m_roughness;

            transform.position = originPos + new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f,
                0f) * m_magnitude * Mathf.PingPong(elapsed, halfDuration);
            uiCamera.position = transform.position;

            yield return null;
        }

        uiCamera.position = transform.position = originPos;
    }
}
