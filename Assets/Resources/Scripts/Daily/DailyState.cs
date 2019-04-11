using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyState : MonoBehaviour
{
    public Text title;
    public Color disable, enable;
    public string enableText;
    public GameObject flag;

    private Island island;
    private bool claimed;
    private Image image;
    private Button button;
    private DateTime now;

    private int year, month, day;

    private void Awake()
    {
        island = Island.Instance();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        now = DateTime.Now;
        island.InitParameter("DailyYear", now.Year - 1);
        island.InitParameter("DailyMonth", now.Month);
        island.InitParameter("DailyDay", now.Day);
        year = island.GetParameter("DailyYear", 0);
        month = island.GetParameter("DailyMonth", 0);
        day = island.GetParameter("DailyDay", 0);
    }

    private void Update()
    {
        now = DateTime.Now;
        if(year == now.Year && month == now.Month && day == now.Day)
        {
            title.text = SecondsToTime(ToSeconds(24, 0, 0) - ToSeconds(now.Hour, now.Minute, now.Second));
            if (image.color != disable) image.color = disable;
            if (flag.activeInHierarchy) flag.SetActive(false);
        }
        else
        {
            if (!button.interactable) button.interactable = true;
            if (image.color != enable) image.color = enable;
            if (title.text != enableText) title.text = enableText;
            if (!flag.activeInHierarchy) flag.SetActive(true);
        }
    }

    private int ToSeconds(int hours, int minutes, int seconds)
    {
        return hours * 60 * 60 + minutes * 60 + seconds;
    }

    private string SecondsToTime(int seconds)
    {
        int hours = seconds / 3600, min = (seconds - hours * 3600) / 60, sec = seconds - hours * 3600 - min * 60;
        return hours.ToString() + ":" + (min < 10 ? "0" : "") + min.ToString() + ":" + (sec < 10 ? "0" : "") + sec.ToString();
    }

    public void UpdateTime()
    {
        now = DateTime.Now;
        island.SetParameter("DailyYear", now.Year);
        island.SetParameter("DailyMonth", now.Month);
        island.SetParameter("DailyDay", now.Day);
        year = island.GetParameter("DailyYear", 0);
        month = island.GetParameter("DailyMonth", 0);
        day = island.GetParameter("DailyDay", 0);
    }
}
