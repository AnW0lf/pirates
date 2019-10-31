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
    public Text rewardText;
    public GameObject back, rays;
    public FreeShipController fsController;
    public IslandController[] islands;

    public static TimeSpan ts;

    private int timeModifier, freeShipCount;
    private bool rewardGained;
    private BigDigit reward, exp;
    private Inventory inventory;
    private Image bg;

    private void Awake()
    {
        rewardGained = false;
        reward = BigDigit.zero;
        bg = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Reward());
    }

    private IEnumerator Reward()
    {
        if (rewardGained) yield break;
        yield return new WaitForFixedUpdate();

        Island.Instance.InitParameter("QuitTime", (DateTime.Now).ToString());
        ts = DateTime.Now - DateTime.Parse(Island.Instance.GetParameter("QuitTime", ""));

        if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes < 15f)
        {
            rewardGained = true;
            bg.enabled = false;
            back.SetActive(false);
            rays.SetActive(false);
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            bg.enabled = true;
            back.SetActive(true);
            rays.SetActive(true);
            timeModifier = Mathf.Clamp(ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 60 * 60 * 24, 0, maxTime);
        }

        reward = BigDigit.zero;
        exp = BigDigit.zero;

        foreach (IslandController land in islands)
        {
            if (land.minLevel <= Island.Instance.Level)
                reward += land.GetReward() * (timeModifier) + new BigDigit(100d);
        }

        reward /= rewardDivisor;

        inventory = Inventory.Instance;
        foreach (Panel panel in inventory.panels)
        {
            foreach (ShipInfo item in panel.items)
            {
                if (item)
                {
                    exp += item.reward / item.raidTime * timeModifier;
                }
            }
        }

        exp /= expDivisor;

        freeShipCount = Mathf.Clamp((int)(timeModifier / fsController.delay / freeShipDivisor), 0, 10);

        rewardText.text = reward.ToString();

        Island.Instance.SetParameter("QuitTime", DateTime.Now.ToString());
        rewardGained = true;
        back.SetActive(!reward.EqualsZero);
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
            Island.Instance.SetParameter("QuitTime", DateTime.Now.ToString());
        }
        else
        {
            rewardGained = false;
        }
    }

    private void OnApplicationQuit()
    {
        Island.Instance.SetParameter("QuitTime", DateTime.Now.ToString());
    }

    public void AddOfflineReward(float modifier)
    {
        Island.Instance.ChangeMoney(reward * Mathf.Abs(modifier));
        Island.Instance.ExpUp(exp * Mathf.Abs(modifier));
        for (int i = 0; i < freeShipCount; i++)
        {
            fsController.AddShip();
        }
        bg.enabled = false;
        back.SetActive(false);
        rays.SetActive(false);
    }
}
