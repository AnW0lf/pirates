using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShipWindow : MonoBehaviour
{
    public GameObject background, locked;
    public Text title, reward, delay, cost, btnText, lockedLevel, shipLevel;
    public Button btn;
    public Image icon;

    private Global global;
    private Pier pier;

    private void Start()
    {
        global = Global.Instance;
        EventManager.Subscribe("OpenUpgradeMenu", Open);
    }

    private void Open(object[] arg0)
    {
        if (arg0.Length > 0)
        {
            background.SetActive(true);
            pier = (Pier)arg0[0];

            if (pier.unlockLevel > global.Level)
            {
                locked.SetActive(true);
                btnText.text = "Buy";
                lockedLevel.text = "Level " + pier.unlockLevel.ToString();
            }
            else
            {
                locked.SetActive(false);
                if (pier.level == 0)
                {
                    btnText.text = "Buy";
                }
                else
                {
                    btnText.text = "Upgrade";
                }
            }

            title.text = pier.title;
            reward.text = "Reward : " + CheckRange(pier.GetReward());
            delay.text = "Delay : " + CheckRange(pier.GetDelay());
            cost.text = "Cost : " + CheckRange(pier.GetUpgradeCost());
            icon.sprite = pier.GetSprite();
            shipLevel.text = "Level : " + pier.level + "/" + pier.maxLevel;
            EventManager.SendEvent("UpgradeMenuButtonUpdate", pier);
        }
    }

    private string CheckRange(float v)
    {
        float value = v;
        int degree = 0;
        while (value > 1000)
        {
            degree++;
            value /= 1000;
        }
        string str = value.ToString();
        str = str.Length >= 4 ? str.Substring(0, 4) : str;
        switch (degree)
        {
            case 0:
                return str;
            case 1:
                return str += "K";
            case 2:
                return str += "M";
            case 3:
                return str += "B";
            case 4:
                return str += "T";
            case 5:
                return str += "Q";
            default:
                return str += "A";
        }
    }

    private string CheckRange(int v)
    {
        float value = v;
        int degree = 0;
        while (value > 1000)
        {
            degree++;
            value /= 1000;
        }
        string str = value.ToString();
        str = str.Length >= 4 ? str.Substring(0, 4) : str;
        switch (degree)
        {
            case 0:
                return str;
            case 1:
                return str += "K";
            case 2:
                return str += "M";
            case 3:
                return str += "B";
            case 4:
                return str += "T";
            case 5:
                return str += "Q";
            default:
                return str += "A";
        }
    }
}
