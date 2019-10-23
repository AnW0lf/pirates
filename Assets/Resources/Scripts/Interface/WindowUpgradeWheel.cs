using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUpgradeWheel : WindowBase
{
    [SerializeField] private Text text = null;
    [SerializeField] private Button btn = null;

    private void Start()
    {
        btn.onClick.AddListener(LuckyWheel.Instance.Switch);
        btn.onClick.AddListener(Close);
    }

    public override void Open(object[] args)
    {
        if (Island.Instance.Level > LuckyWheel.Instance.unlockLevel && LuckyWheel.Instance.levels.Contains(Island.Instance.Level))
        {
            //float mod = (float)arg0[0];
            base.Open(args);
            //text.text = "All rewards increased by " + mod.ToString();
            text.text = "Rewards Upgraded!";
        }
        else Close();
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }
}
