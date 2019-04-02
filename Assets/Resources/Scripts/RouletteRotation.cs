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
    public LifebuoyManager lm;
    public BonusGenerator bg;
    public GameObject flyingReward, arrow;
    public PierManager blackShip;

    [Header("Количество секторов")]
    public int sectorCount;

    [Header("Награда")]
    public GameObject[] rewardObjects;
    public float[] rewardValue;
    public RewardType[] rewardType;
    public int[] nums;
    public GameObject spinButton;

    private RectTransform rect;
    private int section, num;
    private Island island;
    private GameObject _flyingReward;

    public enum RewardType { Money, Bonus, BlackMark };

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        IsRolling = false;
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter(rouletteName + "_num", 0);
        num = island.GetParameter(rouletteName + "_num", 0);
    }

    private void Update()
    {
        if (IsRolling)
        {
            if (spinButton.GetComponent<Button>().interactable)
                spinButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (!spinButton.GetComponent<Button>().interactable)
                spinButton.GetComponent<Button>().interactable = true;
        }
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
                section = Random.Range(0, sectorCount);
            }
            float angle = Mathf.Abs(section * (360f / sectorCount) + ((180f / sectorCount))) % 360f;
            StartCoroutine(Rolling(angle));
        }
    }

    private IEnumerator Rolling(float angle)
    {
        float curRotationTime = Random.Range(0.8f * rotationTime, 1.2f * rotationTime);
        IsRolling = true;
        float a = .0f;
        Vector3 direction = Vector3.back * (this.direction ? -1f : 1f);
        while (a < curRotationTime)
        {
            rect.Rotate(direction, speed * Time.deltaTime);
            a += Time.deltaTime;
            yield return null;
        }
        float r = Random.Range(180f, 360f);
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
    }

    private void Reward()
    {
        if (rewardType.Length <= section || rewardValue.Length <= section) return;
        switch (rewardType[section])
        {
            case RewardType.Money:
                island.ChangeMoney((int)rewardValue[section]);
                break;
            case RewardType.Bonus:
                bg.Bonus((int)rewardValue[section], 3);
                break;
            case RewardType.BlackMark:
                //blackShip.OpenMenu();
                break;
        }

        _flyingReward = Instantiate(flyingReward, transform.parent.transform);
        _flyingReward.GetComponent<FlyingWheelReward>().text.text = rewardObjects[section].GetComponentInChildren<Text>().text;
        _flyingReward.GetComponent<FlyingWheelReward>().image.sprite = rewardObjects[section].GetComponentInChildren<Image>().sprite;
        _flyingReward.GetComponent<FlyingWheelReward>().image.color = rewardObjects[section].GetComponentInChildren<Image>().color;
    }
}
