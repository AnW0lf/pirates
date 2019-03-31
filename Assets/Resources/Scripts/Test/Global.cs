using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
    private static Global global;
    //_______________________________________________________________________________
    public int Money { get; private set; }
    public int Level { get; private set; }
    public int Material { get; private set; }
    public int StartMaterial { get; private set; }
    public float MaterialModifier { get; private set; }

    private List<string> parameters;
    //_______________________________________________________________________________
    private Global()
    {
        parameters = new List<string>();
        Load();
    }

    public static Global Instance
    {
        get
        {
            if (global == null)
            {
                global = new Global();
            }
            return global;
        }
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("Money"))
            PlayerPrefs.SetInt("Money", 0);
        Money = PlayerPrefs.GetInt("Money");

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        Level = PlayerPrefs.GetInt("Level");

        if (!PlayerPrefs.HasKey("Material"))
            PlayerPrefs.SetInt("Material", 0);
        Material = PlayerPrefs.GetInt("Material");

        if (!PlayerPrefs.HasKey("StartMaterial"))
            PlayerPrefs.SetInt("StartMaterial", 10);
        StartMaterial = PlayerPrefs.GetInt("StartMaterial");

        if (!PlayerPrefs.HasKey("MaterialModifier"))
            PlayerPrefs.SetFloat("MaterialModifier", 1.2f);
        MaterialModifier = PlayerPrefs.GetFloat("MaterialModifier");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("Material", Material);
        PlayerPrefs.SetInt("StartMaterial", StartMaterial);
        PlayerPrefs.SetFloat("MaterialModifier", MaterialModifier);
    }

    public void SetSettings(int money, int level, int material, int startMaterial, float materialModifier)
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Material", material);
        PlayerPrefs.SetInt("StartMaterial", startMaterial);
        PlayerPrefs.SetFloat("MaterialModifier", materialModifier);
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

    public bool AddMaterial(int value)
    {
        if (Material + value >= 0)
        {
            Material += value;
            return true;
        }
        return false;
    }

    public int GetMaxMaterial()
    {
        if (Level <= 10)
            return (int)(StartMaterial * Mathf.Pow(MaterialModifier, Level));
        else
            return (int)(StartMaterial * Mathf.Pow(MaterialModifier, 11));
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

    public void MaterialUp(int exp)
    {
        Material += exp;
        Save();
    }

    public void LevelUp()
    {
        Level++; Material = 0;
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
