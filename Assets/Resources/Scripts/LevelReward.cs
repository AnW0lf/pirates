using System.Collections;
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
        money = BigDigit.zero;
        //foreach (GameObject ships in shipsList)
        //{
        //    foreach (Transform child in ships.transform)
        //    { 
        //        if (PlayerPrefs.GetInt("Level") < 10)
        //        {
        //            money += new BigDigit(child.GetComponent<Ship>().reward / child.GetComponent<Ship>().raidTime * modifier * PlayerPrefs.GetInt("Level"));
        //        }
        //        else
        //        {
        //            money += new BigDigit(child.GetComponent<Ship>().reward / child.GetComponent<Ship>().raidTime * modifier * (int)(Mathf.Pow(powModifier, PlayerPrefs.GetInt("Level"))));
        //        }
        //    }
        //}

        foreach (IslandController island in islandsList)
        {
            if (island.minLevel <= this.island.Level)
                money += (island.GetReward() * this.island.Level * 13f);
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
