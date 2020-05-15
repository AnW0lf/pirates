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

    private long accumulation;
    public readonly string iphone_leaderboard_id = "IslandTycoon.Rich_Man";
    //_______________________________________________________________________________
    private List<BigDigit> maxExps = new List<BigDigit>() {
        new BigDigit(1.2f, 2), // 0
        new BigDigit(1.2f, 2), // 0 (1)
        new BigDigit(3.67f, 2), // 1
        new BigDigit(8.37f, 2), // 2
        new BigDigit(1.32f, 3), // 3
        new BigDigit(2.037f, 3), // 4
        new BigDigit(3.031f, 3), // 5
        new BigDigit(6.184f, 3), // 6
        new BigDigit(8.454f, 3), // 7
        new BigDigit(1.1344f, 4), // 8
        new BigDigit(1.5273f, 4), // 9

        new BigDigit(2.021f, 4), // 10
        new BigDigit(3.7496f, 4), // 11
        new BigDigit(4.7293f, 4), // 12
        new BigDigit(5.9689f, 4), // 13
        new BigDigit(7.4567f, 4), // 14
        new BigDigit(9.3195f, 4), // 15
        new BigDigit(1.1644f, 5), // 16
        new BigDigit(1.81723f, 5), // 17
        new BigDigit(2.19476f, 5), // 18
        new BigDigit(2.42315f, 5), // 19
        new BigDigit(2.90676f, 5), // 20
        new BigDigit(3.852f, 5), // 21
        new BigDigit(5.5f, 5), // 22
        new BigDigit(6.9f, 5), // 23

        new BigDigit(5.4f, 5), // 24
        new BigDigit(2.58f, 6), // 25
        new BigDigit(6.156f, 6), // 26
        new BigDigit(1.1556f, 7), // 27
        new BigDigit(1.93f, 7), // 28
        new BigDigit(3.042f, 7), // 29
        new BigDigit(4.58f, 7), // 30
        new BigDigit(6.95f, 7), // 31
        new BigDigit(1.029f, 8), // 32
        new BigDigit(1.4735f, 8), // 33
        new BigDigit(2.072f, 8), // 34
        new BigDigit(2.8f, 8), // 35
        new BigDigit(3.94f, 8), // 36
        new BigDigit(5.4f, 8), // 37
        new BigDigit(6.98f, 8), // 38
        new BigDigit(9.01f, 8), // 39
        new BigDigit(1.157f, 9), // 40
        new BigDigit(1.3455f, 9), // 41
        new BigDigit(1.633f, 9), // 42
        new BigDigit(1.8184f, 9), // 43
        new BigDigit(2.155f, 9), // 44
        new BigDigit(2.58f, 9), // 45
        new BigDigit(3.3f, 9), // 46
        new BigDigit(4f, 9), // 47
        new BigDigit(5.2f, 9), // 48

        new BigDigit(1.5f, 9), // 49
        new BigDigit(8.23f, 9), // 50
        new BigDigit(2f, 10), // 51
        new BigDigit(3.795f, 10), // 52
        new BigDigit(6.365f, 10), // 53
        new BigDigit(1f, 11), // 54
        new BigDigit(1.5f, 11), // 55
        new BigDigit(2.38f, 11), // 56
        new BigDigit(3.4f, 11), // 57
        new BigDigit(4.8f, 11), // 58
        new BigDigit(6.63f, 11), // 59
        new BigDigit(9.32f, 11), // 60
        new BigDigit(1.3f, 12), // 61
        new BigDigit(1.765f, 12), // 62
        new BigDigit(2.32f, 12), // 63
        new BigDigit(3.02f, 12), // 64
        new BigDigit(3.89f, 12), // 65
        new BigDigit(4.48f, 12), // 66
        new BigDigit(5.385f, 12), // 67
        new BigDigit(5.915f, 12), // 68
        new BigDigit(7.04f, 12), // 69
        new BigDigit(9f, 12), // 70
        new BigDigit(1.3f, 13), // 71
        new BigDigit(1.8f, 13), // 72
        new BigDigit(2.5f, 13), // 73


        new BigDigit(5.5f, 14), // 74
        new BigDigit(7f, 15), // 75
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
        if (GetParameter("GlobalResetting", 0) < 3)
        {
            PlayerPrefs.DeleteAll();
            SetParameter("GlobalResetting", 3);
        }

        InitParameter("MoneyMantissa", 0f);
        InitParameter("MoneyExponent", 0);
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

        InitParameter("Accumulation", "0");
        accumulation = long.Parse(GetParameter("Accumulation", ""));
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
        SetParameter("Accumulation", accumulation.ToString());
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

            if (other > 0)
            {
                long l = (long)(other.ToDouble());

                accumulation += l;
                if (accumulation < 0) accumulation = long.MaxValue;

                if (Social.localUser.authenticated)
                {
#if UNITY_IOS
                    Saver.AddScoreToLeaderboard(iphone_leaderboard_id, accumulation);
#elif UNITY_ANDROID
                    Saver.AddScoreToLeaderboard(GPGSIds.leaderboard_rich_man, accumulation);
#endif
                }
            }

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
