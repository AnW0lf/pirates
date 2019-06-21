﻿using System;
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
        new BigDigit(4f, 1), // 0
        new BigDigit(7f, 1), // 1
        new BigDigit(3.5f, 2), // 2
        new BigDigit(4.7f, 2), // 3
        new BigDigit(2.1f, 3), // 4
        new BigDigit(3.3f, 3), // 5
        new BigDigit(5f, 3), // 6
        new BigDigit(6.7f, 3), // 7
        new BigDigit(1.51f, 4), // 8
        new BigDigit(1.58f, 4), // 9
        new BigDigit(1.8f, 4), // 10
        new BigDigit(3.168f, 4), // 11
        new BigDigit(3.744f, 4), // 12
        new BigDigit(4.32f, 4), // 13
        new BigDigit(5.76f, 4), // 14
        new BigDigit(7.2f, 4), // 15
        new BigDigit(8.64f, 4), // 16
        new BigDigit(1.296f, 5), // 17
        new BigDigit(1.368f, 5), // 18
        new BigDigit(1.44f, 5), // 19
        new BigDigit(1.512f, 5), // 20
        new BigDigit(1.584f, 5), // 21
        new BigDigit(1.656f, 5), // 22
        new BigDigit(1.728f, 5), // 23
        new BigDigit(2f, 5), // 24
        new BigDigit(45f, 5), // 25
        new BigDigit(7.5f, 5), // 26
        new BigDigit(1.1f, 6), // 27
        new BigDigit(2.9f, 6), // 28
        new BigDigit(4.5f, 6), // 29
        new BigDigit(6.8f, 6), // 30
        new BigDigit(9f, 6), // 31
        new BigDigit(1.9f, 7), // 32
        new BigDigit(2.2f, 7), // 33
        new BigDigit(2.4f, 7), // 34
        new BigDigit(4.35f, 7), // 35
        new BigDigit(5.1f, 7), // 36
        new BigDigit(5.8f, 7), // 37
        new BigDigit(7.8f, 7), // 38
        new BigDigit(9.7f, 7), // 39
        new BigDigit(1.07f, 8), // 40
        new BigDigit(1.15f, 8), // 41
        new BigDigit(1.9f, 8), // 42
        new BigDigit(2f, 8), // 43
        new BigDigit(2f, 8), // 44
        new BigDigit(2.1f, 8), // 45
        new BigDigit(2.2f, 8), // 46
        new BigDigit(2.3f, 8), // 47
        new BigDigit(2.4f, 8), // 48
        new BigDigit(3f, 8), // 49
        new BigDigit(5f, 8), // 50
        new BigDigit(1.2f, 9), // 51
        new BigDigit(2.8f, 9), // 52
        new BigDigit(6f, 9), // 53
        new BigDigit(9.8f, 9), // 54
        new BigDigit(1.5f, 10), // 55
        new BigDigit(2f, 10), // 56
        new BigDigit(4.1f, 10), // 57
        new BigDigit(4.8f, 10), // 58
        new BigDigit(5.3f, 10), // 59
        new BigDigit(9.4f, 10), // 60
        new BigDigit(1.1f, 11), // 61
        new BigDigit(1.27f, 11), // 62
        new BigDigit(1.72f, 11), // 63
        new BigDigit(2.12f, 11), // 64
        new BigDigit(2.35f, 11), // 65
        new BigDigit(2.53f, 11), // 66
        new BigDigit(4.2f, 11), // 67
        new BigDigit(4.4f, 11), // 68
        new BigDigit(4.4f, 11), // 69
        new BigDigit(4.6f, 11), // 70
        new BigDigit(4.8f, 11), // 71
        new BigDigit(5.1f, 11), // 72
        new BigDigit(5.4f, 11), // 73
        new BigDigit(5.7f, 11), // 74
        new BigDigit(7f, 11), // 75
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

        InitParameter("MoneyMantissa", 0f);
        InitParameter("MoneyExponent", 0);
        Money = new BigDigit(GetParameter("MoneyMantissa", 0f), GetParameter("MoneyExponent", 0));

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
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
