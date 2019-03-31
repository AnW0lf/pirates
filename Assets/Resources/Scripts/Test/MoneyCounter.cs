using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    private Global global;
    private Text text;

    private void Awake()
    {
        global = Global.Instance;
        text = GetComponent<Text>();
    }

    private void Start()
    {
        EventManager.Subscribe("ChangeMoney", UpdateCounter);
        UpdateCounter(new object[0]);
    }

    private void UpdateCounter(object[] arg0)
    {
        float money = global.Money;
        int degree = 0;
        while (money > 1000)
        {
            degree++;
            money /= 1000;
        }
        string str = money.ToString();
        str = str.Length >= 4 ? str.Substring(0, 4) : str;
        switch (degree)
        {
            case 0:
                break;
            case 1:
                str += "K";
                break;
            case 2:
                str += "M";
                break;
            case 3:
                str += "B";
                break;
            case 4:
                str += "T";
                break;
            case 5:
                str += "Q";
                break;
            default:
                str += "A";
                break;
        }
        text.text = str;
    }
}
