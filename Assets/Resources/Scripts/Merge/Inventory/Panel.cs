using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [HideInInspector]
    public ShipInfoList list;

    [Header("Sprites")]
    public Sprite lockSprite;

    [Header("Cells levels")]
    public int[] levels;
    public Sprite sprtStar;

    public int shipsCount { get; set; }
    public ShipInfo[] items { get; private set; }
    private Inventory inventory;
    private Island island;

    private void Awake()
    {
        items = new ShipInfo[transform.childCount];
        for(int i = 0; i < items.Length; i++)
        {
            items[i] = null;
        }
    }

    private void Start()
    {
        inventory = Inventory.Instance;
        island = Island.Instance;
    }

    public int unlockedSlotsCount
    {
        get
        {
            int count = 0;
            for (int i = 0; island.Level >= levels[i] && i < levels.Length; i++, count++) ;
            return Mathf.Clamp(count, 0, transform.childCount);
        }
    }

    public bool IsFull
    {
        get
        {
            int unlocked = unlockedSlotsCount;
            for (int i = 0; i < items.Length; i++)
            {
                if (i < unlocked && items[i] == null)
                    return false;
            }
            return true;
        }
    }

    public void DisplayItems()
    {
        int unlocked = unlockedSlotsCount;
        Vector2 cellSize = transform.GetComponent<GridLayoutGroup>().cellSize;
        for (int i = 0; i < items.Length; i++)
        {
            Transform cell = transform.GetChild(i);
            Image icon = cell.GetChild(0).GetComponent<Image>();
            Text unlockLvlTxt = cell.GetChild(1).GetComponent<Text>();
            GameObject star = icon.transform.GetChild(0).gameObject;
            Image starImg = star.GetComponent<Image>();
            Text level = star.transform.GetComponentInChildren<Text>();
            cell.GetComponent<CurrentItem>().item = items[i];
            if (i < unlocked)
            {
                if (items[i] != null)
                {
                    icon.enabled = true;
                    icon.sprite = items[i].icon;
                    starImg.sprite = sprtStar;
                    float iconY = cellSize.x * 0.85f, iconX = iconY * ((float)icon.sprite.texture.width / (float)icon.sprite.texture.height);
                    icon.rectTransform.sizeDelta = new Vector2(iconX, iconY);
                    icon.rectTransform.anchoredPosition = Vector2.zero;
                    icon.GetComponent<DragHandler>().canDrag = true;
                    star.SetActive(true);
                    level.text = items[i].gradeLevel.ToString();
                    unlockLvlTxt.enabled = false;
                }
                else
                {
                    icon.rectTransform.anchoredPosition = Vector2.zero;
                    icon.enabled = false;
                    icon.GetComponent<DragHandler>().canDrag = false;
                    star.SetActive(false);
                    unlockLvlTxt.enabled = false;
                }
            }
            else
            {
                icon.enabled = true;

                icon.sprite = lockSprite;
                icon.GetComponent<DragHandler>().canDrag = false;
                star.SetActive(false);

                if (i == unlocked)
                {
                    icon.rectTransform.sizeDelta = cellSize * 0.6f;
                    unlockLvlTxt.enabled = true;
                    unlockLvlTxt.text = "Level " + levels[i].ToString();
                    icon.rectTransform.anchoredPosition = Vector2.up * 13f;
                }
                else
                {
                    unlockLvlTxt.enabled = false;
                    icon.rectTransform.sizeDelta = cellSize * 0.85f;
                    icon.rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }
    }
}
