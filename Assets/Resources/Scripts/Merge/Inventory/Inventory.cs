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
            items[i] = null;
            island.InitParameter("ShipCount_" + list.islandNumber.ToString() + "_" + i.ToString(), 0);
            island.InitParameter("ShipAlltimeCount_" + list.islandNumber.ToString() + "_" + i.ToString(), 0);
        }

        Load();
        UpdateBuyButtonInfo();
        DisplayItems(new object[0]);

        EventManager.Subscribe("ChangeMoney", UpdateBuyButtonInteractable);
        EventManager.Subscribe("LevelUp", DisplayItems);
    }

    private void UpdateBuyButtonInteractable(object[] args)
    {
        BigDigit price = list.ships[0].startPrice * (GetShipAlltimeCount(list.islandNumber, 0) + 1);
        bool interactable = price < island.Money && !IsFull && shipsCount < unlockedSlotsCount;
        buyBtn.interactable = interactable;
        UpdateBuyButtonInfo();
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

    private void Update()
    {

    }

    public void Merge(CurrentItem a, CurrentItem b)
    {
        if (list.ships.IndexOf(a.item) < (list.ships.Count - 1))
        {
            ShipInfo item = a.item;
            int id = a.id, newIndex = Mathf.Clamp(list.ships.IndexOf(item) + 1, 0, list.ships.Count - 1);
            Remove(a.id);
            Remove(b.id);
            items[id] = list.ships[newIndex];
            shipsCount++;
            manager.GenerateShips(newIndex, 1);

            SetShipCount(list.islandNumber, newIndex, GetShipCount(list.islandNumber, newIndex) + 1);
            if (GetShipAlltimeCount(list.islandNumber, newIndex) == 0) EventManager.SendEvent("NewShip", list.ships[newIndex]);
            AddShipAlltimeCount(list.islandNumber, newIndex);

            DragHandler.itemBeingDragged.GetComponent<DragHandler>().EndDrag();
            DisplayItems(new object[0]);
        }
        else Switch(a, b);
    }

    public void Switch(CurrentItem a, CurrentItem b)
    {
        ShipInfo item = items[a.id];
        items[a.id] = items[b.id];
        items[b.id] = item;

        DragHandler.itemBeingDragged.GetComponent<DragHandler>().EndDrag();
        DisplayItems(new object[0]);
    }

    public void Add(ShipInfo item)
    {
        if (unlockedSlotsCount > shipsCount)
        {
            for (int i = 0; i < cellContainer.childCount; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    int islandNum = list.islandNumber, shipNum = list.ships.IndexOf(item);
                    shipsCount++;
                    manager.GenerateShips(list.ships.IndexOf(item), 1);
                    DisplayItems(new object[0]);
                    break;
                }
            }
        }
    }

    public void Remove(int id)
    {
        if (shipsCount > 0)
        {
            ShipInfo item = items[id];
            items[id] = null;
            int islandNum = list.islandNumber, shipNum = list.ships.IndexOf(item);
            SetShipCount(islandNum, shipNum, Mathf.Clamp(GetShipCount(islandNum, shipNum) - 1, 0, cellContainer.childCount));
            shipsCount--;
            manager.DestroyShips(list.ships.IndexOf(item), 1);
            DisplayItems(new object[0]);
            UpdateBuyButtonInteractable(new object[0]);
        }
    }

    public void Sell()
    {
        CurrentItem item;
        if (DragHandler.itemBeingDragged
            && (item = DragHandler.itemBeingDragged.transform.parent.GetComponent<CurrentItem>()) && item.item)
        {
            DragHandler.itemBeingDragged.GetComponent<DragHandler>().EndDrag();
            island.ChangeMoney(item.item.startPrice);
            Remove(item.id);
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
        buyBtnTxt.text = (list.ships[0].startPrice * (GetShipAlltimeCount(list.islandNumber, 0) + 1)).ToString() + "[C]";
    }

    private void DisplayItems(object[] args)
    {
        int unlocked = unlockedSlotsCount;
        for (int i = 0; i < items.Length; i++)
        {
            Transform cell = cellContainer.GetChild(i);
            Image icon = cell.GetChild(0).GetComponent<Image>();
            GameObject star = icon.transform.GetChild(0).gameObject;
            cell.GetComponent<CurrentItem>().item = items[i];
            if (i < unlocked)
            {
                if (items[i] != null)
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

    public int GetShipCount(int islandNumber, int shipNumber)
    {
        return island.GetParameter("ShipCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    public int GetShipAlltimeCount(int islandNumber, int shipNumber)
    {
        return island.GetParameter("ShipAlltimeCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    private void AddShipAlltimeCount(int islandNumber, int shipNumber)
    {
        island.SetParameter("ShipAlltimeCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), GetShipAlltimeCount(islandNumber, shipNumber) + 1);
    }

    public void BuyShip(int number)
    {
        int n = Mathf.Clamp(number, 0, list.ships.Count - 1);
        int shipCount = GetShipCount(list.islandNumber, n);
        if (island.ChangeMoney(-list.ships[n].startPrice * (shipCount + 1)))
        {
            Add(list.ships[n]);
            SetShipCount(list.islandNumber, n, Mathf.Clamp(shipCount + 1, 0, cellContainer.childCount));
            if (GetShipAlltimeCount(list.islandNumber, n) == 0) EventManager.SendEvent("NewShip", list.ships[n]);
            AddShipAlltimeCount(list.islandNumber, n);
        }
        if (n == 0) UpdateBuyButtonInfo();
    }
}
