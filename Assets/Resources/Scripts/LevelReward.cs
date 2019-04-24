﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReward : MonoBehaviour
{
    public List<GameObject> shipsList;
    public List<PiersUpgrade> piersList;
    public List<IslandController> islandsList;
    public float modifier, powModifier;
    public Text levelsToShipsText;
    public string oneLevelToShip, someLevelsToShip, noLevelsToShip;

    private int levelsToShip;
    private Island island;
    private BigDigit money;

    private void Awake()
    {
        island = Island.Instance();
    }

    void OnEnable()
    {
        // MoneyReward
        money = BigDigit.zero;

        if (island.Level <= 25)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * island.Level * 13f);
            }
        }
        else if (island.Level > 25 && island.Level <= 50)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * island.Level * 13f * (island.Level - 25));
            }
        }
        else if (island.Level > 50 && island.Level <= 75)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * island.Level * 13f * (island.Level - 50));
            }
        }
        else
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.GetReward() * island.Level * 13f * (island.Level - 75));
            }
        }



        levelsToShip = 999;
        foreach (PiersUpgrade piers in piersList)
        {
            foreach (PierManager pier in piers.piers)
            {
                if (island.Level + 1 < pier.minLvl)
                {
                    if ((pier.minLvl - (island.Level + 1)) < levelsToShip)
                    {
                        levelsToShip = pier.minLvl - (island.Level + 1);
                    }
                }
            }
        }

        if (levelsToShip > 0)
        {
            if (levelsToShip == 1)
            {
                levelsToShipsText.text = levelsToShip + " " + oneLevelToShip;
            }
            else
            {
                levelsToShipsText.text = levelsToShip + " " + someLevelsToShip;
            }
        }
        else
        {
            levelsToShipsText.text = noLevelsToShip;
        }

        GetComponent<Text>().text = money.ToString();
    }

    public void AddLevelReward()
    {
        island.ChangeMoney(money);
    }
}
