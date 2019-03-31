using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    public string planeName;
    public bool IsRolling = false, direction = false;
    public float rotationTime, speed;
    public int sectorCount = 10;
    public int[] sectors;
    public float[] rewardValue;
    public RewardType[] rewardType;

    private Global global;
    private RectTransform rect;
    private int section, curSection;

    public enum RewardType { MONEY, BONUS, BLACK };

    private void Awake()
    {
        IsRolling = false;
        global = Global.Instance;
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        global.InitParameter(planeName + "WheelCurentSector", 0);
        curSection = global.GetParameter(planeName + "WheelCurentSector", 0);
    }

    public void Roll()
    {
        if (!IsRolling)
        {
            float angle;
            if (sectors.Length > curSection)
            {
                section = sectors[curSection];
                angle = Mathf.Abs(section * (360f / sectorCount) + ((180f / sectorCount))) % 360f;
                global.SetParameter(planeName + "WheelCurentSector", ++curSection);
            }
            else
            {
                section = UnityEngine.Random.Range(0, 10);
                angle = Mathf.Abs(section * (360f / sectorCount) + ((180f / sectorCount))) % 360f;
            }
            StartCoroutine(Rolling(angle));
        }
    }

    private IEnumerator Rolling(float angle)
    {
        float curRotationTime = UnityEngine.Random.Range(0.8f * rotationTime, 1.2f * rotationTime);
        IsRolling = true;
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
    }

    private void Reward()
    {
        if (rewardType.Length <= section || rewardValue.Length <= section) return;
        Debug.Log(section + " : " + rewardType[section]);
        switch (rewardType[section])
        {
            case RewardType.MONEY:
                global.ChangeMoney((int)rewardValue[section]);
                EventManager.SendEvent("ChangeMoney");
                break;
            case RewardType.BONUS:
                if ((int)rewardValue[section] == 1)
                {
                    EventManager.SendEvent(planeName + "GenerateBonus", Bonus.BonusType.MATERIAL, 3);
                }
                else if ((int)rewardValue[section] == 2)
                {
                    EventManager.SendEvent(planeName + "GenerateBonus", Bonus.BonusType.DELAY, 3);
                }
                else if ((int)rewardValue[section] == 3)
                {
                    EventManager.SendEvent(planeName + "GenerateBonus", Bonus.BonusType.SPIN, 3);
                }
                break;
            case RewardType.BLACK:
                //blackShip.OpenMenu();
                break;
        }
    }
}
