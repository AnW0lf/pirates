using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierManager : MonoBehaviour
{
    public string label;
    public float raidTime;
    public int reward;
    public Ship ship;
    public GameObject bacon;

    private int bodyLvl = 0, sailLvl = 0, gunLvl = 0;
    private Island island;

    [Header("Элементы UpgradeMenu")]
    public UpgradeMenuManager upgradeMenuManager;

    [Header("Параметры улучшения")]
    public int startCost = 10;
    public int costIncrease = 10;
    public float rewardIncrease = 0.2f;
    public float raidTimeReduce = 0.1f;
    public int maxLvl = 10;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        UpdateShip();
    }

    private void Update()
    {
        if (island.GetMoney() < GetUpgradeCost())
        {
            bacon.SetActive(false);
        }
        else
        {
            bacon.SetActive(true);
        }
    }

    public void OpenMenu()
    {
        upgradeMenuManager.GenerateMenu(this);
    }

    private void UpdateShip()
    {
        ship.raidTime = GetRaidTime(sailLvl);
        ship.reward = GetReward(bodyLvl, gunLvl);
    }

    private void OnMouseUpAsButton()
    {
        OpenMenu();
    }

    public int GetRaidTime(int sail)
    {
        float raidTimeMultiplier = 1f - sail * raidTimeReduce;
        return (int)(raidTime * (raidTimeMultiplier < 0 ? 0 : raidTimeMultiplier) + 0.5f);
    }

    public int GetReward(int body, int gun)
    {
        float rewardMultiplier = 1f + (body + gun) * rewardIncrease;
        return (int)(reward * rewardMultiplier + 0.5f);
    }

    public int GetUpgradeCost()
    {
        return startCost + costIncrease * (bodyLvl + sailLvl + gunLvl);
    }

    public int GetMaxLevel()
    {
        return maxLvl;
    }

    public int GetBodyLevel()
    {
        return bodyLvl;
    }

    public int GetSailLevel()
    {
        return sailLvl;
    }

    public int GetGunLevel()
    {
        return gunLvl;
    }

    public void Upgrade()
    {
        if(bodyLvl < maxLvl)
        {
            if (island.ChangeMoney(-GetUpgradeCost()))
            {
                bodyLvl++;
            }
            UpdateShip();
        }
        else if (sailLvl < maxLvl)
        {
            if (island.ChangeMoney(-GetUpgradeCost()))
            {
                sailLvl++;
            }
            UpdateShip();
        }
        else if (gunLvl < maxLvl)
        {
            if (island.ChangeMoney(-GetUpgradeCost()))
            {
                gunLvl++;
            }
            UpdateShip();
        }
    }
}
