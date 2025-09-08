using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ButtonType
{
    LeftButton,
    RightButton,

    SkillSlot1,
    SkillSlot2,
    SkillSlot3,
    SkillSlot4,
    SkillSlot5,
}

public class UIManager : SingletonDestroy<UIManager>, IManager
{
    private GameObject player1UI;
    private GameObject player2UI;

    private Image player1WhiteUI;
    private Image player1HpUI;
    private Image player2WhiteUI;
    private Image player2HpUI;

    private GameObject moveButtons;
    private MoveButton leftButton;
    private MoveButton rightButton;

    private GameObject skillUI;

    private Image Skill1Image;
    private Image Skill2Image;
    private Image Skill3Image;
    private Image Skill4Image;
    private Image Skill5Image;

    private GameObject Skill1CoolDownImage;
    private GameObject Skill2CoolDownImage;
    private GameObject Skill3CoolDownImage;
    private GameObject Skill4CoolDownImage;
    private GameObject Skill5CoolDownImage;

    private TextMeshProUGUI Skill1CoolDownText;
    private TextMeshProUGUI Skill2CoolDownText;
    private TextMeshProUGUI Skill3CoolDownText;
    private TextMeshProUGUI Skill4CoolDownText;
    private TextMeshProUGUI Skill5CoolDownText;



    private GameObject startUI;

    private GameObject start3;
    private GameObject start2;
    private GameObject start1;
    private GameObject startText;
    private GameObject letsLock;

    private TextMeshProUGUI timeText;
    private Image bloodScreen;
    private GameObject regamePannel;


    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;

    public static event Action OnSkillSlot1Pressed;
    public static event Action OnSkillSlot2Pressed;
    public static event Action OnSkillSlot3Pressed;
    public static event Action OnSkillSlot4Pressed;
    public static event Action OnSkillSlot5Pressed;

    public event Action OnStartEffectEnd;

