using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogArrow : Arrow
{
    [SerializeField] private GameObject fogObject;
    private GameObject fogInstance;

    public override void Init(GameObject attacker, GameObject target, float damage, float speed)
    {
        base.Init(attacker, target, damage, speed);

        OnHitGround = null;
        OnHitGround += SetFog;
    }

    public void SetFog()
    {
        if(fogInstance == null)
            fogInstance = Instantiate(fogObject);

        fogInstance.transform.position = transform.position;
        fogInstance.SetActive(true);
    }
}
