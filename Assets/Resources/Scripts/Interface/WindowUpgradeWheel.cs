using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUpgradeWheel : WindowBase
{
    public Text text;
    public Button btn;

    private WheelButton wb = null;

    private void Start()
    {
        EventManager.Subscribe("UpgradeWheel", UpdateInfo);
    }

    private void UpdateInfo(object[] args)
    {
        if (args.Length == 2)
            wb = (WheelButton)args[1];
    }

    public override void Open(object[] args)
    {

        //float mod = (float)arg0[0];
        base.Open(args);
        btn.onClick.RemoveAllListeners();
        if (wb != null)
            btn.onClick.AddListener(wb.WheelSwitch);
        btn.onClick.AddListener(Close);
        //text.text = "All rewards increased by " + mod.ToString();
        text.text = "Rewards Upgraded!";
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }
}
