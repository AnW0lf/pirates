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

    private void Awake()
    {
        island = Island.Instance();
        btn = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        text.text = level.ToString();
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
        if (island.Level >= level && !btn.interactable)
        {
            btn.interactable = true;
            btn.onClick.AddListener(Unlock);
        }
    }

    private void Unlock()
    {
        wheel.SetActive(true);
        bonuses.SetActive(true);
        screenUI.SetActive(true);
        progress.SetActive(true);
        Destroy(gameObject);
    }
}
