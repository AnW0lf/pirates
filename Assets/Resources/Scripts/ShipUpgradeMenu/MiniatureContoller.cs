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
    private ShipCtrl ship;

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

    public void SetInfo(ShipCtrl ship)
    {
        this.ship = ship;
        UpdateInfo(new object[0]);
    }

    private void UpdateInfo(object[] arg0)
    {
        if (ship == null) return;
        icon.sprite = ship.SpriteForMenu;

        if (!ship.Unlocked)
        {
            icon.color = Color.black;
            level.text = "Level " + ship.UnlockLevel;
        }
        else if (!ship.Exists)
        {
            //if (ship.black)
            //    icon.color = Color.black;
            //else
                icon.color = Color.white;
            level.text = "0/" + (ship.levels.Length).ToString();
        }
        else if (!ship.MaxGraded)
        {
            icon.color = Color.white;
            level.text = (ship.Level).ToString()
                + "/" + (ship.levels.Length).ToString();
        }
        else
        {
            icon.color = Color.white;
            level.text = "Max";
        }
        Light.SetActive(/*ship.black ? ship.GetBlackMark() > 0 && !ship.maxLvl: */ship.Unlocked && ship.Cost <= island.Money && !ship.MaxGraded);
    }
}
