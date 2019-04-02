using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReward : MonoBehaviour
{
    public List<GameObject> shipsList;
    public List<GameObject> piersList;
    public float modifier;
    public Text levelsToShipsText;
    public string oneLevelToShip, someLevelsToShip, noLevelsToShip;

    private int money, levelsToShip;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    void OnEnable()
    {
        money = 0;
        foreach (GameObject ships in shipsList)
        {
            foreach (Transform child in ships.transform)
            {
                money += (int)(child.GetComponent<Ship>().reward / child.GetComponent<Ship>().raidTime * modifier * PlayerPrefs.GetInt("Level"));
            }
        }

        levelsToShip = 999;
        foreach (GameObject piers in piersList)
        {
            foreach (Transform child in piers.transform)
            {
                if (island.Level + 1 < child.GetComponent<PierManager>().minLvl)
                {
                    if ((child.GetComponent<PierManager>().minLvl - (PlayerPrefs.GetInt("Level") + 1)) < levelsToShip)
                    {
                        levelsToShip = child.GetComponent<PierManager>().minLvl - (PlayerPrefs.GetInt("Level") + 1);
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
