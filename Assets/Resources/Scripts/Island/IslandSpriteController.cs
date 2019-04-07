using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandSpriteController : MonoBehaviour
{
    public List<int> levels;
    public Sprite[] sprites;

    private Image image;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
        image.GetComponent<Image>();
    }

    private void Start()
    {
        EventManager.Subscribe("LevelUp", UpdateInfo);
        InitInfo();
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (levels.Contains(island.Level) && sprites.Length > levels.IndexOf(island.Level))
        {
            image.sprite = sprites[levels.IndexOf(island.Level)];
        }
    }

    private void InitInfo()
    {
        if (levels.Count > 0 && sprites.Length > 0)
        {
            bool setted = false;
            for(int i = island.Level; i > 0; i--)
            {
                if(levels.Contains(i) && sprites.Length > levels.IndexOf(i))
                {
                    image.sprite = sprites[levels.IndexOf(i)];
                    setted = true;
                }
            }
            if (!setted)
            {
                image.sprite = sprites[0];
            }
        }
    }
}
