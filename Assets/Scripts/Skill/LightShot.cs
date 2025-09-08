using System.Collections;
using UnityEngine;

public class LightShot : Skill
{
    private Animator animator;

    private int animAttackHash = Animator.StringToHash("SkillAttack");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void Attack()
    {
        if (IsUsable())
        {
            // skill

            animator.SetTrigger(animAttackHash);

            StartCoroutine("Cast");

            StartCoroutine(OnCoolTime());
        }
    }

    private IEnumerator Cast()
    {
        attackManager.isAttack = true;
        SoundManager.Instance.PlaySound(data.skillSFX);

        var arrow = PoolManager.Instance.fogArrowPool.GetPoolObject();
        if (arrow.TryGetComponent<Arrow>(out var arrowComponent))
        {
            arrowComponent.Init(gameObject, GetComponent<PlayerController>().enemy, 0, 0f);
            if (TryGetComponent<AttackManager>(out var attackManager))
            {
                arrow.transform.position = attackManager.shootPos.position;
                arrow.GetComponent<Rigidbody2D>().velocity =
                    BasicAttack.GetLaunchVelocity_ByApex(
                        arrow.transform.position,
                        GetComponent<PlayerController>().enemy.transform.position,
                        Mathf.Abs(GetComponent<PlayerController>().enemy.transform.position.x - transform.position.x) / 7f);


                //rb.AddForce(transform.position * this.speed);
            }
        }

        yield return YieldInstructionCache.WaitForSeconds(0.2f);
    }
}
