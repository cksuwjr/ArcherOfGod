using UnityEngine;

public class BasicAttack : Skill
{
    private Status status;
    private Animator animator;

    private int animAttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        TryGetComponent<Status>(out status);
        animator = GetComponentInChildren<Animator>();
        Cooltime = 1f / status.AttackSpeed;
    }

    public override void Attack()
    {
        if (IsUsable() && status.AttackSpeed > 0)
        {
            attackManager.isAttack = true;
            animator.speed = status.AttackSpeed;
            // skill
            animator.SetTrigger(animAttackHash);

            Cooltime = 1f / status.AttackSpeed;
            StartCoroutine(OnCoolTime());
        }
    }

    public void Shoot()
    {
        SoundManager.Instance.PlaySound(data.skillSFX);

        attackManager.isAttack = false;

        var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

        if (arrow.TryGetComponent<Arrow>(out var arrowComponent))
        {
            arrowComponent.Init(gameObject, GetComponent<PlayerController>().enemy, 50 + Random.Range(0, 30), 10f);
            if (TryGetComponent<AttackManager>(out var attackManager))
            {
                arrow.transform.position = attackManager.shootPos.position;
                arrow.GetComponent<Rigidbody2D>().velocity =
                    GetLaunchVelocity_ByApex(
                        arrow.transform.position,
                        GetComponent<PlayerController>().enemy.transform.position,
                        Mathf.Abs(GetComponent<PlayerController>().enemy.transform.position.x - transform.position.x) / 4.5f);


                //rb.AddForce(transform.position * this.speed);
            }
        }
    }

    public static Vector3 GetLaunchVelocity_ByApex(Vector3 origin, Vector3 target, float apexHeight)
    {
        float g = Mathf.Abs(Physics.gravity.y); // 9.81
        Vector3 toTarget = target - origin;

        // 수평 성분(XZ)과 수직 성분(Y) 분리
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float yOffset = toTarget.y;

        // 위로 올라가는 시간(정점까지)
        float tUp = Mathf.Sqrt(2f * Mathf.Max(0.0001f, apexHeight) / g);

        // 정점에서 타깃까지 내려오는 높이 = apexHeight + (origin.y - target.y)
        float descendHeight = apexHeight + (origin.y - target.y);
        if (descendHeight < 0f) descendHeight = 0.0001f; // 수치 안정성

        // 내려오는 시간
        float tDown = Mathf.Sqrt(2f * descendHeight / g);

        float time = tUp + tDown;

        // 초기 수직 속도: vy = g * tUp
        float vy = g * tUp;

        // 초기 수평 속도: vXZ = ΔXZ / time
        Vector3 vXZ = toTargetXZ / time;

        // 최종 초기속도(위로 양의 y)
        return vXZ + Vector3.up * vy;
    }
}
