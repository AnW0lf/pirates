using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sector : MonoBehaviour
{
    public int index = -1;
    public LuckyWheelRewardType type;
    public BigDigit startReward;

    private Text txt;

    private void Awake()
    {
        if (index < 0) index = transform.GetSiblingIndex();

        txt = transform.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);

        UpdateInfo(new object[0]);
    }

    private void UpdateInfo(object[] arg0)
    {
        if (txt)
        {
            switch (type)
            {
                case LuckyWheelRewardType.Money:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                            txt.text = (startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]).ToString();
                        break;
                    }
                case LuckyWheelRewardType.Bonus:
                    {
                        txt.text = "X3";
                        break;
                    }
                case LuckyWheelRewardType.Polundra:
                    {
                        txt.text = "Polundra";
                        break;
                    }
                case LuckyWheelRewardType.Premium:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                            txt.text = (startReward * LuckyWheel.Instance.modifiers[Island.Instance.Level]).ToString();
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
