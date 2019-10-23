using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    public Text title;
    public Image fill;

    private Island island;

    private void Awake()
    {
        island = Island.Instance;
    }

    private void OnEnable()
    {
        EventManager.Subscribe("AddExp", UpdateProgress);
        EventManager.Subscribe("LevelUp", UpdateTitle);
        UpdateInfo();
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("LevelUp", UpdateTitle);
        EventManager.Unsubscribe("AddExp", UpdateProgress);
    }

    private void UpdateProgress(object[] arg0)
    {
        fill.fillAmount = (island.Exp / island.GetMaxExp()).ToFloat();
    }

    private void UpdateTitle(object[] arg0)
    {
        fill.fillAmount = (island.Exp / island.GetMaxExp()).ToFloat();
        title.text = "Level " + island.Level;
    }

    private void UpdateInfo()
    {
        fill.fillAmount = (island.Exp / island.GetMaxExp()).ToFloat();
        title.text = "Level " + island.Level;
    }

    public void Resetting()
    {
        island.Resetting();
    }
}
