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
    public List<BigDigit> maxExps = new List<BigDigit>();
    //_______________________________________________________________________________
    private void Awake()
    {
        Instance = this;
        Load();

        Application.targetFrameRate = 60;
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
