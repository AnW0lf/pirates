using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    private static Island island;
    //_______________________________________________________________________________
    private int money;
    //_______________________________________________________________________________
    private Island() {}

    public static Island Instance()
    {
        if (island == null) island = new Island();
        return island;
    }

    public bool ChangeMoney(int value)
    {
        if (money + value >= 0)
        {
            money += value;
            return true;
        }
        return false;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetLevel()
    {
        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        return PlayerPrefs.GetInt("Level");
    }
}
