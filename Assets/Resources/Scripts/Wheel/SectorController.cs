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

    private int reward = 0;
    private Island island;
    private Text title;

    private void Awake()
    {
        island = Island.Instance();
        title = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);
        UpdateInfo();
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        int level = Mathf.Clamp(island.Level - minLevel, 0, maxLevel);
        switch (type)
        {
            case RouletteRotation.RewardType.Money:
                reward = (int)Mathf.Pow(2, level / levelDifference) * startReward;
                title.text = reward.ToString();
                break;
            case RouletteRotation.RewardType.Bonus:
                reward = (level / levelDifference + 1) * startReward;
                title.text = "X" + reward.ToString();
                break;
            case RouletteRotation.RewardType.BlackMark:
                reward = 1;
                title.text = "X" + reward.ToString();
                break;
        }
        if (minLevel == 0)
            Debug.Log(name + " : " + reward);
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
