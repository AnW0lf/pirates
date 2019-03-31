using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinCounter : MonoBehaviour
{
    public string planeName;

    private Global global;
    private int curSpin, maxSpin;

    private void Awake()
    {
        global = Global.Instance;
        global.InitParameter(planeName + "Spin", 0);
        global.InitParameter(planeName + "MaxSpin", 3);
        curSpin = global.GetParameter(planeName + "Spin", 0);
        maxSpin = global.GetParameter(planeName + "MaxSpin", 0);
    }

    private void OnEnable()
    {
        EventManager.Subscribe("AddSpin", AddSpin);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("AddSpin", AddSpin);
    }

    private void AddSpin(object[] arg0)
    {
        global.SetParameter(planeName + "Spin", ++curSpin);
        GetComponent<Text>().text = curSpin + "/" + maxSpin;
    }
}
