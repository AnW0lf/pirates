using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public GameObject levelProgress, levelUp;
    public Image levelBar;
    public Text levelNumber;
    public int minLevel, maxLevel;

    private Island island;
    private int maxExp;

    private void Awake()
    {
        island = Island.Instance();
    }

    void Start()
    {
        Load();
    }

    void Update()
    {
        levelNumber.text = "Level " + island.Level;
        if(island.Level < minLevel || island.Level > maxLevel)
        {
            if (levelProgress.activeInHierarchy)
                levelProgress.SetActive(false);
            if (levelUp.activeInHierarchy)
                levelUp.SetActive(false);
            return;
        }

        if (island.Exp < maxExp)
        {
            levelUp.SetActive(false);
            levelProgress.SetActive(true);
            levelBar.fillAmount = (float)island.Exp / maxExp;
        }
        else
        {
            levelProgress.SetActive(false);
            levelUp.SetActive(true);
        }
    }

    private void Load()
    {
        maxExp = island.GetMaxExp();
    }

    public void LevelUp()
    {
        island.LevelUp();
        Load();
    }

    public void Resetting()
    {
        island.Resetting();
        Load();
    }
}
