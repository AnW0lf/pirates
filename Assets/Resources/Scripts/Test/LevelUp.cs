using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    private Global global;

    public int minLevel, maxLevel;
    public GameObject levelUpButton, progressBar;
    public Text levelText;

    private void Awake()
    {
        global = Global.Instance;
    }

    private void OnEnable()
    {
        EventManager.Subscribe("AddMaterial", Up);
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("AddMaterial", Up);
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
    }

    private void Start()
    {
        UpdateInfo(new object[0]);
    }

    private void UpdateInfo(object[] arg0)
    {
        if (global.Level >= minLevel && global.Level <= maxLevel)
        {
            levelText.text = "Level : " + (global.Level - minLevel) + "/" + (maxLevel - minLevel);
            if (!progressBar.activeInHierarchy)
                progressBar.SetActive(true);
        }
        else if (progressBar.activeInHierarchy)
        {
            progressBar.SetActive(false);
        }
    }

    private void Up(object[] parameters)
    {
        if (global.Level >= minLevel && global.Level <= maxLevel && global.Material >= global.GetMaxMaterial())
        {
            levelUpButton.SetActive(true);
        }
    }
}
