using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteRotation : MonoBehaviour
{
    public bool direction;
    public float speed, rotationTime;
    public bool IsRolling { get; private set; }
    public LifebuoyManager lm;
    public BonusGenerator bg;

    [Header("Количество секторов")]
    public int sectorCount;

    [Header("Награда")]
    public float[] rewardValue;
    public RewardType[] rewardType;

    private RectTransform rect;
    private int section;
    private Island island;

    public enum RewardType { Money, Bonus, BlackMark};

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        IsRolling = false;
        island = Island.Instance();
    }

    public void Roll(int sectionNumber)
    {
        if (IsRolling)
        {
            return;
        }
        else
        {
            if (!lm.SubtractLifebuoy())
            {
                return;
            }
            else
            {
                section = sectionNumber;
                float angle = Mathf.Abs(sectionNumber * (360f / sectorCount) + ((180f / sectorCount))) % 360f;
                StartCoroutine(Rolling(angle));
            }
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
                bg.Bonus((int)rewardValue[section]);
                break;
            case RewardType.BlackMark:
                break;
        }
    }
}
