using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeArrow : Arrow
{
    public override void Init(GameObject attacker, GameObject target, float damage, float speed)
    {
        base.Init(attacker, target, damage, speed);

        OnHitGround = null;
        OnHitGround += Earthquake;
    }

    public void Earthquake()
    {
        if (Camera.main.TryGetComponent<CameraShake>(out var cam))
            cam.ShakeCamera(1f, 10, 3);

        var colliders = Physics2D.OverlapCircleAll(transform.position, 2);

        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].CompareTag("Player"))
                if (colliders[i].TryGetComponent<PlayerController>(out var player))
                    player.GetDamage(100 - 10 * Mathf.Abs(transform.position.x - player.transform.position.x));
    }
}
