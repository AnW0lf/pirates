using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShipButton : MonoBehaviour
{
    private Global global;
    private Button btn;
    private Pier pier;

    private void Awake()
    {
        global = Global.Instance;
        btn = GetComponent<Button>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("ChangeMoney", UpdateState);
        EventManager.Subscribe("UpgradeMenuButtonUpdate", UpdateInfo);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ChangeMoney", UpdateState);
        EventManager.Unsubscribe("UpgradeMenuButtonUpdate", UpdateInfo);
    }

    private void UpdateInfo(object[] arg0)
    {
        pier = null;
        if (arg0.Length > 0)
            pier = (Pier)arg0[0];
        UpdateState(new object[0]);
    }

    private void UpdateState(object[] arg0)
    {
        if (pier != null)
        {
            if (pier.GetUpgradeCost() <= global.Money && pier.level < pier.maxLevel)
            {
                btn.interactable = true;
            }
            else
            {
                btn.interactable = false;
            }
        }
        else btn.interactable = false;
    }

    public void Click()
    {
        if (global.ChangeMoney(-pier.GetUpgradeCost()))
        {
            EventManager.SendEvent(pier.shipName + "Upgrade");
            EventManager.SendEvent("ChangeMoney");
        }

    }
}
