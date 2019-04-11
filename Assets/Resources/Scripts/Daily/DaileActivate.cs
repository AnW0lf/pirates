using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaileActivate : MonoBehaviour
{
    public int unlockLevel = 5;
    public GameObject child;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);
        InitInfo();
    }

    private void InitInfo()
    {
        if(island.Level >= unlockLevel)
        {
            child.SetActive(true);
        }
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (island.Level >= unlockLevel)
        {
            child.SetActive(true);
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
    }
}
