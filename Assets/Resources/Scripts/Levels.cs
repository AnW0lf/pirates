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
