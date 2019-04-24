using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWheelWindow : MonoBehaviour
{
    public GameObject child, fade;
    public Text text;
    public Button btn;

    private void OnEnable()
    {
        EventManager.Subscribe("UpgradeWheel", Open);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("UpgradeWheel", Open);
    }

    private void Open(object[] arg0)
    {
        if(arg0.Length == 2)
        {
            //float mod = (float)arg0[0];
            WheelButton wb = (WheelButton)arg0[1];
            child.SetActive(true);
            fade.SetActive(true);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(wb.WheelSwitch);
            btn.onClick.AddListener(Close);
            //text.text = "All rewards increased by " + mod.ToString();
            text.text = "Rewards Upgraded!";
        }
    }

    private void Close()
    {
        child.SetActive(false);
        fade.SetActive(false);
    }
}
