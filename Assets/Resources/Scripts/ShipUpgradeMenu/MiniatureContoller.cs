using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniatureContoller : MonoBehaviour
{
    public Text level;
    public Image icon;
    public GameObject backLight, Light;

    private Island island;
    private Transform parent;
    private PierManager pier;

    private void Awake()
    {
        island = Island.Instance();
        parent = transform.parent;
    }

    private void OnEnable()
    {
        EventManager.Subscribe("ChangeMoney", UpdateInfo);
        EventManager.Subscribe("AddExp", UpdateInfo);
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ChangeMoney", UpdateInfo);
        EventManager.Unsubscribe("AddExp", UpdateInfo);
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
    }

    public void UnfocusThis()
    {
        backLight.SetActive(false);
    }

    public void FocusThis()
    {
        foreach (MiniatureContoller child in parent.GetComponentsInChildren<MiniatureContoller>())
            child.UnfocusThis();
        backLight.SetActive(true);
    }

    public void SetInfo(PierManager pier)
    {
        this.pier = pier;
        UpdateInfo(new object[0]);
    }

    private void UpdateInfo(object[] arg0)
    {
        if (pier == null) return;
        icon.sprite = pier.shipIcon;

        if (pier.minLvl > island.Level)
        {
            icon.color = Color.black;
            level.text = "Level " + pier.minLvl;
        }
        else if (!pier.shipExist)
        {
            if (pier.black)
                icon.color = Color.black;
            else
                icon.color = Color.white;
            level.text = "0/" + (1 + pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3).ToString();
        }
        else if (!pier.maxLvl)
        {
            icon.color = Color.white;
            level.text = (1 + pier.detailCurrentLvl1 + pier.detailCurrentLvl2 + pier.detailCurrentLvl3).ToString()
                + "/" + (1 + pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3).ToString();
        }
        else
        {
            icon.color = Color.white;
            level.text = "Max";
        }
        Light.SetActive(pier.black ? pier.GetBlackMark() > 0 : pier.minLvl <= island.Level && pier.GetUpgradeCost() <= island.Money && !pier.maxLvl);
    }
}
