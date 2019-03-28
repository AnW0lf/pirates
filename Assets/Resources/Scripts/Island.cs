using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    private static Island island;
    //_______________________________________________________________________________
    public int Money { get; private set; }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int StartExp { get; private set; }
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
        if (!PlayerPrefs.HasKey("Money"))
            PlayerPrefs.SetInt("Money", 25);
        Money = PlayerPrefs.GetInt("Money");

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        Level = PlayerPrefs.GetInt("Level");

        if (!PlayerPrefs.HasKey("Exp"))
            PlayerPrefs.SetInt("Exp", 0);
        Exp = PlayerPrefs.GetInt("Exp");

        if (!PlayerPrefs.HasKey("StartExp"))
            PlayerPrefs.SetInt("StartExp", 4);
        StartExp = PlayerPrefs.GetInt("StartExp");

        if (!PlayerPrefs.HasKey("ExpModifier"))
            PlayerPrefs.SetFloat("ExpModifier", 1.2f);
        ExpModifier = PlayerPrefs.GetFloat("ExpModifier");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("Exp", Exp);
        PlayerPrefs.SetInt("StartExp", StartExp);
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

    public bool ChangeMoney(int value)
    {
        if (Money + value >= 0)
        {
            Money += value;
            return true;
        }
        return false;
    }

    public int GetMaxExp()
    {
        if (Level <= 10)
            return (int)(StartExp * Mathf.Pow(ExpModifier, Level));
        else
            return (int)(StartExp * Mathf.Pow(ExpModifier, 11));
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
        Save();
    }

    public void LevelUp()
    {
        Level++; Exp = 0;
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
