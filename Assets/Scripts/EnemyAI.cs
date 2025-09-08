using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
}

public class EnemyAI : MonoBehaviour
{
    private PlayerMovement movement;
    private AttackManager attackManager;
    private Status status;

    private EnemyState state;

    public void Init()
    {
        ChangeState(GetRandomState());
    }


    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out movement);
        TryGetComponent<AttackManager>(out attackManager);
        TryGetComponent<Status>(out status);
    }

    public void ChangeState(EnemyState state)
    {
        StopCoroutine(state.ToString());
        this.state = state;
        StartCoroutine(state.ToString());
    }

    private IEnumerator Idle()
    {
        float time = (1f / status.AttackSpeed);
        movement?.Move(Vector3.zero);
        yield return YieldInstructionCache.WaitForSeconds(time);
        
        ChangeState(GetRandomState());
    }

    private IEnumerator Move()
    {
        float time = (1f / status.AttackSpeed);
        float value = 0f;
        int direction = Random.Range(-1, 2);

        attackManager.isAttack = false;
        while (value < time)
        {
            movement?.Move(Vector3.right * direction);
            if (!movement.IsMove)
                attackManager?.BasicAttack();
            else
                attackManager.isAttack = false;
            yield return null;
            value += Time.deltaTime;
        }

        ChangeState(GetRandomState());
    }

    private IEnumerator Attack()
    {
        int num = Random.Range(1, 6);

        switch (num) {
            case 1:
                attackManager.CastingSkill_1();
                break;
            case 2:
                attackManager.CastingSkill_2();
                break;
            case 3:
                attackManager.CastingSkill_3();
                break;
            case 4:
                attackManager.CastingSkill_4();
                break;
            case 5:
                attackManager.CastingSkill_5();
                break;
        }
        yield return YieldInstructionCache.WaitForSeconds(1f);
        ChangeState(GetRandomState());
    }

    private EnemyState GetRandomState()
    {
        EnemyState[] states = (EnemyState[])System.Enum.GetValues(typeof(EnemyState));

        // 제외할 상태 빼기
        EnemyState[] filteredStates = states.Where(s => s != state).ToArray();

        int randomIndex = Random.Range(0, filteredStates.Length);
        return filteredStates[randomIndex];
    }
}
