using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheel : MonoBehaviour
{
    [Header("Вращение")]
    public float time; // Минимальное время вращения
    public AnimationCurve animationCurve; // Кривая скорости вращения
    public BonusGenerator[] bgs; // Массив генераторов бонусов
    public GameObject flyingReward, rewardEffect; // Эффекты и префабы
    public bool IsRolling { get; private set; }

    [Header("Элементы интерфейса")]
    public Text txtCounter;
    public Button btnSpin;
    public Image imgFill;
    //public Image imgArrow;
    public GameObject gobjFlag;
    public RectTransform wheelRect;
    public RectTransform window;

    [Header("Уровни")]
    public List<int> levels;
    public List<float> modifiers;

    [Header("Псевдорандом")]
    public int[] nums;

    private Sector[] sectors;
    private int curSector, num;
    private bool speedUp = false, opened = false;
    private float anglePerItem;
    private Island island;
    private GameObject _flyingReward, _rewardEffect;
    private Vector2 startPos;

    public static LuckyWheel Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        IsRolling = false;

        float b = ((float)Screen.width / (float)Screen.height) / (1125f / 2436f);
        startPos = new Vector2(window.anchoredPosition.x * b, window.anchoredPosition.y);
        window.anchoredPosition = startPos;
    }

    private void Start()
    {
        island = Island.Instance;

        num = island.GetParameter("LuckyWheelNum", 0);

        sectors = new Sector[wheelRect.childCount];
        for(int i = 0; i < wheelRect.childCount; i++)
        {
            sectors[i] = wheelRect.GetChild(i).GetComponent<Sector>();
        }

        EventManager.Subscribe("ChangeLifebuoy", UpdateSpinButton);
        EventManager.Subscribe("ChangeLifebuoyMax", UpdateSpinButton);
        EventManager.Subscribe("LevelUp", UpdateSpinButton);

        UpdateSpinButton(new object[0]);

        anglePerItem = 360f / sectors.Length;
    }

    private void Update()
    {
        if(opened && window.anchoredPosition.x != 0)
        {
            window.anchoredPosition = Vector2.MoveTowards(window.anchoredPosition, Vector2.zero, Time.deltaTime * 5000f);
        }
        else if (!opened && window.anchoredPosition.x != startPos.x)
        {
            window.anchoredPosition = Vector2.MoveTowards(window.anchoredPosition, startPos, Time.deltaTime * 5000f);
        }
    }

    private void UpdateSpinButton(object[] args)
    {
        if (island.Lifebuoy > 0 && !gobjFlag.activeSelf) gobjFlag.SetActive(true);
        else if (island.Lifebuoy <= 0 && gobjFlag.activeSelf) gobjFlag.SetActive(false);
        btnSpin.interactable = island.Lifebuoy > 0 && (!IsRolling || !speedUp);
        txtCounter.text = island.Lifebuoy + "/" + island.LifebuoyMax;
        imgFill.fillAmount = ((float)island.Lifebuoy / (float)island.LifebuoyMax) * 0.62f + 0.19f;
    }

    public void Roll()
    {
        if (!IsRolling && island.ChangeLifebuoy(-1))
        {
            if (num < nums.Length)
            {
                curSector = nums[num++];
                island.SetParameter("LuckyWheelNum", num);
            }
            else
            {
                curSector = UnityEngine.Random.Range(0, wheelRect.childCount);
            }
            float maxAngle = 360f + 360f * time + ((curSector + 6) * anglePerItem) + (anglePerItem / 2f);
            StartCoroutine(Rolling(3f * time, maxAngle));
        }
        else if (IsRolling && !speedUp)
        {
            speedUp = true;
            float maxAngle = 360f * time + ((curSector + 6) * anglePerItem) + (anglePerItem / 2f);
            StopAllCoroutines();
            StartCoroutine(Rolling(time / 2f, maxAngle));
        }
    }

    private IEnumerator Rolling(float time, float maxAngle)
    {
        IsRolling = true;
        btnSpin.interactable = !IsRolling || !speedUp;
        float timer = 0.0f;
        float startAngle = wheelRect.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        while (timer < time)
        {
            float angle = maxAngle * animationCurve.Evaluate(timer / time);
            wheelRect.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return null;
        }

        wheelRect.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        IsRolling = false;
        speedUp = false;
        Reward();
        UpdateSpinButton(new object[0]);
    }

    private void Reward()
    {
        //Taptic.Heavy();

        sectors[curSector].Reward();

        _flyingReward = Instantiate(flyingReward, window);
        _flyingReward.GetComponent<FlyingWheelReward>().text.text = sectors[curSector].GetComponentInChildren<Text>().text;
        _flyingReward.GetComponent<FlyingWheelReward>().image.sprite = sectors[curSector].GetComponentInChildren<Image>().sprite;
        _flyingReward.GetComponent<FlyingWheelReward>().image.color = sectors[curSector].GetComponentInChildren<Image>().color;

        _rewardEffect = Instantiate(rewardEffect, window);

        Taptic.Light();
    }

    public void Switch()
    {
        opened = !opened;
    }
}

public enum LuckyWheelRewardType { Money, Bonus, Premium, Polundra };
