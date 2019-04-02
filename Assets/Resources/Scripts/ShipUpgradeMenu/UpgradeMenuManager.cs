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

    private void Awake()
    {
        wm = windows.GetComponent<WindowsManager>();
        mm = miniatures.GetComponent<MiniaturesManager>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("OpenShipUpgradeMenu", Open);
        EventManager.Subscribe("CloseShipUpgradeMenu", Close);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("OpenShipUpgradeMenu", Open);
        EventManager.Unsubscribe("CloseShipUpgradeMenu", Close);
    }

    private void Open(object[] arg0)
    {
        Debug.Log("Opening");
        if (arg0.Length > 0)
        {
            windows.SetActive(true);
            miniatures.SetActive(true);
            wm.SetWindows((List<PierManager>)arg0[0]);
            mm.SetMiniatures((List<PierManager>)arg0[0]);
            Debug.Log("Opened");
        }
    }

    private void Close(object[] arg0)
    {
        windows.SetActive(false);
        miniatures.SetActive(false);
    }
}
