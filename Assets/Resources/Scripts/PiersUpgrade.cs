using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiersUpgrade : MonoBehaviour
{
    public GameObject flag;
    public List<ShipCtrl> ships;

    private void Start()
    {
        EventManager.Subscribe("ChangeMoney", UpdateInfo);
        EventManager.Subscribe("AddExp", UpdateInfo);
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void UpdateInfo(object[] arg0)
    {
        foreach (ShipCtrl ship in ships)
        {
            if (ship.Unlocked && ship.Cost <= Island.Instance().Money && !ship.MaxGraded)
            {
                if (!flag.activeInHierarchy)
                    flag.SetActive(true);
                break;
            }
            else
            {
                if (flag.activeInHierarchy)
                    flag.SetActive(false);
            }
        }
    }

    public void OpenShipUpgradeMenu()
    {
        EventManager.SendEvent("OpenShipUpgradeMenu", ships);
    }

    private void OnMouseUpAsButton()
    {
        EventManager.SendEvent("OpenShipUpgradeMenu", ships);
    }
}
