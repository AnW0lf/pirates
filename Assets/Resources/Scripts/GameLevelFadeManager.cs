using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelFadeManager : MonoBehaviour
{
    public int level;
    public GameObject bonuses, screenUI, wheel, progress;
    public TextManager text;

    private Island island;
    private Button btn;
    private int comingSoon = 10000;

    private void Awake()
    {
        island = Island.Instance();
        btn = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        if (level > comingSoon)
        {
            text.text = "COMING SOON";
            text.prefix = "";
        }
        else
        {
            text.text = level.ToString();
            text.prefix = "Level ";
        }
        wheel.SetActive(false);
        bonuses.SetActive(false);
        screenUI.SetActive(false);
        progress.SetActive(false);
        btn.interactable = false;
        if (island.Level >= level)
            Unlock();
        EventManager.Subscribe("LevelUp", SetUnlockButton);
    }

    private void SetUnlockButton(object[] arg0)
    {
        Unlock();
        /*
        if (island.Level >= level && !btn.interactable)
        {
            btn.interactable = true;
            btn.onClick.AddListener(Unlock);
        }
        */
    }

    public void Unlock()
    {
        if (island.Level < level) return;
        wheel.SetActive(true);
        bonuses.SetActive(true);
        screenUI.SetActive(true);
        progress.SetActive(true);
        Destroy(gameObject);
    }
}
