using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiersUpgrade : MonoBehaviour
{
    public GameObject flag;
    public List<PierManager> piers;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
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

    private void UpdateInfo(object[] arg0)
    {
        foreach (PierManager pier in piers)
        {
            if (((pier.black && pier.GetBlackMark() > 0 ) || (pier.minLvl <= island.Level && pier.GetUpgradeCost() <= island.Money)) && !pier.maxLvl)
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
        EventManager.SendEvent("OpenShipUpgradeMenu", piers);
    }
}
