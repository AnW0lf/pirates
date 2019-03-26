using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject flag, spinButton;

    private int max, cur, lvl, s;
    private bool isTimer = false;


    private Island island;
    private string modifierName;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        modifierName = upgrade.modifierName;
        island.InitParameter(modifierName + "_level", 1);
        island.InitParameter(modifierName + "_current", 0);
        island.InitParameter(modifierName + "_timer", 0);
        lvl = island.GetParameter(modifierName + "_level", 0);
        max = (int)upgrade.startReward + (lvl - 1) * (int)upgrade.modifier;
        cur = island.GetParameter(modifierName + "_current", 0);
        s = island.GetParameter(modifierName + "_timer", 0);
        UpdateInfo();
        if (s != 0) StartCoroutine(Timer(s));
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

            if (flag.activeInHierarchy)
            {
                flag.SetActive(false);
            }
        }
    }

    private void UpdateInfo()
    {
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
