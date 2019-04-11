﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyWindowController : MonoBehaviour
{
    public Transform rewards;
    public GameObject button;
    public Text timer;

    private Island island;
    private int dailyDaysARaw, year, month, day;
    private DateTime now;

    private void Awake()
    {
        island = Island.Instance();
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

        island.InitParameter("DailyDaysARaw", 0);
        dailyDaysARaw = island.GetParameter("DailyDaysARaw", 0);

        UpdateRewardState(year == now.Year && month == now.Month && day == now.Day);
    }

    private void Update()
    {
        now = DateTime.Now;
        if (year == now.Year && month == now.Month && day == now.Day)
        {
            if (button.activeInHierarchy) button.SetActive(false);
            if (!timer.gameObject.activeInHierarchy) timer.gameObject.SetActive(true);
            timer.text = "Next Bonus in " + SecondsToTime(ToSeconds(24, 0, 0) - ToSeconds(now.Hour, now.Minute, now.Second));
        }
        else
        {
            if (timer.gameObject.activeInHierarchy) timer.gameObject.SetActive(false);
            if (!button.activeInHierarchy) button.SetActive(true);
        }
    }

    private void UpdateRewardState(bool claimed)
    {
        for (int i = 0; i < rewards.childCount; i++)
        {
            rewards.GetChild(i).GetComponent<DailyRewardState>().SetState(i < dailyDaysARaw ? 0 : (i == dailyDaysARaw ? (claimed ? 2 : 1) : 2));
        }
    }

    public void Claim()
    {
        TakeReward(rewards.GetChild(dailyDaysARaw).GetComponent<DailyRewardState>());
        dailyDaysARaw = ++dailyDaysARaw % 7;
        island.SetParameter("DailyDaysARaw", dailyDaysARaw);
        year = island.GetParameter("DailyYear", 0);
        month = island.GetParameter("DailyMonth", 0);
        day = island.GetParameter("DailyDay", 0);
        UpdateRewardState(true);
    }

    private void TakeReward(DailyRewardState dailyRewardState)
    {
        Debug.Log("Given " + dailyRewardState.reward.ToString() + " black marks.");
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
}
