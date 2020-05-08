using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorController : MonoBehaviour
{
    public RouletteRotation.RewardType type;
    public int minLevel, maxLevel, levelDifference, startReward, bonusId;
    public BonusGenerator bg;
    public ShipCtrl blackShip;
    public Text title;

    public float startRewardMantissa;
    public int startRewardExponent;

    private int reward = 0, modifier;
    private Island island;
    private float mod = 1f;

    private BigDigit startMoneyReward, moneyReward;

    private void Awake()
    {
        island = Island.Instance();
        reward = startReward;
    }

    private void OnEnable()
    {
        startMoneyReward = new BigDigit(startRewardMantissa, startRewardExponent);
        moneyReward = startMoneyReward * mod;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        switch (type)
        {
            case RouletteRotation.RewardType.Money:
                title.text = moneyReward.ToString();
                break;
            case RouletteRotation.RewardType.Bonus:
                title.text = "X" + reward.ToString();
                break;
            case RouletteRotation.RewardType.BlackMark:
                title.text = "X" + reward.ToString();
                break;
        }
    }

    public void UpdateReward(float[] modifiers)
    {
        if(type == RouletteRotation.RewardType.Money)
        {
            mod = 1f;
            foreach(float m in modifiers)
            {
                mod *= m;
            }
            startMoneyReward = new BigDigit(startRewardMantissa, startRewardExponent);
            moneyReward = startMoneyReward * mod;
        }
        UpdateInfo();
    }

    public void Reward()
    {
        switch (type)
        {
            case RouletteRotation.RewardType.Money:
                island.ChangeMoney(moneyReward);
                break;
            case RouletteRotation.RewardType.Bonus:
                bg.Bonus(bonusId, reward);
                break;
            case RouletteRotation.RewardType.BlackMark:
                blackShip.AddBlackMark(1);
                break;
        }
    }
}
