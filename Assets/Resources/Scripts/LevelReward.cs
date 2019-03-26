using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelReward : MonoBehaviour
{
    public List<GameObject> shipsList;
    public float modifier;

    private int money;
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

        GetComponent<Text>().text = money.ToString();
    }

    public void AddLevelReward()
    {
        island.ChangeMoney(money);
    }
}
