using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour, IAttack
{
    protected AttackManager attackManager;

    public float Cooltime { get; protected set; }

    protected bool isUsable = true;

    protected SkillData data;

    public Sprite Icon => data.icon;
    public string SkillName => data.skillName;

    private float remainTime = 0;

    public abstract void Attack();

    public Action<float> OnChangeRemainTime;

    public void Init(AttackManager attack, SkillData data)
    {
        attackManager = attack;
        this.data = data;
        Cooltime = data.cooldown;
    }

    protected IEnumerator OnCoolTime()
    {
        isUsable = false;
        OnChangeRemainTime?.Invoke(Cooltime);
        remainTime = 0;

        while (remainTime < Cooltime)
        {
            remainTime += Time.deltaTime;
            OnChangeRemainTime?.Invoke(Cooltime - remainTime);

            yield return null;
        }

        remainTime = Cooltime;
        OnChangeRemainTime?.Invoke(0);

        isUsable = true;
    }

    protected bool IsUsable()
    {
        // 사용조건 여기다가

        if (!isUsable)
            return false;
        else
            return true;
    }

    public void ReturnCoolDown(float time)
    {
        remainTime += time;
    }
}
