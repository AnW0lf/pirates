using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUpgradeWheel : WindowBase
{
    [SerializeField] private Text text = null;
    [SerializeField] private Button btn = null;
    [SerializeField] private List<LuckyWheel> wheels = null;
    [SerializeField] protected ScrollManager sm;

    private WheelButton wb = null;
    private Island island;

    private void Awake()
    {
        island = Island.Instance;
    }

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
        bool requaredLevel()
        {
            foreach (LuckyWheel w in wheels)
            {
                if (w.levels.Contains(island.Level))
                    return true;
            }
            return false;
        }
        if (requaredLevel())
        {
            //float mod = (float)arg0[0];
            base.Open(args);
            btn.onClick.RemoveAllListeners();
            if (wb != null)
                btn.onClick.AddListener(wb.WheelSwitch);
            btn.onClick.AddListener(OpenWheel);
            btn.onClick.AddListener(Close);
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

    public void OpenWheel()
    {
        foreach (LuckyWheel w in wheels)
        {
            if (w.levels.Contains(island.Level))
            {
                //w.wb.WheelSwitch();
                StartCoroutine(Center());
                return;
            }
        }
    }

    private IEnumerator Center()
    {
        bool Opened() { return transform.parent.GetComponent<InterfaceIerarchy>().Done; }
        yield return new WaitWhile(Opened);
        yield return new WaitForSeconds(0.5f);
        sm.Center();
    }
}
