using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteRotation : MonoBehaviour
{
    public string rouletteName;
    public bool direction;
    public float speed, rotationTime;
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
    public GameObject spinButton;

    private RectTransform rect;
    private int section, num;
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
        spinButton.GetComponent<Button>().interactable = !IsRolling;

        if (num == 0) lm.MaximizeLifebuoys();
    }

    public void Roll()
    {
        if (IsRolling || !lm.SubtractLifebuoy())
        {
            return;
        }
        else
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
            float angle = Mathf.Abs(section * (360f / sectorCount) + ((180f / sectorCount))) % 360f;
            StartCoroutine(Rolling(angle));
        }
    }

    public void OpenWheel()
    {
        if (levels.Contains(island.Level))
            wb.WheelSwitchOn();
    }

    private IEnumerator Rolling(float angle)
    {
        float curRotationTime = UnityEngine.Random.Range(0.8f * rotationTime, 1.2f * rotationTime);
        IsRolling = true;
        spinButton.GetComponent<Button>().interactable = !IsRolling;
        float a = .0f;
        Vector3 direction = Vector3.back * (this.direction ? -1f : 1f);
        while (a < curRotationTime)
        {
            rect.Rotate(direction, speed * Time.deltaTime);
            a += Time.deltaTime;
            yield return null;
        }
        float r = UnityEngine.Random.Range(180f, 360f);
        a = angle - r < 0f ? angle - r + 360f : angle - r;
        while (Mathf.Abs(rect.localEulerAngles.z - a) >= 4f)
        {
            rect.Rotate(direction, speed * Time.deltaTime);
            yield return null;
        }
        a = speed * speed / (2f * r);
        float stopSpeed = speed;
        while (Mathf.Abs(rect.localEulerAngles.z - angle) >= 1f && stopSpeed > 0f)
        {
            rect.Rotate(direction, stopSpeed * Time.deltaTime);
            stopSpeed -= a * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        IsRolling = false;
        Reward();
        spinButton.GetComponent<Button>().interactable = !IsRolling;
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
