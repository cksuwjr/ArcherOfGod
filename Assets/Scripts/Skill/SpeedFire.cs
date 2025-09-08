using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedFire : Skill
{
    private Status status;

    private void Awake()
    {
        TryGetComponent<Status>(out status);
    }

    public override void Attack()
    {
        if (IsUsable())
        {
            StartCoroutine("Cast");

            StartCoroutine(OnCoolTime());
        }
    }

    private IEnumerator Cast()
    {
        status.AttackSpeed = 3;
        attackManager.BasicAttackInitialize();
        yield return YieldInstructionCache.WaitForSeconds(3f);
        status.AttackSpeed = 1f;
    }
}
