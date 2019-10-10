using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OfflineReward : MonoBehaviour
{
    public int maxTime;
    public BigDigit rewardDivisor, expDivisor;
    public float freeShipDivisor;
    public GameObject window;
    public FreeShipController fsController;
    public IslandController[] islands;

    public static TimeSpan ts;

    private int timeModifier;
    private Island island;
    private Text text;
    private bool rewardGained;
    private BigDigit reward, exp;
    private Inventory inventory;

    private void Awake()
    {
        island = Island.Instance;
        text = GetComponent<Text>();
        rewardGained = false;
        reward = BigDigit.zero;
    }

    private void Start()
    {
        StartCoroutine(Reward());
    }

    private IEnumerator Reward()
    {
        if (rewardGained) yield break;
        yield return new WaitForFixedUpdate();
        island.InitParameter("QuitTime", (DateTime.Now).ToString());
        ts = DateTime.Now - DateTime.Parse(island.GetParameter("QuitTime", ""));

        if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes < 15f)
        {
            rewardGained = true;
            window.SetActive(false);
            yield break;
        }
        else
        {
            timeModifier = Mathf.Clamp(ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24, 0, maxTime);
        }

        reward = BigDigit.zero;
        exp = BigDigit.zero;

        foreach (IslandController land in islands)
        {
            if (land.minLevel <= island.Level)
                reward += land.GetReward() * (timeModifier) + new BigDigit(100d);
        }

        reward /= rewardDivisor;

        inventory = Inventory.Instance;
        foreach (Panel panel in inventory.panels)
        {
            foreach (ShipInfo item in panel.items)
            {
                exp += item.reward / item.raidTime * timeModifier;
            }
        }

        exp /= expDivisor;

        island.ChangeMoney(reward);
        island.ExpUp(exp);

        int count = Mathf.Clamp((int)(timeModifier / fsController.delay / freeShipDivisor), 0, 10);
        for(int i = 0; i < count; i++)
        {
            fsController.AddShip();
        }

        text.text = reward.ToString();

        island.SetParameter("QuitTime", DateTime.Now.ToString());
        rewardGained = true;
        window.SetActive(!reward.EqualsZero);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            StartCoroutine(Reward());
        }
        else
        {
            rewardGained = false;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            island.SetParameter("QuitTime", DateTime.Now.ToString());
        }
        else
        {
            rewardGained = false;
        }
    }

    private void OnApplicationQuit()
    {
        island.SetParameter("QuitTime", DateTime.Now.ToString());
    }

    public void AddOfflineReward(float modifier)
    {
        island.ChangeMoney(reward * Mathf.Abs(modifier));
    }
}
