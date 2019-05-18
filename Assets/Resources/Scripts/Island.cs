using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    private static Island island;
    //_______________________________________________________________________________
    public BigDigit Money { get; private set; }
    public int Level { get; private set; }
    public BigDigit Exp { get; private set; }
    public BigDigit StartExp { get; private set; }

    private List<string> parameters;
    //_______________________________________________________________________________
    private List<BigDigit> maxExps = new List<BigDigit>() {
        new BigDigit(3f, 1), // 0
        new BigDigit(8.5f, 1), // 1
        new BigDigit(3.5f, 2), // 2
        new BigDigit(9.5f, 2), // 3
        new BigDigit(1.45f, 3), // 4
        new BigDigit(2.1f, 3), // 5
        new BigDigit(3.25f, 3), // 6
        new BigDigit(4.12f, 3), // 7
        new BigDigit(5.12f, 3), // 9
        new BigDigit(6.22f, 3), // 8
        new BigDigit(1f, 4), // 10
        new BigDigit(1.17f, 4), // 11
        new BigDigit(1.36f, 4), // 12
        new BigDigit(1.55f, 4), // 13
        new BigDigit(1.77f, 4), // 14
        new BigDigit(2.66f, 4), // 15
        new BigDigit(2.98f, 4), // 16
        new BigDigit(3.32f, 4), // 17
        new BigDigit(3.35f, 4), // 18
        new BigDigit(3.37f, 4), // 19
        new BigDigit(3.39f, 4), // 20
        new BigDigit(3.41f, 4), // 21
        new BigDigit(3.43f, 4), // 22
        new BigDigit(3.45f, 4), // 23
        new BigDigit(3.5f, 4), // 24
        new BigDigit(1f, 5), // 25
        new BigDigit(2f, 5), // 26
        new BigDigit(4.5f, 5), // 27
        new BigDigit(1f, 6), // 28
        new BigDigit(1.5f, 6), // 29
        new BigDigit(2f, 6), // 30
        new BigDigit(2.75f, 6), // 31
        new BigDigit(3.4f, 6), // 32
        new BigDigit(4.1f, 6), // 33
        new BigDigit(5.8f, 6), // 34
        new BigDigit(6.8f, 6), // 35
        new BigDigit(7.9f, 6), // 36
        new BigDigit(9.1f, 6), // 37
        new BigDigit(1.03f, 7), // 38
        new BigDigit(1.56f, 7), // 39
        new BigDigit(1.75f, 7), // 40
        new BigDigit(1.96f, 7), // 41
        new BigDigit(2.18f, 7), // 42
        new BigDigit(2.41f, 7), // 43
        new BigDigit(3.55f, 7), // 44
        new BigDigit(3.9f, 7), // 45
        new BigDigit(4.3f, 7), // 46
        new BigDigit(4.65f, 7), // 47
        new BigDigit(5.04f, 7), // 48
        new BigDigit(5.45f, 7), // 49
        new BigDigit(1.592f, 8), // 50
        new BigDigit(3.9f, 8), // 51
        new BigDigit(8.3f, 8), // 52
        new BigDigit(1.85f, 9), // 53
        new BigDigit(2.93f, 9), // 54
        new BigDigit(3.55f, 9), // 55
        new BigDigit(5.5f, 9), // 56
        new BigDigit(6.82f, 9), // 57
        new BigDigit(8.05f, 9), // 58
        new BigDigit(1.11f, 10), // 59
        new BigDigit(1.27f, 10), // 60
        new BigDigit(1.44f, 10), // 61
        new BigDigit(1.615f, 10), // 62
        new BigDigit(1.81f, 10), // 63
        new BigDigit(2.7f, 10), // 64
        new BigDigit(2.97f, 10), // 65
        new BigDigit(3.25f, 10), // 66
        new BigDigit(3.55f, 10), // 67
        new BigDigit(3.87f, 10), // 68
        new BigDigit(5.6f, 10), // 69
        new BigDigit(6.05f, 10), // 70
        new BigDigit(6.55f, 10), // 71
        new BigDigit(7.05f, 10), // 72
        new BigDigit(7.55f, 10), // 73
        new BigDigit(8.05f, 10), // 74
        new BigDigit(9f, 10), // 75
    };
    //_______________________________________________________________________________
    private Island()
    {
        parameters = new List<string>();
        Load();
    }

    public static Island Instance()
    {
        if (island == null)
        {
            island = new Island();
        }
        return island;
    }

    private void Load()
    {
        InitParameter("GlobalResetting", 0);
        if (GetParameter("GlobalResetting", 0) < 1)
        {
            PlayerPrefs.DeleteAll();
            SetParameter("GlobalResetting", 1);
        }

        InitParameter("MoneyMantissa", 1f);
        InitParameter("MoneyExponent", 15);
        Money = new BigDigit(GetParameter("MoneyMantissa", 0f), GetParameter("MoneyExponent", 0));

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 51);
        Level = PlayerPrefs.GetInt("Level");

        //Проверка на старый формат Exp
        if (!PlayerPrefs.HasKey("OldExpDeleted"))
            PlayerPrefs.SetInt("OldExpDeleted", 0);
        if(PlayerPrefs.GetInt("OldExpDeleted") == 0)
        {
            PlayerPrefs.DeleteKey("Exp");
            PlayerPrefs.SetInt("OldExpDeleted", 1);
        }
        if (PlayerPrefs.GetInt("OldExpDeleted") == 1)
        {
            //float exp = long.Parse(PlayerPrefs.GetString("Exp"));
            //InitParameter("ExpMantissa", exp);
            PlayerPrefs.DeleteKey("Exp");
            PlayerPrefs.SetInt("OldExpDeleted", 2);
        }

        InitParameter("ExpMantissa", 0f);
        InitParameter("ExpExponent", 0);
        Exp = new BigDigit(GetParameter("ExpMantissa", 0f), GetParameter("ExpExponent", 0));

        //Проверка на старый формат StartExp
        if (!PlayerPrefs.HasKey("OldStartExpDeleted"))
            PlayerPrefs.SetInt("OldStartExpDeleted", 0);
        if (PlayerPrefs.GetInt("OldStartExpDeleted") == 0)
        {
            PlayerPrefs.DeleteKey("StartExp");
            PlayerPrefs.SetInt("OldStartExpDeleted", 1);
        }
        if (PlayerPrefs.GetInt("OldStartExpDeleted") == 1)
        {
            //float startExp = long.Parse(PlayerPrefs.GetString("StartExp"));
            //InitParameter("StartExpMantissa", startExp);
            PlayerPrefs.DeleteKey("StartExp");
            PlayerPrefs.SetInt("OldStartExpDeleted", 2);
        }

        InitParameter("StartExpMantissa", 0f);
        InitParameter("StartExpExponent", 0);
        StartExp = new BigDigit(GetParameter("StartExpMantissa", 0f), GetParameter("StartExpExponent", 0));

        //Проверка на наличие старой переменной ExpModifier
        if (PlayerPrefs.HasKey("ExpModifier"))
            PlayerPrefs.DeleteKey("ExpModifier");
    }

    public void Save()
    {
        SetParameter("MoneyMantissa", (float)Money.Mantissa);
        SetParameter("MoneyExponent", (int)Money.Exponent);
        PlayerPrefs.SetInt("Level", Level);
        SetParameter("ExpMantissa", (float)Exp.Mantissa);
        SetParameter("ExpExponent", (int)Exp.Exponent);
        SetParameter("StartExpMantissa", (float)StartExp.Mantissa);
        SetParameter("StartExpExponent", (int)StartExp.Exponent);
    }

    public void Resetting()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public bool ChangeMoney(BigDigit other)
    {

        if ((Money + other) >= BigDigit.zero)
        {
            Money.Sum(other);
            EventManager.SendEvent("ChangeMoney");
            return true;
        }
        return false;
    }

    public BigDigit GetMaxExp()
    {
        if (maxExps.Count > Level)
            return maxExps[Level];
        else
            return maxExps[maxExps.Count - 1];
    }

    public int GetParameter(string parameter, int useless)
    {
        if (PlayerPrefs.HasKey(parameter))
            return PlayerPrefs.GetInt(parameter);
        return 0;
    }

    public float GetParameter(string parameter, float useless)
    {
        if (PlayerPrefs.HasKey(parameter))
            return PlayerPrefs.GetFloat(parameter);
        return 0f;
    }

    public string GetParameter(string parameter, string useless)
    {
        if (PlayerPrefs.HasKey(parameter))
            return PlayerPrefs.GetString(parameter);
        return "";
    }

    public void ExpUp(BigDigit exp)
    {
        Exp += exp;
        EventManager.SendEvent("AddExp");
        Save();
    }

    public void LevelUp()
    {
        Level++; Exp = BigDigit.zero;
        EventManager.SendEvent("LevelUp");
        Save();
    }

    public void InitParameter(string parameter, int initVal)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetInt(parameter, initVal);
    }

    public void InitParameter(string parameter, float initVal)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetFloat(parameter, initVal);
    }

    public void InitParameter(string parameter, string initVal)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetString(parameter, initVal);
    }

    public void SetParameter(string parameter, int value)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        PlayerPrefs.SetInt(parameter, value);
    }

    public void SetParameter(string parameter, float value)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        PlayerPrefs.SetFloat(parameter, value);
    }

    public void SetParameter(string parameter, string value)
    {
        if (!parameters.Contains(parameter))
            parameters.Add(parameter);
        PlayerPrefs.SetString(parameter, value);
    }

}
