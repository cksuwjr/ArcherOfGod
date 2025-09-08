using System.Collections;
using UnityEngine;

public class Dash : Skill
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


        rb.AddForce(new Vector2(20 * move.directionX, 0f), ForceMode2D.Impulse);

        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        rb.velocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.15f);
            SoundManager.Instance.PlaySound(data.skillSFX);

            var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

            if (arrow.TryGetComponent<Arrow>(out var arrowComponent))
            {
                arrowComponent.Init(gameObject, GetComponent<PlayerController>().enemy, 50 + Random.Range(0, 30), 10f);
                if (TryGetComponent<AttackManager>(out var attackManager))
                {
                    arrow.transform.position = attackManager.shootPos.position + Vector3.up * Random.Range(-0.5f, 0.5f);
                    arrow.GetComponent<Rigidbody2D>().velocity = Vector3.right * move.directionX * 10;
                    arrow.GetComponent<Rigidbody2D>().gravityScale = 0f;
                }
            }
        }
    }
}
