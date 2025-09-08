using System.Collections;
using UnityEngine;

public class JumpShot : Skill
{
    private Rigidbody2D rb;
    private PlayerMovement move;

    public override void Attack()
    {
        if (IsUsable())
        {
            // skill

            StartCoroutine("Cast");

            StartCoroutine(OnCoolTime());
        }
    }

    private IEnumerator Cast()
    {
        if (!rb) TryGetComponent<Rigidbody2D>(out rb);
        if (!move) TryGetComponent<PlayerMovement>(out move);


        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 20f), ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(data.skillSFX);

        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        for (int i = 0; i < 3; i++)
        {
            var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

            if (arrow.TryGetComponent<Arrow>(out var arrowComponent))
            {
                arrowComponent.Init(gameObject, GetComponent<PlayerController>().enemy, 80 + Random.Range(0, 30), 10f);
                if (TryGetComponent<AttackManager>(out var attackManager))
                {
                    arrow.transform.position = attackManager.shootPos.position;
                    arrow.GetComponent<Rigidbody2D>().velocity = (Vector3.right * move.directionX * 10f) + (Vector3.down * i * 10);
                }
            }
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        rb.velocity = Vector3.down * 5f;
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        rb.gravityScale = 1f;
    }
}
