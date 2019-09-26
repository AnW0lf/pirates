using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sector : MonoBehaviour
{
    public int index = -1;
    public Text title;
    public LuckyWheelRewardType type;
    public BigDigit startReward;

    private void Awake()
    {
        if (index < 0) index = transform.GetSiblingIndex();
    }

    private void Start()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void UpdateInfo(object[] arg0)
    {
        if (title)
        {
            switch (type)
            {
                case LuckyWheelRewardType.Money:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                            title.text = (startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]).ToString();
                        break;
                    }
                case LuckyWheelRewardType.Bonus:
                    {
                        title.text = "X3";
                        break;
                    }
                case LuckyWheelRewardType.Polundra:
                    {
                        title.text = "Polundra";
                        break;
                    }
                case LuckyWheelRewardType.Premium:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                            title.text = (startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]).ToString();
                        break;
                    }
                default:
                    Debug.LogWarning("Unknown LuckyWheelRewardType value " + type.ToString());
                    break;
            }
        }
    }

    public void Reward()
    {
        switch (type)
        {
            case LuckyWheelRewardType.Money:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                        Island.Instance.ChangeMoney(startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]);
                    break;
                }
            case LuckyWheelRewardType.Bonus:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                        LuckyWheel.Instance.bgs[Mathf.Clamp(Island.Instance.Level / 25, 0, LuckyWheel.Instance.bgs.Length - 1)].Bonus((int)startReward.exponent, 3);
                    break;
                }
            case LuckyWheelRewardType.Polundra:
                {
                    if (PolundraTimer.Instance)
                        PolundraTimer.Instance.BeginPolundra();
                    break;
                }
            case LuckyWheelRewardType.Premium:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                        Island.Instance.ChangePremium(startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]);
                    break;
                }
            default:
                Debug.LogWarning("Unknown LuckyWheelRewardType value " + type.ToString());
                break;
        }
    }
}
