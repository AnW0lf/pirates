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

    private void Awake()
    {
        island = Island.Instance();
    }

    void OnEnable()
    {
        if (PlayerPrefs.HasKey("QuitTime"))
        {
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("QuitTime"));
            Debug.Log(ts.Hours + " " + ts.Minutes + " " + ts.Seconds + " ");
        }
        else
        {
            ts = DateTime.Now - DateTime.Now;
        }

        //Пересчитываем в секунды
        if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes < 10f)
        {
            window.SetActive(false);
        }
        else
        {

            timeModifier = (((int)ts.Seconds)) + (((int)ts.Minutes) * 60) + (((int)ts.Hours) * 60 * 60) + (((int)ts.Days) * 60 * 60 * 24);
            if (timeModifier > maxTime)
            {
                timeModifier = maxTime;
            }
        }

        //Считаем бабки и левел-ап
        money = 0;
        expToAdd = 0;
        foreach (GameObject ships in shipsList)
        {
            foreach (Transform child in ships.transform)
            {
                money += (int)(child.GetComponent<Ship>().reward / child.GetComponent<Ship>().raidTime * timeModifier / modifier) + 100;
                expToAdd += (int)(timeModifier / child.GetComponent<Ship>().raidTime / 10f);
            }
        }

        //Выдаем ЛЕВЕЛ-АП
        island.ExpUp(expToAdd);


        GetComponent<Text>().text = money.ToString();

        //Write Time for Offline Reward
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToString());

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
