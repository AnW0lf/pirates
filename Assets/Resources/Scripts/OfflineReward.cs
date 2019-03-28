using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OfflineReward : MonoBehaviour
{
    public GameObject window;
    public List<GameObject> shipsList;
    public float modifier;
    public int maxTime;

    public static TimeSpan ts;

    private int money, timeModifier, expToAdd;
    private Island island;
    private Text text;

    private void Awake()
    {
        island = Island.Instance();
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        island.InitParameter("QuitTime", (DateTime.Now - DateTime.Now).ToString());
        ts = DateTime.Now - DateTime.Parse(island.GetParameter("QuitTime", ""));

        //Пересчитываем в секунды
        if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes < 10f)
        {
            window.SetActive(false);
        }
        else
        {
            timeModifier = Mathf.Clamp(ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24, 0, maxTime);
        }

        //Считаем бабки и левел-ап
        money = 0;
        expToAdd = 0;
        foreach (GameObject ships in shipsList)
        {
            foreach (Transform child in ships.transform)
            {
                Ship ship = child.GetComponent<Ship>();
                money += (int)(ship.reward / ship.raidTime * timeModifier / modifier) + 100;
                expToAdd += (int)(timeModifier / ship.raidTime / 10f);
            }
        }

        //Выдаем ЛЕВЕЛ-АП
        island.ExpUp(expToAdd);


        text.text = money.ToString();

        //Write Time for Offline Reward
        island.SetParameter("QuitTime", DateTime.Now.ToString());

        if (money == 0)
        {
            window.SetActive(false);
        }
    }

    public void AddOfflineReward()
    {
        island.ChangeMoney(money);
    }
}
