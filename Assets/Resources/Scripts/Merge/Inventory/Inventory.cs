using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform gameFieldScrollRect, panelRect;
    public ShipInfoList[] lists;
    public Panel[] panels;
    public ShipsManager[] managers;

    [Header("Flags")]
    public GameObject mainFlag;
    public GameObject additionFlag;

    [Header("Buttons")]
    public Button buyBtn;
    public Text buyBtnTxt;
    public Button shopBtn;
    public int shopBtnMinLvl = 3;
    public GameObject sellBtn;

    [Header("Windows")]
    public WindowNewSlot newSlotWindow;

    public Panel selectedPanel { get; private set; }

    private Island island;
    private int selectedGameFieldNumber = -1;
    private bool switching = false;
    private Vector2 panelRectNewPos;
    private float switchSpeed = 3000f;

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;

        for (int i = 0; i < panels.Length && i < lists.Length; i++)
            panels[i].list = lists[i];
    }

    private void Start()
    {
        island = Island.Instance;

        CheckSelectedPanel();
        Load();

        UpdateBuyButtonInfo();
        DisplayItems(new object[0]);
        LevelUpChanges();

        EventManager.Subscribe("ChangeMoney", UpdateBuyButtonInteractable);
        EventManager.Subscribe("ChangeMoney", UpdateFlagsState);
        EventManager.Subscribe("LevelUp", DisplayItems);
        EventManager.Subscribe("LevelUp", CheckNewSlot);
    }

    private void Update()
    {
        CheckSelectedPanel();
        SwitchPanel();
    }

    private void CheckSelectedPanel()
    {
        int n = (gameFieldScrollRect.childCount - 1) - Mathf.Clamp((int)Mathf.Abs((gameFieldScrollRect.anchoredPosition.y + ((float)Screen.height / 2f))
            / gameFieldScrollRect.sizeDelta.y * gameFieldScrollRect.childCount), 0, gameFieldScrollRect.childCount - 1);

        if (n != selectedGameFieldNumber)
        {
            selectedGameFieldNumber = n;
            selectedPanel = panels[Mathf.Clamp(n, 0, panels.Length - 1)];

            BeginSwitchPanel(selectedGameFieldNumber);
            DisplayItems(new object[0]);
        }
    }

    private void BeginSwitchPanel(int number)
    {
        switching = true;
        float newX = -(panelRect.sizeDelta.x / panels.Length * number);
        panelRectNewPos = new Vector2(newX, panelRect.anchoredPosition.y);
    }

    private void SwitchPanel()
    {
        if (switching)
        {
            panelRect.anchoredPosition = Vector2.MoveTowards(panelRect.anchoredPosition, panelRectNewPos, Time.deltaTime * switchSpeed);
            if (panelRect.anchoredPosition.x == panelRectNewPos.x) switching = false;
        }
    }

    private void LevelUpChanges()
    {
        if (island.Level >= shopBtnMinLvl)
        {
            if (!shopBtn.gameObject.activeSelf)
                shopBtn.gameObject.SetActive(true);
            if (!sellBtn.activeSelf)
                sellBtn.SetActive(true);
        }
        else
        {
            if (shopBtn.gameObject.activeSelf)
                shopBtn.gameObject.SetActive(false);
            if (sellBtn.activeSelf)
                sellBtn.SetActive(false);
        }
    }

    private void UpdateBuyButtonInteractable(object[] args)
    {
        BigDigit price = selectedPanel.list.ships[0].price * (GetShipAlltimeCount(selectedPanel.list.islandNumber, 0) + 1);
        bool interactable = price < island.Money && !selectedPanel.IsFull && selectedPanel.shipsCount < unlockedSlotsCount;
        buyBtn.interactable = interactable;
        UpdateBuyButtonInfo();
    }

    private void CheckNewSlot(object[] args)
    {
        for (int p = 0; p < panels.Length; p++)
        {
            for (int i = 0; i < panels[p].levels.Length; i++)
            {
                if (panels[p].levels[i] == island.Level)
                {
                    newSlotWindow.Activate = true;
                    return;
                }
                if (panels[p].levels[i] > island.Level) return;
            }
        }
        LevelUpChanges();
    }


    private void UpdateFlagsState(object[] args)
    {
        if (!selectedPanel.IsFull && island.Money >= (GetShipAlltimeCount(selectedPanel.list.islandNumber, 0) + 1) * selectedPanel.list.ships[0].price)
        {
            if (!mainFlag.activeSelf) mainFlag.SetActive(true);
            int max = selectedPanel.list.ships.Count - 1;
            for (int i = 1; i < selectedPanel.list.ships.Count; i++)
            {
                if (CheckShipUnlocked(selectedPanel.list.islandNumber, Mathf.Clamp(i, 0, max))
                    && CheckShipUnlocked(selectedPanel.list.islandNumber, Mathf.Clamp(i + 2, 0, max))
                    && island.Money >= (GetShipAlltimeCount(selectedPanel.list.islandNumber, i) + 1) * selectedPanel.list.ships[i].price)
                {
                    if (!additionFlag.activeSelf) additionFlag.SetActive(true);
                    return;
                }
            }
            if (additionFlag.activeSelf) additionFlag.SetActive(false);
        }
        else
        {
            if (mainFlag.activeSelf) mainFlag.SetActive(false);
            if (additionFlag.activeSelf) additionFlag.SetActive(false);
        }
    }

    public void Merge(CurrentItem a, CurrentItem b)
    {
        if (a.id == b.id) return;
        if (selectedPanel.list.ships.IndexOf(a.item) < (selectedPanel.list.ships.Count - 1))
        {
            ShipInfo item = a.item;
            int id = a.id, newIndex = Mathf.Clamp(selectedPanel.list.ships.IndexOf(item) + 1, 0, selectedPanel.list.ships.Count - 1);
            Remove(a.id);
            Remove(b.id);
            selectedPanel.items[id] = selectedPanel.list.ships[newIndex];
            selectedPanel.shipsCount++;
            managers[selectedGameFieldNumber].GenerateShips(newIndex, 1);

            SetShipCount(selectedPanel.list.islandNumber, newIndex, GetShipCount(selectedPanel.list.islandNumber, newIndex) + 1);
            if (GetShipAlltimeCount(selectedPanel.list.islandNumber, newIndex) == 0) EventManager.SendEvent("NewShip", selectedPanel.list.ships[newIndex]);
            AddShipAlltimeCount(selectedPanel.list.islandNumber, newIndex);
            AddShipUnlocked(selectedPanel.list.islandNumber, newIndex);


            DragHandler.itemBeingDragged.GetComponent<DragHandler>().EndDrag();
            DisplayItems(new object[0]);
        }
        else Switch(a, b);
    }

    public void Switch(CurrentItem a, CurrentItem b)
    {
        ShipInfo item = selectedPanel.items[a.id];
        selectedPanel.items[a.id] = selectedPanel.items[b.id];
        selectedPanel.items[b.id] = item;

        DragHandler.itemBeingDragged.GetComponent<DragHandler>().EndDrag();
        DisplayItems(new object[0]);
    }

    public void Add(int panelNumber, ShipInfo item)
    {
        if (unlockedSlotsCount > panels[panelNumber].shipsCount)
        {
            for (int i = 0; i < panels[panelNumber].transform.childCount; i++)
            {
                if (panels[panelNumber].items[i] == null)
                {
                    panels[panelNumber].items[i] = item;
                    int islandNum = panels[panelNumber].list.islandNumber, shipNum = panels[panelNumber].list.ships.IndexOf(item);
                    panels[panelNumber].shipsCount++;
                    managers[panelNumber].GenerateShips(panels[panelNumber].list.ships.IndexOf(item), 1);
                    DisplayItems(new object[0]);
                    break;
                }
            }
        }
    }

    public void Remove(int id)
    {
        if (selectedPanel.shipsCount > 0)
        {
            ShipInfo item = selectedPanel.items[id];
            selectedPanel.items[id] = null;
            int islandNum = selectedPanel.list.islandNumber, shipNum = selectedPanel.list.ships.IndexOf(item);
            SetShipCount(islandNum, shipNum, Mathf.Clamp(GetShipCount(islandNum, shipNum) - 1, 0, selectedPanel.transform.childCount));
            selectedPanel.shipsCount--;
            managers[selectedGameFieldNumber].DestroyShips(item.gradeLevel, 1);
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
            island.ChangeMoney(item.item.price);
            Remove(item.id);
        }
    }

    public int unlockedSlotsCount
    {
        get
        {
            int count = 0;
            for (int i = 0; island.Level >= selectedPanel.levels[i] && i < selectedPanel.transform.childCount; i++, count++) ;
            return Mathf.Clamp(count, 2, selectedPanel.transform.childCount);
        }
    }

    private void UpdateBuyButtonInfo()
    {
        buyBtnTxt.text = (selectedPanel.list.ships[0].price * (GetShipAlltimeCount(selectedPanel.list.islandNumber, 0) + 1)).ToString() + "[C]";
    }

    private void DisplayItems(object[] args)
    {
        foreach (Panel p in panels)
        {
            int unlocked = p.unlockedSlotsCount;
            Vector2 cellSize = p.transform.GetComponent<GridLayoutGroup>().cellSize;
            for (int i = 0; i < p.items.Length; i++)
            {
                Transform cell = p.transform.GetChild(i);
                Image icon = cell.GetChild(0).GetComponent<Image>();
                Text unlockLvlTxt = cell.GetChild(1).GetComponent<Text>();
                GameObject star = icon.transform.GetChild(0).gameObject;
                Image starImg = star.GetComponent<Image>();
                Text level = star.transform.GetComponentInChildren<Text>();
                cell.GetComponent<CurrentItem>().item = p.items[i];
                if (i < unlocked)
                {
                    if (p.items[i] != null)
                    {
                        icon.enabled = true;
                        icon.sprite = p.items[i].icon;
                        starImg.sprite = p.sprtStar;
                        float iconY = cellSize.x * 0.85f, iconX = iconY * ((float)icon.sprite.texture.width / (float)icon.sprite.texture.height);
                        icon.rectTransform.sizeDelta = new Vector2(iconX, iconY);
                        icon.rectTransform.anchoredPosition = Vector2.zero;
                        icon.GetComponent<DragHandler>().canDrag = true;
                        star.SetActive(true);
                        level.text = p.items[i].gradeLevel.ToString();
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

                    icon.sprite = p.lockSprite;
                    icon.GetComponent<DragHandler>().canDrag = false;
                    star.SetActive(false);

                    if (i == unlocked)
                    {
                        icon.rectTransform.sizeDelta = cellSize * 0.6f;
                        unlockLvlTxt.enabled = true;
                        unlockLvlTxt.text = "Level " + p.levels[i].ToString();
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

    private void Load()
    {
        for (int p = 0; p < panels.Length; p++)
        {
            for (int i = 0; i < panels[p].items.Length; i++)
            {
                for (int j = 0; j < GetShipCount(panels[p].list.islandNumber, i); j++)
                {
                    Add(panels[p].list.islandNumber - 1, panels[p].list.ships[i]);
                }
            }
        }
    }

    public void SetShipCount(int islandNumber, int shipNumber, int value)
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

    public void AddShipAlltimeCount(int islandNumber, int shipNumber)
    {
        island.SetParameter("ShipAlltimeCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), GetShipAlltimeCount(islandNumber, shipNumber) + 1);
    }

    public bool CheckShipUnlocked(int islandNumber, int shipNumber)
    {
        return island.GetParameter("ShipUnlocked_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0) != 0;
    }

    public void AddShipUnlocked(int islandNumber, int shipNumber)
    {
        island.SetParameter("ShipUnlocked_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 1);
    }

    public void BuyShip(int number)
    {
        int n = Mathf.Clamp(number, 0, selectedPanel.list.ships.Count - 1);
        int shipCount = GetShipCount(selectedPanel.list.islandNumber, n);
        if (island.ChangeMoney(-selectedPanel.list.ships[n].price * (shipCount + 1)))
        {
            Add(selectedPanel.list.islandNumber - 1, selectedPanel.list.ships[n]);
            SetShipCount(selectedPanel.list.islandNumber, n, Mathf.Clamp(shipCount + 1, 0, selectedPanel.transform.childCount));
            if (GetShipAlltimeCount(selectedPanel.list.islandNumber, n) == 0) EventManager.SendEvent("NewShip", selectedPanel.list.ships[n]);
            AddShipAlltimeCount(selectedPanel.list.islandNumber, n);
            AddShipUnlocked(selectedPanel.list.islandNumber, n);
        }
        if (n == 0) UpdateBuyButtonInfo();
    }


}
