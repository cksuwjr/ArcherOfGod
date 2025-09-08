using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour, IManager
{
    private Skill basicAttack;

    private List<Skill> skillList = new List<Skill>();

    public Transform shootPos;

    public bool isInit = false;
    public bool isAttack = false;

    public void Init()
    {
        Skill skill;

        skill = gameObject.AddComponent<BasicAttack>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(0));
        basicAttack = skill;

        skill = gameObject.AddComponent<Dash>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(1));
        skillList.Add(skill);

        skill = gameObject.AddComponent<JumpShot>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(2));
        skillList.Add(skill);

        skill = gameObject.AddComponent<SpeedFire>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(3));
        skillList.Add(skill);

        skill = gameObject.AddComponent<EarthquakeShot>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(4));
        skillList.Add(skill);

        skill = gameObject.AddComponent<LightShot>();
        skill.Init(this, DataManager.Instance.GetSkillDataById(5));
        skillList.Add(skill);

        shootPos = transform.GetChild(1).transform;


        isInit = true;

        if (gameObject == GameManager.Instance.player01)
        {
            UIManager.OnSkillSlot1Pressed += CastingSkill_1;
            UIManager.OnSkillSlot2Pressed += CastingSkill_2;
            UIManager.OnSkillSlot3Pressed += CastingSkill_3;
            UIManager.OnSkillSlot4Pressed += CastingSkill_4;
            UIManager.OnSkillSlot5Pressed += CastingSkill_5;

            skillList[0].OnChangeRemainTime += (value) => UIManager.Instance.SetCoolDownText(ButtonType.SkillSlot1, value);
            skillList[1].OnChangeRemainTime += (value) => UIManager.Instance.SetCoolDownText(ButtonType.SkillSlot2, value);
            skillList[2].OnChangeRemainTime += (value) => UIManager.Instance.SetCoolDownText(ButtonType.SkillSlot3, value);
            skillList[3].OnChangeRemainTime += (value) => UIManager.Instance.SetCoolDownText(ButtonType.SkillSlot4, value);
            skillList[4].OnChangeRemainTime += (value) => UIManager.Instance.SetCoolDownText(ButtonType.SkillSlot5, value);
        }
    }

    public void Stop()
    {
        isInit = false;
    }

    public void BasicAttack()
    {
        if (!isInit) return;

        basicAttack.Attack();
    }

    public void BasicAttackInitialize()
    {
        basicAttack.ReturnCoolDown(10);
    }

    public void CastingSkill_1()
    {
        if (!isInit) return;

        skillList[0].Attack();
    }

    public void CastingSkill_2()
    {
        if (!isInit) return;

        skillList[1].Attack();
    }

    public void CastingSkill_3()
    {
        if (!isInit) return;

        skillList[2].Attack();
    }

    public void CastingSkill_4()
    {
        if (!isInit) return;

        skillList[3].Attack();
    }

    public void CastingSkill_5()
    {
        if (!isInit) return;

        skillList[4].Attack();
    }
}
