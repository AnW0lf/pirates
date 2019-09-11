using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSpriteController : MonoBehaviour
{
    public List<int> levels;
    public Sprite[] sprites;

    private Island island;
    private Image image;

    private void Awake()
    {
        island = Island.Instance;
        image = GetComponent<Image>();
    }

    void Start()
    {
        InitSprite();
        EventManager.Subscribe("LevelUp", UpdateSprite);
    }

    private void UpdateSprite(object[] arg0)
    {
        if (sprites.Length != levels.Count
            || sprites.Length == 0
            || levels.Count == 0) return;
        int level = island.Level;
        if (levels.Contains(level))
            image.sprite = sprites[levels.IndexOf(level)];
    }

    private void InitSprite()
    {
        if (sprites.Length != levels.Count
            || sprites.Length == 0
            || levels.Count == 0) return;
        int level = island.Level, i;
        for (i = level; !levels.Contains(i) && i != 0; i--) ;
        image.sprite = sprites[levels.IndexOf(i)];
    }
}
