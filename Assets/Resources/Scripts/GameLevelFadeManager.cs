using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelFadeManager : MonoBehaviour
{
    public int level;
    public List<GameObject> unlocking;
    public TextManager text;

    private Button btn;
    private int comingSoon = 10000;

    private void Awake()
    {
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
        foreach (GameObject obj in unlocking) obj.SetActive(false);
        if (btn != null) btn.interactable = false;
        if (Island.Instance.Level >= level)
            Unlock();
        EventManager.Subscribe("LevelUp", SetUnlockButton);
    }

    private void SetUnlockButton(object[] arg0)
    {
        if (Island.Instance.Level == level)
        {
            string islandName = Inventory.Instance.lists[level / 25].islandName;
            EventManager.SendEvent("IslandUnlocked", islandName);
        }
        Unlock();
    }

    public void Unlock()
    {
        if (Island.Instance.Level < level) return;
        foreach (GameObject obj in unlocking) obj.SetActive(true);

        Destroy(gameObject);
    }
}
