using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteRotation : MonoBehaviour
{
    public string rouletteName;
    public float time;
    public AnimationCurve animationCurve;
    public bool IsRolling { get; private set; }
    public WheelButton wb;
    public LifebuoyManager lm;
    public BonusGenerator bg;
    public GameObject flyingReward, arrow, rewardEffect;
    public PierManager blackShip;

    [Header("Количество секторов")]
    public int sectorCount;

    [Header("Награда")]
    public SectorController[] sectors;
    public List<int> levels;
    public List<float> modifiers;
    public int[] nums;
    public Button spinButton;

    private RectTransform rect;
    private int section, num;
    private bool speedUp = false;
    private float anglePerItem;
    private Island island;
    private GameObject _flyingReward, _rewardEffect;

    public enum RewardType { Money, Bonus, BlackMark };

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        IsRolling = false;
        island = Island.Instance();
    }

    private void OnEnable()
    {
        InitInfo();
    }

    private void InitInfo()
    {
        for (int i = island.Level; i > 0; i--)
        {
            if (levels.Contains(i))
            {
                float[] mods = modifiers.GetRange(0, levels.IndexOf(i) + 1).ToArray();
                foreach (SectorController sector in sectors)
                {
                    sector.UpdateReward(mods);
                }
                return;
            }
        }
    }

    public void UpdateInfo()
    {
        if (island == null) return;

        if (levels.Contains(island.Level))
        {
            float[] mods = modifiers.GetRange(0, levels.IndexOf(island.Level) + 1).ToArray();
            foreach (SectorController sector in sectors)
            {
                sector.UpdateReward(mods);
            }
            EventManager.SendEvent("UpgradeWheel", mods[mods.Length - 1], wb);
        }
    }

    private void Start()
    {
        island.InitParameter(rouletteName + "_num", 0);
        num = island.GetParameter(rouletteName + "_num", 0);
        spinButton.interactable = !IsRolling || speedUp;

        if (num == 0) lm.MaximizeLifebuoys();
        anglePerItem = 360f / sectors.Length;
    }

    public void Roll()
    {
        if (!IsRolling && lm.SubtractLifebuoy())
        {
            if (num < nums.Length)
            {
                section = nums[num++];
                island.SetParameter(rouletteName + "_num", num);
            }
            else
            {
                section = UnityEngine.Random.Range(0, sectorCount);
            }
            float maxAngle = 360f + 360f * time + ((section + 6) * anglePerItem) + (anglePerItem / 2f);
            StartCoroutine(Rolling(5 * time, maxAngle));
        }
        else if (IsRolling && !speedUp)
        {
            speedUp = true;
            float maxAngle = ((section + 6) * anglePerItem) + (anglePerItem / 2f);
            StopAllCoroutines();
            StartCoroutine(Rolling(time / 2f, maxAngle));
        }
    }

    public void OpenWheel()
    {
        if (levels.Contains(island.Level))
            wb.WheelSwitchOn();
    }

    private IEnumerator Rolling(float time, float maxAngle)
    {
        IsRolling = true;
        spinButton.interactable = !IsRolling || speedUp;
        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        while (timer < time)
        {
            float angle = maxAngle * animationCurve.Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        IsRolling = false;
        speedUp = false;
        Reward();
        spinButton.interactable = !IsRolling || speedUp;
    }

    private void Reward()
    {
        sectors[section].Reward();

        _flyingReward = Instantiate(flyingReward, transform.parent.transform);
        _flyingReward.GetComponent<FlyingWheelReward>().text.text = sectors[section].GetComponentInChildren<Text>().text;
        _flyingReward.GetComponent<FlyingWheelReward>().image.sprite = sectors[section].GetComponentInChildren<Image>().sprite;
        _flyingReward.GetComponent<FlyingWheelReward>().image.color = sectors[section].GetComponentInChildren<Image>().color;

        _rewardEffect = Instantiate(rewardEffect, arrow.transform);
    }
}
