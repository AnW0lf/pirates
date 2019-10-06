using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public static Island Instance { get; private set; }
    //_______________________________________________________________________________
    public BigDigit Money { get; private set; }
    public int Level { get; private set; }
    public BigDigit Exp { get; private set; }
    public BigDigit StartExp { get; private set; }
    public int Lifebuoy { get; private set; }
    public int LifebuoyMax { get { return SpinLevel + 3; } }
    public BigDigit Premium { get; private set; }
    public int SpeedLevel { get; private set; }
    public int MoneyLevel { get; private set; }
    public int SpinLevel { get; private set; }
    //_______________________________________________________________________________
    private List<BigDigit> maxExps = new List<BigDigit>() {
 new BigDigit(1f, 2), // 0
        new BigDigit(1f, 2), // 1
        new BigDigit(1.5f, 2), // 2
        new BigDigit(2.25f, 2), // 3
        new BigDigit(3.38f, 2), // 4
        new BigDigit(5.7f, 2), // 5
        new BigDigit(7.61f, 2), // 6
        new BigDigit(1.142f, 3), // 7
        new BigDigit(1.713f, 3), // 8
        new BigDigit(3.855f, 3), // 9
        new BigDigit(5.783f, 3), // 10
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
        new BigDigit(2f, 5), // 25
        new BigDigit(5f, 5), // 26
        new BigDigit(6.5f, 5), // 27
        new BigDigit(2.9f, 6), // 28
        new BigDigit(4.5f, 6), // 29
        new BigDigit(6.9f, 6), // 30
        new BigDigit(9.2f, 6), // 31
        new BigDigit(1.8f, 7), // 32
        new BigDigit(2.3f, 7), // 33
        new BigDigit(2.7f, 7), // 34
        new BigDigit(4.25f, 7), // 35
        new BigDigit(5.1f, 7), // 36
        new BigDigit(5.9f, 7), // 37
        new BigDigit(7.7f, 7), // 38
        new BigDigit(9.8f, 7), // 39
        new BigDigit(1.17f, 8), // 40
        new BigDigit(1.3f, 8), // 41
        new BigDigit(1.5f, 8), // 42
        new BigDigit(1.7f, 8), // 43
        new BigDigit(1.95f, 8), // 44
        new BigDigit(2.1f, 8), // 45
        new BigDigit(2.25f, 8), // 46
        new BigDigit(2.35f, 8), // 47
        new BigDigit(2.7f, 8), // 48
        new BigDigit(2.9f, 8), // 49
        new BigDigit(3.1f, 8), // 50
        new BigDigit(9f, 8), // 51
        new BigDigit(1.5f, 9), // 52
        new BigDigit(6.3f, 9), // 53
        new BigDigit(9.9f, 9), // 54
        new BigDigit(1.5f, 10), // 55
        new BigDigit(2f, 10), // 56
        new BigDigit(4.5f, 10), // 57
        new BigDigit(4.7f, 10), // 58
        new BigDigit(5.3f, 10), // 59
        new BigDigit(9.4f, 10), // 60
        new BigDigit(1.11f, 11), // 61
        new BigDigit(1.29f, 11), // 62
        new BigDigit(1.7f, 11), // 63
        new BigDigit(2.1f, 11), // 64
        new BigDigit(2.6f, 11), // 65
        new BigDigit(3f, 11), // 66
        new BigDigit(3.3f, 11), // 67
        new BigDigit(4f, 11), // 68
        new BigDigit(4.5f, 11), // 69
        new BigDigit(4.7f, 11), // 70
        new BigDigit(4.9f, 11), // 71
        new BigDigit(5.15f, 11), // 72
        new BigDigit(5.3f, 11), // 73
        new BigDigit(5.5f, 11), // 74
        new BigDigit(7f, 11), // 75
    };
    //_______________________________________________________________________________
    private void Awake()
    {
        Instance = this;
        Load();
    }

    private void Load()
    {
        InitParameter("GlobalResetting", 0);
        if (GetParameter("GlobalResetting", 0) < 2)
        {
            PlayerPrefs.DeleteAll();
            SetParameter("GlobalResetting", 2);
        }

        InitParameter("MoneyMantissa", 5.5f);
        InitParameter("MoneyExponent", 1);
        Money = new BigDigit(GetParameter("MoneyMantissa", 0f), GetParameter("MoneyExponent", 0));

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        Level = PlayerPrefs.GetInt("Level");

        InitParameter("ExpMantissa", 0f);
        InitParameter("ExpExponent", 0);
        Exp = new BigDigit(GetParameter("ExpMantissa", 0f), GetParameter("ExpExponent", 0));

        InitParameter("StartExpMantissa", 0f);
        InitParameter("StartExpExponent", 0);
        StartExp = new BigDigit(GetParameter("StartExpMantissa", 0f), GetParameter("StartExpExponent", 0));

        InitParameter("Lifebuoy", 0);
        Lifebuoy = GetParameter("Lifebuoy", 0);

        InitParameter("SpinLevel", 0);
        SpinLevel = GetParameter("SpinLevel", 0);

        InitParameter("SpeedLevel", 0);
        SpeedLevel = GetParameter("SpeedLevel", 0);

        InitParameter("MoneyLevel", 0);
        MoneyLevel = GetParameter("MoneyLevel", 0);

        InitParameter("PremiumMantissa", 0f);
        InitParameter("PremiumExponent", 0);
        Premium = new BigDigit(GetParameter("PremiumMantissa", 0f), GetParameter("PremiumExponent", 0));
    }

    public void Save()
    {
        SetParameter("MoneyMantissa", (float)Money.Mantissa);
        SetParameter("MoneyExponent", (int)Money.Exponent);

        SetParameter("Level", Level);

        SetParameter("ExpMantissa", (float)Exp.Mantissa);
        SetParameter("ExpExponent", (int)Exp.Exponent);

        SetParameter("StartExpMantissa", (float)StartExp.Mantissa);
        SetParameter("StartExpExponent", (int)StartExp.Exponent);

        SetParameter("Lifebuoy", Lifebuoy);
        SetParameter("SpinLevel", SpinLevel);
        SetParameter("SpeedLevel", SpeedLevel);
        SetParameter("MoneyLevel", MoneyLevel);

        SetParameter("PremiumMantissa", (float)Premium.Mantissa);
        SetParameter("PremiumExponent", (int)Premium.Exponent);
    }

    public void Resetting()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public float moneyBonus { get { return MoneyLevel * 0.05f + 1f; } }
    public float speedBonus { get { return SpeedLevel * 0.05f + 1f; } }

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

    public bool ChangePremium(BigDigit other)
    {

        if ((Premium + other) >= BigDigit.zero)
        {
            Premium.Sum(other);
            EventManager.SendEvent("ChangePremium");
            return true;
        }
        return false;
    }

    public bool ChangeLifebuoy(int value)
    {
        int v = Lifebuoy + value;
        if (v >= 0)
        {
            Lifebuoy = v;
            EventManager.SendEvent("ChangeLifebuoy");
            return true;
        }
        return false;
    }

    public bool AddSpinLevel(BigDigit price)
    {
        bool b;
        if ((b = ChangePremium(price)))
        {
            SpinLevel++;
            EventManager.SendEvent("ChangeLifebuoyMax");
        }
        return b;
    }

    public bool AddSpeedLevel(BigDigit price)
    {
        bool b;
        if((b = ChangePremium(price)))
        {
            SpeedLevel++;
        }
        return b;
    }

    public bool AddMoneyLevel(BigDigit price)
    {
        bool b;
        if ((b = ChangePremium(price)))
        {
            MoneyLevel++;
        }
        return b;
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

        if (Lifebuoy < LifebuoyMax) Lifebuoy = LifebuoyMax;

        EventManager.SendEvent("LevelUp");
        Save();
    }

    public void InitParameter(string parameter, int initVal)
    {
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetInt(parameter, initVal);
    }

    public void InitParameter(string parameter, float initVal)
    {
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetFloat(parameter, initVal);
    }

    public void InitParameter(string parameter, string initVal)
    {
        if (!PlayerPrefs.HasKey(parameter))
            PlayerPrefs.SetString(parameter, initVal);
    }

    public void SetParameter(string parameter, int value)
    {
        PlayerPrefs.SetInt(parameter, value);
    }

    public void SetParameter(string parameter, float value)
    {
        PlayerPrefs.SetFloat(parameter, value);
    }

    public void SetParameter(string parameter, string value)
    {
        PlayerPrefs.SetString(parameter, value);
    }

}
