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
    public long Exp { get; private set; }
    public long StartExp { get; private set; }
    public float ExpModifier { get; private set; }

    private List<string> parameters;
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
        long oldExp = 0;
        if (PlayerPrefs.HasKey("Exp"))
        {
            try
            {
                oldExp = PlayerPrefs.GetInt("Exp");
            }
            catch
            {
                oldExp = long.Parse(PlayerPrefs.GetString("Exp"));
            }
            PlayerPrefs.DeleteKey("Exp");
        }

        if (!PlayerPrefs.HasKey("Exp"))
            PlayerPrefs.SetString("Exp", "0");
        Exp = long.Parse(PlayerPrefs.GetString("Exp")) + oldExp;

        if (!PlayerPrefs.HasKey("StartExp"))
            PlayerPrefs.SetString("StartExp", "150");
        StartExp = long.Parse(PlayerPrefs.GetString("StartExp"));

        if (!PlayerPrefs.HasKey("ExpModifier"))
            PlayerPrefs.SetFloat("ExpModifier", 1.85f);
        ExpModifier = PlayerPrefs.GetFloat("ExpModifier");
    }

    public void Save()
    {
        SetParameter("MoneyMantissa", (float)Money.Mantissa);
        SetParameter("MoneyExponent", (int)Money.Exponent);
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetString("Exp", Exp.ToString());
        PlayerPrefs.SetString("StartExp", StartExp.ToString());
        PlayerPrefs.SetFloat("ExpModifier", ExpModifier);
    }

    public void SetSettings(int money, int level, int exp, int startExp, int curExp, float expModifier)
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Exp", exp);
        PlayerPrefs.SetInt("StartExp", startExp);
        PlayerPrefs.SetInt("CurExp", curExp);
        PlayerPrefs.SetFloat("ExpModifier", expModifier);
        Load();
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

    public long GetMaxExp()
    {
        if (Level == 1)
            return (long)(StartExp * Mathf.Pow(Level, ExpModifier) / 1.5f);
        else if (Level <= 18)
            return (long)(StartExp * Mathf.Pow(Level, ExpModifier));
        else if (Level > 18 && Level <= 25)
            return (long)(StartExp * Mathf.Pow(18, ExpModifier));
        else if (Level > 25 && Level <= 50)
            return (long)((long)StartExp * (double)Mathf.Pow(Level, ExpModifier) * 12d * (long)(Level - 25));
        else
            return (long)((long)StartExp * (double)Mathf.Pow(Level, ExpModifier) * 7500d * (long)(Level - 50));
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

    public void ExpUp(int exp)
    {
        Exp += exp;
        EventManager.SendEvent("AddExp");
        Save();
    }

    public void LevelUp()
    {
        Level++; Exp = 0;
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
