using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LifebuoyManager : MonoBehaviour
{
    public GlobalUpgradeButton upgrade;
    [Header("Текст со счётчиком")]
    public TextManager tm;
    [Header("Полоска наполнения")]
    public Image bar;
    [Header("Таймер")]
    public TextManager timer;
    [Header("Время востановления")]
    public int seconds;

    public GameObject flag, spinButton, wheelButton;

    private int max, cur, lvl, s, lifebuoysOffline;
    private bool isTimer = false;


    private Island island;
    private string modifierName;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        modifierName = upgrade.modifierName;
        island.InitParameter(modifierName + "_level", 1);
        island.InitParameter(modifierName + "_current", 3);
        island.InitParameter(modifierName + "_timer", 0);
        lvl = island.GetParameter(modifierName + "_level", 0);
        max = (int)upgrade.startReward + (lvl - 1) * (int)upgrade.modifier;
        cur = island.GetParameter(modifierName + "_current", 0);
        s = island.GetParameter(modifierName + "_timer", 0);

        MaximizeLifebuoys();

        LifebuoysOffline();

        UpdateInfo();
        if (s != 0) StartCoroutine(Timer(s));

        EventManager.Subscribe("LevelUp", MaximizeLifebuoys);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("LevelUp", MaximizeLifebuoys);
    }

    private void MaximizeLifebuoys(object[] arg0)
    {
        MaximizeLifebuoys();
    }

    private void Update()
    {
        if (cur < max && !isTimer)
        {
            StartCoroutine(Timer(seconds));
        }
        if (cur == max && isTimer)
        {
            StopAllCoroutines();
            isTimer = false;
        }
        if (cur >= max)
        {
            if (timer.gameObject.activeInHierarchy)
            {
                timer.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!timer.gameObject.activeInHierarchy)
            {
                timer.gameObject.SetActive(true);
            }
        }

        // FLAG AND SPIN BUTTON
        if (cur > 0)
        {
            if (!spinButton.GetComponent<Button>().interactable)
            {
                spinButton.GetComponent<Button>().interactable = true;
            }

            if (wheelButton.GetComponent<Image>().color != new Color(1f, 1f, 1f))
            {
                wheelButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }

            if (!flag.activeInHierarchy)
            {
                flag.SetActive(true);
            }
        }
        else
        {
            if (spinButton.GetComponent<Button>().interactable)
            {
                spinButton.GetComponent<Button>().interactable = false;
            }

            if (wheelButton.GetComponent<Image>().color == new Color(1f, 1f, 1f))
            {
                wheelButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }

            if (flag.activeInHierarchy)
            {
                flag.SetActive(false);
            }
        }
    }

    public void UpdateInfo()
    {
        modifierName = upgrade.modifierName;
        island.InitParameter(modifierName + "_level", 0);
        lvl = island.GetParameter(modifierName + "_level", 0);
        max = (int)upgrade.startReward + (lvl - 1) * (int)upgrade.modifier;
        cur = island.GetParameter(modifierName + "_current", 0);
        bar.fillAmount = (float)cur / max;
        tm.text = cur + "/" + max;
        flag.GetComponentInChildren<Text>().text = cur.ToString();
    }

    public void AddLifebuoy()
    {
        island.SetParameter(modifierName + "_current", ++cur);
        UpdateInfo();
    }

    public bool SubtractLifebuoy()
    {
        if(cur > 0)
        {
            island.SetParameter(modifierName + "_current", --cur);
            UpdateInfo();
            return true;
        }
        return false;
    }

    public void MaximizeLifebuoys()
    {
        if (cur < max)
        {
            island.SetParameter(modifierName + "_current", max);
        }
        UpdateInfo();
    }

    private IEnumerator Timer(int seconds)
    {
        isTimer = true;
        WaitForSeconds wait = new WaitForSeconds(1);
        for (s = seconds; s > 0; --s)
        {
            timer.prefix = "+1 in " + (s / 60).ToString();
            if (s % 60 < 10)
            {
                island.SetParameter(modifierName + "_timer", s);
                timer.postfix = "0" + (s % 60).ToString();
            }
            else
                timer.postfix = (s % 60).ToString();
            yield return wait;
        }
        AddLifebuoy();
        isTimer = false;
    }

    private void LifebuoysOffline()
    {
        if (PlayerPrefs.HasKey("QuitTime"))
        {
            OfflineReward.ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("QuitTime"));
        }
        else
        {
            OfflineReward.ts = DateTime.Now - DateTime.Now;
        }
        int timeModifier = (((int)OfflineReward.ts.Seconds)) + (((int)OfflineReward.ts.Minutes) * 60) + (((int)OfflineReward.ts.Hours) * 60 * 60) + (((int)OfflineReward.ts.Days) * 60 * 60 * 24);

        lifebuoysOffline = timeModifier / seconds;

        if (lifebuoysOffline > 0)
        {
            for (int i = 0; i < lifebuoysOffline; i++)
            {
                if (cur < max)
                {
                    AddLifebuoy();
                }
                else
                {
                    break;
                }
            }
        }
    }


    private void OnApplicationPause(bool pause)
    {
        island.SetParameter(modifierName + "_timer", s);
    }

    private void OnApplicationFocus(bool focus)
    {
        island.SetParameter(modifierName + "_timer", s);
    }

    private void OnApplicationQuit()
    {
        island.SetParameter(modifierName + "_timer", s);
    }
}
