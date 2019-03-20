using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceManager : MonoBehaviour
{
    public string prefix = "", postfix = "";
    private TextManager tm;

    private void Awake()
    {
        tm = GetComponent<TextManager>();
    }

    public void SetPrice(int money)
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
        if (strValue.Length >= 5)
            strValue = strValue.Substring(0, 5);
        tm.text = strValue;
        tm.postfix = postfix;
    }
}
