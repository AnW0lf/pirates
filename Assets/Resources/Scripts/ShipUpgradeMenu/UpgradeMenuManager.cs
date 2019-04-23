using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuManager : MonoBehaviour
{
    public GameObject windows, miniatures;

    private WindowsManager wm;
    private MiniaturesManager mm;
    private Island island;

    private void Awake()
    {
        wm = windows.GetComponent<WindowsManager>();
        mm = miniatures.GetComponent<MiniaturesManager>();
        island = Island.Instance();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("OpenShipUpgradeMenu", Open);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("OpenShipUpgradeMenu", Open);
    }

    private void Open(object[] arg0)
    {
        if (arg0.Length > 0)
        {
            windows.SetActive(true);
            miniatures.SetActive(true);
            List<PierManager> piers = (List<PierManager>)arg0[0];
            wm.SetWindows(piers);
            mm.SetMiniatures(piers);

            int cur = 0;
            bool canUp = false;
            for(int i = 0; i < piers.Count; i++)
            {
                if (piers[i].minLvl <= island.Level)
                {
                    if(!piers[i].maxLvl
                        && (piers[i].GetUpgradeCost() <= island.Money
                        || (piers[i].black && piers[i].GetBlackMark() > 0)))
                    {
                        canUp = true;
                        cur = i;
                    }
                    else if (!canUp)
                    {
                        cur = i;
                    }
                }
            }
            mm.FocusMiniature(cur);
            wm.FocusWindow(cur);
        }
    }

    public void Close()
    {
        windows.SetActive(false);
        miniatures.SetActive(false);
    }
}
