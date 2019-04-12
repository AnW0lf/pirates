using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankController : MonoBehaviour
{
    public Image progress;
    public Text rankNumber, rankName;

    public string[] names;
    public List<int> levels;

    private Island island;
    private int currentRank = 0;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);
        InitInfo();
    }

    private void InitInfo()
    {
        for(int i = island.Level; i >= 0; i--)
        {
            if (levels.Contains(i))
            {
                currentRank = levels.IndexOf(i);
                rankName.text = names[currentRank];
                rankNumber.text = "Rank " + (currentRank + 1);
                break;
            }
        }
        if (levels.Count > currentRank + 1)
        {
            progress.fillAmount = ((float)(island.Level - levels[currentRank]) / (levels[currentRank + 1] - levels[currentRank]));
        }
        else
        {
            progress.fillAmount = 1f;
        }
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (levels.Contains(island.Level))
        {
            currentRank = levels.IndexOf(island.Level);
            rankName.text = names[currentRank];
            rankNumber.text = "Rank " + (currentRank + 1);
        }
        if (levels.Count > currentRank + 1)
        {
            progress.fillAmount = ((float)(island.Level - levels[currentRank]) / (levels[currentRank + 1] - levels[currentRank]));
        }
        else
        {
            progress.fillAmount = 1f;
        }
    }
}
