using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ShipInfoList list;
    public Transform cellContainer;
    public ShipsManager manager;

    [Header("Sprites")]
    public Sprite lockSprite;

    [Header("Buttons")]
    public Button buyBtn;
    public Text buyBtnTxt;

    ShipInfo[] items;
    Island island;
    int shipsCount = 0;

    private void Start()
    {
        island = Island.Instance;

        items = new ShipInfo[cellContainer.childCount];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new ShipInfo();
            island.InitParameter("ShipCount_" + list.islandNumber.ToString() + "_" + i.ToString(), 0);
        }

        Load();
        UpdateBuyButtonInfo();
        DisplayItems();

        EventManager.Subscribe("ChangeMoney", UpdateBuyButtonInteractable);
    }

    private void UpdateBuyButtonInteractable(object[] args)
    {
        BigDigit price = list.ships[0].startPrice * (GetShipCount(list.islandNumber, 0) + 1);
        bool interactable = price < island.Money && !IsFull && shipsCount < unlockedSlotsCount;
        buyBtn.interactable = interactable;
    }

    public bool IsFull
    {
        get
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == ShipInfo.defaultName)
                    return false;
            }
            return true;
        }
    }

    private void Update()
    {

    }

    public void Add(ShipInfo item)
    {
        if (unlockedSlotsCount > shipsCount)
        {
            for (int i = 0; i < cellContainer.childCount; i++)
            {
                if (items[i].name == ShipInfo.defaultName)
                {
                    items[i] = item;
                    shipsCount++;
                    DisplayItems();
                    manager.GenerateShips(list.ships.IndexOf(item), 1);
                    break;
                }
            }
        }
    }

    private int unlockedSlotsCount
    {
        get
        {
            return Mathf.Clamp(island.Level - ((list.islandNumber - 1) * 25), 2, cellContainer.childCount);
        }
    }

    private void UpdateBuyButtonInfo()
    {
        buyBtnTxt.text = (list.ships[0].startPrice * (GetShipCount(list.islandNumber, 0) + 1)).ToString() + "[C]";
    }

    private void DisplayItems()
    {
        int unlocked = unlockedSlotsCount;
        for (int i = 0; i < items.Length; i++)
        {
            Transform cell = cellContainer.GetChild(i);
            Image icon = cell.GetChild(0).GetComponent<Image>();
            GameObject star = icon.transform.GetChild(0).gameObject;
            if (i < unlocked)
            {

                if (items[i].name != ShipInfo.defaultName)
                {
                    Text level = star.transform.GetComponentInChildren<Text>();
                    icon.enabled = true;
                    icon.sprite = items[i].icon;
                    icon.GetComponent<DragHandler>().canDrag = true;
                    star.SetActive(true);
                    level.text = items[i].gradeLevel.ToString();
                }
                else
                {
                    icon.enabled = false;
                    icon.GetComponent<DragHandler>().canDrag = false;
                    star.SetActive(false);
                }
            }
            else
            {
                icon.enabled = true;
                icon.sprite = lockSprite;
                icon.GetComponent<DragHandler>().canDrag = false;
                star.SetActive(false);
            }
        }
    }

    private void Load()
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < GetShipCount(list.islandNumber, i); j++)
            {
                Add(list.ships[i]);
            }
        }
    }

    private void SetShipCount(int islandNumber, int shipNumber, int value)
    {
        island.SetParameter("ShipCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), value);
    }

    private int GetShipCount(int islandNumber, int shipNumber)
    {
        return island.GetParameter("ShipCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    public void BuyShip(int number)
    {
        int n = Mathf.Clamp(number, 0, list.ships.Count - 1);
        int shipCount = GetShipCount(list.islandNumber, n);
        if (island.ChangeMoney(-list.ships[n].startPrice * (shipCount + 1)))
        {
            SetShipCount(list.islandNumber, n, shipCount + 1);
            Add(list.ships[n]);
        }
        if (n == 0) UpdateBuyButtonInfo();
    }
}
