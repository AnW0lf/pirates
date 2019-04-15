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
            for(int i = 0; i < piers.Count; i++)
            {
                if (piers[i].black && piers[i].GetBlackMark() > 0)
                {
                    cur = i;
                    break;
                }
                if (piers[i].minLvl <= island.Level && !piers[i].maxLvl && piers[i].GetUpgradeCost() <= island.Money)
                    cur = i;
            }
            if (cur == 0 && piers[cur].GetBlackMark() == 0)
            {
                for (int i = piers.Count - 1; i >= 0 ; i--)
                {
                    if (piers[i].minLvl <= island.Level && piers[i].shipExist)
                    {
                        cur = i;
                        break;
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
