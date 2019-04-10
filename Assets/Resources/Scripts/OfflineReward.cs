using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OfflineReward : MonoBehaviour
{
    public GameObject window;
    public List<GameObject> shipsList;
    public IslandController[] islands;
    public int modifier, expModifier;
    public int maxTime;
    public int bonusModifier = 1;
    public BonusGenerator[] bgs;

    public static TimeSpan ts;

    private int timeModifier, expToAdd;
    private Island island;
    private Text text;
    private bool rewardGained;
    private BigDigit money;

    private void Awake()
    {
        island = Island.Instance();
        text = GetComponent<Text>();
        rewardGained = false;
        money = BigDigit.zero;
    }

    void Update()
    {
        if (!rewardGained)
        {
            island.InitParameter("QuitTime", (DateTime.Now).ToString());
            ts = DateTime.Now - DateTime.Parse(island.GetParameter("QuitTime", ""));

            //Debug.Log(ts);

            //Пересчитываем в секунды
            if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes < 10f)
            {
                rewardGained = true;
                window.SetActive(false);
            }
            else
            {
                timeModifier = Mathf.Clamp(ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24, 0, maxTime);
            }

            //Считаем бабки и левел-ап
            money = BigDigit.zero;
            expToAdd = 0;

            foreach (IslandController land in islands)
            {
                if (land.minLevel <= island.Level)
                    money += land.GetReward() * (timeModifier / modifier) + new BigDigit(100d);
            }

            foreach (GameObject ships in shipsList)
            {
                foreach (Transform child in ships.transform)
                {
                    Ship ship = child.GetComponent<Ship>();
                    expToAdd += ship.reward / (int)ship.raidTime * timeModifier / expModifier;
                }
            }

            //Добавлям бонусы
            for (int i = 0; i <= Mathf.Clamp(island.Level / 25, 0, bgs.Length - 1); i++)
                bgs[i].RandomBonus(timeModifier / bonusModifier);

            //Выдаем ЛЕВЕЛ-АП
            island.ExpUp(expToAdd);


            text.text = money.ToString();

            //Write Time for Offline Reward
            island.SetParameter("QuitTime", DateTime.Now.ToString());

            rewardGained = true;
            window.SetActive(!money.EqualsZero());
        }
    }

    public void AddOfflineReward()
    {
        island.ChangeMoney(money);
    }
}