    public void Init()
    {
        var canvas = GameObject.Find("Canvas");


        // Left Top
        var leftTop = canvas.transform.GetChild(0);
        var leftBottom = canvas.transform.GetChild(1);
        var rightTop = canvas.transform.GetChild(2);
        var rightBottom = canvas.transform.GetChild(3);


        player1UI = leftTop.GetChild(0).gameObject;
        player2UI = rightTop.GetChild(0).gameObject;


        player1UI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).TryGetComponent<Image>(out player1WhiteUI);
        player1UI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).TryGetComponent<Image>(out player1HpUI);

        if (GameManager.Instance.player01.TryGetComponent<Status>(out var stat1))
        {
            stat1.OnChangeHp += (hp, maxHp) => 
            { 
                StopCoroutine("HpUIWhiteHandlePlayer01");
                StartCoroutine("HpUIWhiteHandlePlayer01", hp / maxHp);

                player1HpUI.fillAmount = hp / maxHp; 
                player1HpUI.GetComponentInChildren<TextMeshProUGUI>().text = ((int)hp).ToString();
            };
        }

        player2UI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).TryGetComponent<Image>(out player2WhiteUI);
        player2UI.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).TryGetComponent<Image>(out player2HpUI);

        if (GameManager.Instance.player02.TryGetComponent<Status>(out var stat2))
        {
            stat2.OnChangeHp += (hp, maxHp) =>
            {
                StopCoroutine("HpUIWhiteHandlePlayer02");
                StartCoroutine("HpUIWhiteHandlePlayer02", hp / maxHp);

                player2HpUI.fillAmount = hp / maxHp;
                player2HpUI.GetComponentInChildren<TextMeshProUGUI>().text = ((int)hp).ToString();
            };
        }


        moveButtons = leftBottom.GetChild(0).gameObject;
        leftBottom.GetChild(0).GetChild(0).TryGetComponent<MoveButton>(out leftButton);
        leftBottom.GetChild(0).GetChild(1).TryGetComponent<MoveButton>(out rightButton);

        leftButton.Pressed += () => HandleButtonClick(ButtonType.LeftButton);
        rightButton.Pressed += () => HandleButtonClick(ButtonType.RightButton);


        skillUI = rightBottom.GetChild(0).gameObject;

        if (skillUI.transform.GetChild(0).TryGetComponent<Button>(out var skillBtn1))
            skillBtn1.onClick.AddListener(() => HandleButtonClick(ButtonType.SkillSlot1));
        if (skillUI.transform.GetChild(1).TryGetComponent<Button>(out var skillBtn2))
            skillBtn2.onClick.AddListener(() => HandleButtonClick(ButtonType.SkillSlot2));
        if (skillUI.transform.GetChild(2).TryGetComponent<Button>(out var skillBtn3))
            skillBtn3.onClick.AddListener(() => HandleButtonClick(ButtonType.SkillSlot3));
        if (skillUI.transform.GetChild(3).TryGetComponent<Button>(out var skillBtn4))
            skillBtn4.onClick.AddListener(() => HandleButtonClick(ButtonType.SkillSlot4));
        if (skillUI.transform.GetChild(4).TryGetComponent<Button>(out var skillBtn5))
            skillBtn5.onClick.AddListener(() => HandleButtonClick(ButtonType.SkillSlot5));


        skillUI.transform.GetChild(0).GetChild(0).TryGetComponent<Image>(out Skill1Image);
        skillUI.transform.GetChild(1).GetChild(0).TryGetComponent<Image>(out Skill2Image);
        skillUI.transform.GetChild(2).GetChild(0).TryGetComponent<Image>(out Skill3Image);
        skillUI.transform.GetChild(3).GetChild(0).TryGetComponent<Image>(out Skill4Image);
        skillUI.transform.GetChild(4).GetChild(0).TryGetComponent<Image>(out Skill5Image);

        Skill1Image.sprite = DataManager.Instance.GetSkillDataById(1).icon;
        Skill2Image.sprite = DataManager.Instance.GetSkillDataById(2).icon;
        Skill3Image.sprite = DataManager.Instance.GetSkillDataById(3).icon;
        Skill4Image.sprite = DataManager.Instance.GetSkillDataById(4).icon;
        Skill5Image.sprite = DataManager.Instance.GetSkillDataById(5).icon;

        Skill1Image.gameObject.SetActive(true);
        Skill2Image.gameObject.SetActive(true);
        Skill3Image.gameObject.SetActive(true);
        Skill4Image.gameObject.SetActive(true);
        Skill5Image.gameObject.SetActive(true);


        Skill1CoolDownImage = skillUI.transform.GetChild(0).GetChild(2).gameObject;
        Skill2CoolDownImage = skillUI.transform.GetChild(1).GetChild(2).gameObject;
        Skill3CoolDownImage = skillUI.transform.GetChild(2).GetChild(2).gameObject;
        Skill4CoolDownImage = skillUI.transform.GetChild(3).GetChild(2).gameObject;
        Skill5CoolDownImage = skillUI.transform.GetChild(4).GetChild(2).gameObject;

        Skill1CoolDownImage.transform.GetChild(0).TryGetComponent(out Skill1CoolDownText);
        Skill2CoolDownImage.transform.GetChild(0).TryGetComponent(out Skill2CoolDownText);
        Skill3CoolDownImage.transform.GetChild(0).TryGetComponent(out Skill3CoolDownText);
        Skill4CoolDownImage.transform.GetChild(0).TryGetComponent(out Skill4CoolDownText);
        Skill5CoolDownImage.transform.GetChild(0).TryGetComponent(out Skill5CoolDownText);


        startUI = canvas.transform.GetChild(4).gameObject;

        start3 = startUI.transform.GetChild(0).gameObject;
        start2 = startUI.transform.GetChild(1).gameObject;
        start1 = startUI.transform.GetChild(2).gameObject;
        startText = startUI.transform.GetChild(3).gameObject;
        letsLock = startUI.transform.GetChild(4).gameObject;

        canvas.transform.GetChild(5).TryGetComponent<TextMeshProUGUI>(out timeText);

        canvas.transform.GetChild(6).TryGetComponent<Image>(out bloodScreen);

        regamePannel = canvas.transform.GetChild(7).gameObject;
    }

    private void HandleButtonClick(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.LeftButton:
                OnLeftButtonPressed?.Invoke();
                break;
            case ButtonType.RightButton:
                OnRightButtonPressed?.Invoke(); 
                break;

            case ButtonType.SkillSlot1:
                OnSkillSlot1Pressed?.Invoke();
                break;
            case ButtonType.SkillSlot2:
                OnSkillSlot2Pressed?.Invoke();
                break;
            case ButtonType.SkillSlot3:
                OnSkillSlot3Pressed?.Invoke();
                break;
            case ButtonType.SkillSlot4:
                OnSkillSlot4Pressed?.Invoke();
                break;
            case ButtonType.SkillSlot5:
                OnSkillSlot5Pressed?.Invoke();
                break;
        }
    }

    public void StartCountDown()
    {
        StartCoroutine("CountDown");
    }

    private IEnumerator CountDown()
    {
        SetUnVisibleGameUI();

        startUI.SetActive(true);
        start3.SetActive(true);
        startText.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        start3.SetActive(false);
        start2.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        start2.SetActive(false);
        start1.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        start1.SetActive(false);
        startText.SetActive(false);
        letsLock.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        letsLock.SetActive(false);


        moveButtons.SetActive(true);

        player1UI.SetActive(true);
        player2UI.SetActive(true);
        skillUI.SetActive(true);
        startUI.SetActive(false);

        OnStartEffectEnd?.Invoke();
    }

    private IEnumerator HpUIWhiteHandlePlayer01(float amount)
    {
        float time = 1f;
        while (time > 0) {
            player1WhiteUI.fillAmount = Mathf.Lerp(player1WhiteUI.fillAmount, amount, 1 - time / 1f);
            time -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator HpUIWhiteHandlePlayer02(float amount)
    {
        float time = 1f;
        while (time > 0)
        {
            player2WhiteUI.fillAmount = Mathf.Lerp(player2WhiteUI.fillAmount, amount, 1f - time / 1f);
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void BloodScreen(float value)
    {
        var color = bloodScreen.color;
        color.a = value;
        bloodScreen.color = color;
    }

    public void SetCoolDownText(ButtonType type, float value)
    {
        switch (type)
        {
            case ButtonType.SkillSlot1:
                Skill1CoolDownImage.SetActive(value > 0);
                Skill1CoolDownText.text = ((int)value).ToString();
                break;
            case ButtonType.SkillSlot2:
                Skill2CoolDownImage.SetActive(value > 0);
                Skill2CoolDownText.text = ((int)value).ToString();
                break;
            case ButtonType.SkillSlot3:
                Skill3CoolDownImage.SetActive(value > 0);
                Skill3CoolDownText.text = ((int)value).ToString();
                break;
            case ButtonType.SkillSlot4:
                Skill4CoolDownImage.SetActive(value > 0);
                Skill4CoolDownText.text = ((int)value).ToString();
                break;
            case ButtonType.SkillSlot5:
                Skill5CoolDownImage.SetActive(value > 0);
                Skill5CoolDownText.text = ((int)value).ToString();
                break;
        }
    }

    public void SetUnVisibleGameUI()
    {
        moveButtons.SetActive(false);
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        skillUI.SetActive(false);
        startUI.SetActive(false);

        start3.SetActive(false);
        start2.SetActive(false);
        start1.SetActive(false);

        timeText.text = "";
    }

    public void SetRegamePannel(bool tf)
    {
        regamePannel.SetActive(tf);
    }

    public void SetResultText(string resultText)
    {
        if (regamePannel.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var text))
            text.text = resultText;
    }
}
