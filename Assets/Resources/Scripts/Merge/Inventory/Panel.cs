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
    public Sprite sprtStar, sprtLockLevel;

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
            for (int i = 0; island.Level >= levels[i] && i < transform.childCount; i++, count++) ;
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
}
