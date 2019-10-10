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
            int index;
            switch (type)
            {
                case LuckyWheelRewardType.Money:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                        {
                            index = 0;

                            for (; index < LuckyWheel.Instance.levels.Count
                                && index < LuckyWheel.Instance.modifiers.Count
                                && LuckyWheel.Instance.levels[index] <= Island.Instance.Level; index++) ;
                            index--;
                            index = Mathf.Clamp(index, 0, LuckyWheel.Instance.modifiers.Count - 1);

                            txt.text = (startReward * LuckyWheel.Instance.modifiers[index]).ToString();
                        }
                        break;
                    }
                case LuckyWheelRewardType.Bonus:
                    {
                        //txt.text = "X1";
                        break;
                    }
                case LuckyWheelRewardType.Polundra:
                    {
                        txt.text = "Bonus Rain";
                        break;
                    }
                case LuckyWheelRewardType.Premium:
                    {
                        if (LuckyWheel.Instance && Island.Instance)
                        {
                            index = 0;

                            for (; index < LuckyWheel.Instance.levels.Count
                                && index < LuckyWheel.Instance.modifiers.Count
                                && LuckyWheel.Instance.levels[index] <= Island.Instance.Level; index++) ;
                            index--;
                            index = Mathf.Clamp(index, 0, LuckyWheel.Instance.modifiers.Count - 1);

                            txt.text = (startReward * LuckyWheel.Instance.modifiers[index]).ToString();
                        }
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
        int index;
        switch (type)
        {
            case LuckyWheelRewardType.Money:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                    {
                        index = 0;

                        for (; index < LuckyWheel.Instance.levels.Count
                            && index < LuckyWheel.Instance.modifiers.Count
                            && LuckyWheel.Instance.levels[index] <= Island.Instance.Level; index++) ;
                        index--;
                        index = Mathf.Clamp(index, 0, LuckyWheel.Instance.modifiers.Count - 1);

                        Island.Instance.ChangeMoney(startReward * LuckyWheel.Instance.modifiers[index]);
                    }
                    break;
                }
            case LuckyWheelRewardType.Bonus:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                    {
                        LuckyWheel.Instance.bgs[Mathf.Clamp(Island.Instance.Level / 25, 0, LuckyWheel.Instance.bgs.Length - 1)].Bonus((int)startReward.exponent, 1);
                    }
                    break;
                }
            case LuckyWheelRewardType.Polundra:
                {
                    if (Polundra.Instance)
                    {
                        Polundra.Instance.BeginPolundra();
                    }
                    break;
                }
            case LuckyWheelRewardType.Premium:
                {
                    if (LuckyWheel.Instance && Island.Instance)
                    {
                        index = 0;

                        for (; index < LuckyWheel.Instance.levels.Count
                            && index < LuckyWheel.Instance.modifiers.Count
                            && LuckyWheel.Instance.levels[index] <= Island.Instance.Level; index++) ;
                        index--;
                        index = Mathf.Clamp(index, 0, LuckyWheel.Instance.modifiers.Count - 1);

                        Island.Instance.ChangePremium(startReward * LuckyWheel.Instance.modifiers[index]);
                    }
                    break;
                }
            default:
                Debug.LogWarning("Unknown LuckyWheelRewardType value " + type.ToString());
                break;
        }
    }
}
