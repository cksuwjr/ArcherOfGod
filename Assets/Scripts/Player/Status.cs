using System;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float hp;

    [SerializeField] protected float attackSpeed;

    [SerializeField] protected float moveSpeed;

    public Action<float, float> OnChangeHp;

    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float HP
    {
        get { return hp; }
        set 
        { 
            hp = Mathf.Clamp(value, 0, MaxHp); 
            OnChangeHp?.Invoke(hp, maxHp);
        }
    }

    public float AttackSpeed { get { return attackSpeed; } set {  attackSpeed = value; } }
    public float MoveSpeed { get { return moveSpeed; } set {  moveSpeed = value; } }
}
