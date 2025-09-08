using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : PoolObject
{
    private GameObject attacker;
    private GameObject target;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float damage;
    private float speed;

    private bool hittable = false;

    protected Action OnHitGround;

    [SerializeField] private bool isOneHit = true;

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out sr);
        TryGetComponent<Rigidbody2D>(out rb);
    }

    private void OnEnable()
    {
        var color = sr.color;
        color.a = 1f;
        sr.color = color;

        rb.gravityScale = 1f;
    }

    public virtual void Init(GameObject attacker, GameObject target, float damage, float speed)
    {
        this.attacker = attacker;
        this.target = target;

        this.damage = damage;
        this.speed = speed;

        hittable = true;

        rb.gravityScale = 1f;

        Invoke("Dissappear", 5.75f);
    }

    private void FixedUpdate()
    {
        if (!hittable) return;

        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hittable) return;

        if(collision.gameObject == target)
        {
            if (target.TryGetComponent<PlayerController>(out var enemy))
            {
                if (!enemy.isDead)
                {
                    enemy.GetDamage(damage);
                    if (isOneHit)
                        ReturnToPool();
                }
            }
        }

        if(collision.CompareTag("Ground"))
        {
            hittable = false;
            OnHitGround?.Invoke();

            StartCoroutine("FadeOut", 1);
        }
    }

    private void Dissappear()
    {
        ReturnToPool();
    }

    private void OnDisable()
    {
        StopCoroutine("FadeOut");
        CancelInvoke();
    }

    private IEnumerator FadeOut(float duration)
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;

        Color color = sr.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            sr.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Dissappear();
    }
}
