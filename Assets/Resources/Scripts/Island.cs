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
        new BigDigit(1f, 0), // 0
        new BigDigit(1f, 1), // 1
        new BigDigit(1f, 1), // 2
        new BigDigit(1f, 1), // 3
        new BigDigit(1f, 1), // 4
        new BigDigit(1f, 1), // 5
        new BigDigit(1f, 1), // 6
        new BigDigit(1f, 1), // 7
        new BigDigit(1f, 1), // 9
        new BigDigit(1f, 1), // 8
        new BigDigit(1f, 1), // 10
        new BigDigit(1f, 2), // 11
        new BigDigit(1f, 2), // 12
        new BigDigit(1f, 2), // 13
        new BigDigit(1f, 2), // 14
        new BigDigit(1f, 2), // 15
        new BigDigit(1f, 2), // 16
        new BigDigit(1f, 2), // 17
        new BigDigit(1f, 2), // 18
        new BigDigit(1f, 2), // 19
        new BigDigit(1f, 2), // 20
        new BigDigit(1f, 3), // 21
        new BigDigit(1f, 3), // 22
        new BigDigit(1f, 3), // 23
        new BigDigit(1f, 3), // 24
        new BigDigit(1f, 3), // 25
        new BigDigit(1f, 3), // 26
        new BigDigit(1f, 3), // 27
        new BigDigit(1f, 3), // 28
        new BigDigit(1f, 3), // 29
        new BigDigit(1f, 3), // 30
        new BigDigit(1f, 4), // 31
        new BigDigit(1f, 4), // 32
        new BigDigit(1f, 4), // 33
        new BigDigit(1f, 4), // 34
        new BigDigit(1f, 4), // 35
        new BigDigit(1f, 4), // 36
        new BigDigit(1f, 4), // 37
        new BigDigit(1f, 4), // 38
        new BigDigit(1f, 4), // 39
        new BigDigit(1f, 4), // 40
        new BigDigit(1f, 5), // 41
        new BigDigit(1f, 5), // 42
        new BigDigit(1f, 5), // 43
        new BigDigit(1f, 5), // 44
        new BigDigit(1f, 5), // 45
        new BigDigit(1f, 5), // 46
        new BigDigit(1f, 5), // 47
        new BigDigit(1f, 5), // 48
        new BigDigit(1f, 5), // 49
        new BigDigit(1f, 5), // 50
        new BigDigit(1f, 6), // 51
        new BigDigit(1f, 6), // 52
        new BigDigit(1f, 6), // 53
        new BigDigit(1f, 6), // 54
        new BigDigit(1f, 6), // 55
        new BigDigit(1f, 6), // 56
        new BigDigit(1f, 6), // 57
        new BigDigit(1f, 6), // 58
        new BigDigit(1f, 6), // 59
        new BigDigit(1f, 6), // 60
        new BigDigit(1f, 7), // 61
        new BigDigit(1f, 7), // 62
        new BigDigit(1f, 7), // 63
        new BigDigit(1f, 7), // 64
        new BigDigit(1f, 7), // 65
        new BigDigit(1f, 7), // 66
        new BigDigit(1f, 7), // 67
        new BigDigit(1f, 7), // 68
        new BigDigit(1f, 7), // 69
        new BigDigit(1f, 7), // 70
        new BigDigit(1f, 8), // 71
        new BigDigit(1f, 8), // 72
        new BigDigit(1f, 8), // 73
        new BigDigit(1f, 8), // 74
        new BigDigit(1f, 8), // 75
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
            float exp = long.Parse(PlayerPrefs.GetString("Exp"));
            InitParameter("ExpMantissa", exp);
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
            float startExp = long.Parse(PlayerPrefs.GetString("StartExp"));
            InitParameter("StartExpMantissa", startExp);
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

    public void ExpUp(long exp)
    {
        Exp += new BigDigit(exp);
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
