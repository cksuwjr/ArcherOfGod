using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private IMove movement;
    private IInputHandle inputHandle;

    private Status status;
    private AttackManager attackManager;
    private Animator animator;

    public GameObject enemy;

    public bool isDead = false;
    public event Action OnDie;

    public Light2D spotLight;

    private void Awake()
    {
        TryGetComponent<IMove>(out movement);
        TryGetComponent<IInputHandle>(out inputHandle);

        TryGetComponent<Status>(out status);
        TryGetComponent<AttackManager>(out attackManager);

        animator = GetComponentInChildren<Animator>();

        spotLight = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        if (inputHandle is not null)
        {
            movement?.Move(inputHandle.GetInput());

            if (Input.GetKeyDown(KeyCode.Q))
                attackManager.CastingSkill_1();
            if (Input.GetKeyDown(KeyCode.W))
                attackManager.CastingSkill_2();
            if (Input.GetKeyDown(KeyCode.E))
                attackManager.CastingSkill_3();
            if (Input.GetKeyDown(KeyCode.R))
                attackManager.CastingSkill_4();
            if (Input.GetKeyDown(KeyCode.T))
                attackManager.CastingSkill_5();
        }


        if (!movement.IsMove)
            attackManager?.BasicAttack();
        else
            attackManager.isAttack = false;
    }

    public void Init()
    {
        attackManager?.Init();
        movement?.Init();

        isDead = false;
    }

    public void StopAct()
    {
        movement?.Stop();
        attackManager?.Stop();
    }

    public void GetDamage(float value)
    {
        if(isDead) return;

        status.HP -= value;


        var dmgObject = PoolManager.Instance.damagePool.GetPoolObject();
        dmgObject.transform.position = transform.position;
        if (dmgObject.TryGetComponent<DamageUI>(out var dmg))
            dmg.Init(value);

        var particleObject = PoolManager.Instance.particlePool.GetPoolObject();
        particleObject.transform.position = transform.position;
        if (particleObject.TryGetComponent<ParticleEffect>(out var particle))
            particle.Init();

        if (status.HP < 1)
            Die();
        else
            Hit();

    }

    private void Hit()
    {
        if (!attackManager.isAttack)
            animator?.SetTrigger("Hit");

        if (gameObject == GameManager.Instance.player01)
        {
            StopCoroutine("HitEffect");
            StartCoroutine("HitEffect");
        }
    }

    private IEnumerator HitEffect()
    {
        float time = 0f;
        
        if (Camera.main.TryGetComponent<CameraShake>(out var cam))
            cam.ShakeCamera(1f);

        var duration = 1f;
        while (time < (duration / 2f))
        {
            time += Time.deltaTime;
            UIManager.Instance.BloodScreen(time / (duration / 2f));
            yield return null;
        }
        time = 0f;
        while (time < (duration / 2f))
        {
            time += Time.deltaTime;
            UIManager.Instance.BloodScreen(1 - (time / (duration / 2f)));
            yield return null;
        }
    }

    private void Die()
    {
        if (gameObject == GameManager.Instance.player01)
        {
            SoundManager.Instance.PlaySound(DataManager.Instance.GetAudioDataById(0));
            UIManager.Instance.SetResultText("Fail ..");
            if(GameManager.Instance.player02.TryGetComponent<PlayerController>(out var player02Con))
                player02Con.StopAct();
        }
        if (gameObject == GameManager.Instance.player02)
        {
            SoundManager.Instance.PlaySound(DataManager.Instance.GetAudioDataById(1));
            UIManager.Instance.SetResultText("Victory !!");
            if (GameManager.Instance.player01.TryGetComponent<PlayerController>(out var player01Con))
                player01Con.StopAct();
        }

        animator?.SetTrigger("Dead");
        StopAct();

        isDead = true;

        GameManager.Instance.GameEnd();
    }

    public void NarrowSight(bool tf)
    {
        StopCoroutine("NarrowSightSlowly");
        StartCoroutine("NarrowSightSlowly", tf);
    }

    private IEnumerator NarrowSightSlowly(bool tf)
    {
        if (tf)
        {
            while (spotLight.pointLightOuterRadius > 1f)
            {
                spotLight.pointLightOuterRadius -= Time.deltaTime * 40f;
                spotLight.pointLightOuterRadius = Mathf.Clamp(spotLight.pointLightOuterRadius, 5f, 70f);
                yield return null;
            }
        }
        else
        {
            while (spotLight.pointLightOuterRadius < 70f)
            {
                spotLight.pointLightOuterRadius += Time.deltaTime * 40f;
                spotLight.pointLightOuterRadius = Mathf.Clamp(spotLight.pointLightOuterRadius, 5f, 70f);
                yield return null;
            }
        }
    }
}
