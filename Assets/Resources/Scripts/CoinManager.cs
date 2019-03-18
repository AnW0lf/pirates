using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public string prefix = "";

    [SerializeField]
    private int money;
    private string postfix = "";
    private TextManager tm;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        tm = GetComponent<TextManager>();
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        tm.prefix = prefix;
        int degree = 0;
        float value = money;
        while (value >= 1000)
        {
            value /= 1000;
            degree++;
        }
        switch (degree)
        {
            case 0:
                postfix = "";
                break;
            case 1:
                postfix = "K";
                break;
            case 2:
                postfix = "M";
                break;
            case 3:
                postfix = "B";
                break;
            case 4:
                postfix = "T";
                break;
            case 5:
                postfix = "q";
                break;
            case 6:
                postfix = "Q";
                break;
            default:
                postfix = "?";
                break;
        }
        string strValue = value.ToString();
        if (value < 100)
        {
            if (strValue.Length >= 4)
                strValue = strValue.Substring(0, 4);
        }
        else
            strValue = strValue.Substring(0, 3);
        tm.text = strValue;
        tm.postfix = postfix;
    }

    private void Update()
    {
        if(money != island.GetMoney())
        {
            money = island.GetMoney();
            UpdateMoney();
        }
    }
}
