using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMove
{
    [SerializeField] private Vector2 areaConfineX = new Vector2(-10f, -1.85f);
    private Vector3 moveDelta;

    private Status status;

    private Animator animator;
    private int animParam_IsWalk = Animator.StringToHash("IsWalk");

    [SerializeField] private int startDir = 1;
    public bool IsMove { get; protected set; }
    public int directionX;

    private float isMoveTimer = 0f;
    private const float isMoveDelay = 0.1f;

    private bool movable = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        TryGetComponent<Status>(out status);
    }

    public void Init()
    {
        movable = true;

        if (gameObject == GameManager.Instance.player01)
        {
            UIManager.OnLeftButtonPressed += () => Move(Vector3.left);
            UIManager.OnRightButtonPressed += () => Move(Vector3.right);
        }
    }

    public void Stop()
    {
        movable = false;
        if(TryGetComponent<PlayerController>(out var player))
            if(!player.isDead)
                animator?.SetBool(animParam_IsWalk, false);
    }

    public void Move(Vector3 direction)
    {
        if (!movable) return;

        animator.speed = 1f;

        moveDelta.x = direction.x;
        moveDelta.y = 0;
        moveDelta.z = 0;

        moveDelta *= status.MoveSpeed * Time.deltaTime;

        if (startDir == 1)
            directionX = direction.x >= 0 ? 1 : -1;
        else
            directionX = direction.x > 0 ? 1 : -1;

        var animIsWalk = Mathf.Abs(moveDelta.x) > 0f;

        animator?.SetBool(animParam_IsWalk, animIsWalk);

        if (animIsWalk)
        {
            isMoveTimer = isMoveDelay;
            IsMove = true;
        }
        else 
            IsMove = false;


        transform.localScale = new Vector3(directionX, 1, 1);

        moveDelta += transform.position;

        moveDelta.x = Mathf.Clamp(moveDelta.x, areaConfineX.x, areaConfineX.y);

        transform.position = moveDelta;

    }

    private void Update()
    {
        if (IsMove)
        {
            isMoveTimer -= Time.deltaTime;
            if (isMoveTimer <= 0f)
            {
                IsMove = false;
            }
        }

        var x = Mathf.Clamp(transform.position.x, areaConfineX.x, areaConfineX.y);

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
