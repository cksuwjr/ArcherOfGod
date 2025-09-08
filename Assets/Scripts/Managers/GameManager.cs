using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IManager
{
    private UIManager uiManager;
    private IManager poolManager;
    private IManager soundManager;

    public GameObject player01;
    public GameObject player02;

    public Action OnTimerEnd;
    public Action OnGameEnd;

    protected override void DoAwake()
    {
        base.DoAwake();
        Init();
    }

    public void Init()
    {
        player01 = GameObject.Find("Player01");
        player02 = GameObject.Find("Player02");

        if (player01.TryGetComponent<PlayerController>(out var player01Con))
            player01Con.OnDie += GameEnd;
        if (player02.TryGetComponent<PlayerController>(out var player02Con))
            player02Con.OnDie += GameEnd;


        GameObject.Find("UIManager").TryGetComponent<UIManager>(out uiManager);
        GameObject.Find("PoolManager").TryGetComponent<IManager>(out poolManager);
        GameObject.Find("SoundManager").TryGetComponent<IManager>(out soundManager);

        uiManager?.Init();
        poolManager?.Init();
        soundManager?.Init();



        uiManager.OnStartEffectEnd += () =>
        {
            player01Con?.Init();
            player02Con?.Init();

            if (player02.TryGetComponent<EnemyAI>(out var enemyAI))
                enemyAI.Init();

            StartCoroutine("Timer");
        };
        uiManager.StartCountDown();

    }

    private IEnumerator Timer()
    {
        var time = 90f;
        uiManager.SetTimeText(((int)time).ToString());

        while(time > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            time -= 1f;
            uiManager.SetTimeText(((int)time).ToString());
        }
        uiManager.SetTimeText("");
        OnTimerEnd?.Invoke();
    }

    public void GameEnd()
    {
        if (player01.TryGetComponent<PlayerController>(out var player01Con))
            player01Con.OnDie -= GameEnd;
        if (player02.TryGetComponent<PlayerController>(out var player02Con))
            player02Con.OnDie -= GameEnd;

        uiManager.SetRegamePannel(true);
        OnGameEnd?.Invoke();
        StopAllCoroutines();
    }

    public void ReStartGame()
    {

        if (player01.TryGetComponent<Status>(out var player01stat))
            player01stat.HP = player01stat.MaxHp;

        if (player02.TryGetComponent<Status>(out var player02stat))
            player02stat.HP = player02stat.MaxHp;
        
        player01.transform.position = new Vector3(-6.73f, -1.01f);
        player02.transform.position = new Vector3(6.73f, -1.01f);


        uiManager.SetRegamePannel(false);
        uiManager.StartCountDown();
    }
}
