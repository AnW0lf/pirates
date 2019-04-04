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
    public PierManager blackShip;

    private int reward = 0, modifier;
    private Island island;
    private Text title;

    private void Awake()
    {
        island = Island.Instance();
        title = GetComponentInChildren<Text>();
        reward = startReward;
    }

    private void OnEnable()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        switch (type)
        {
            case RouletteRotation.RewardType.Money:
                title.text = reward.ToString();
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
            float mod = 1f;
            foreach(float m in modifiers)
            {
                mod *= m;
            }
            reward = (int)(startReward * mod);
        }
        UpdateInfo();
    }

    public void Reward()
    {
        switch (type)
        {
            case RouletteRotation.RewardType.Money:
                island.ChangeMoney(reward);
                break;
            case RouletteRotation.RewardType.Bonus:
                bg.Bonus(bonusId, reward);
                break;
            case RouletteRotation.RewardType.BlackMark:
                blackShip.ChangeBlackMark(1);
                break;
        }
    }
}
