using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBonus : MonoBehaviour
{
    public int minLevel, maxLevel;
    public BonusGenerator bg;

    private Island island;

    public void GenerateBonuses(int count)
    {
        if (island == null) island = Island.Instance();
        //if (island.Level >= minLevel && island.Level <= maxLevel) bg.RandomBonus(count);
    }
}
