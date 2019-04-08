using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReward : MonoBehaviour
{
    public List<GameObject> shipsList;
    public List<PiersUpgrade> piersList;
    public float modifier;
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
        foreach (GameObject ships in shipsList)
        {
            foreach (Transform child in ships.transform)
            {
                money += new BigDigit(child.GetComponent<Ship>().reward / child.GetComponent<Ship>().raidTime * modifier * PlayerPrefs.GetInt("Level"));
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
