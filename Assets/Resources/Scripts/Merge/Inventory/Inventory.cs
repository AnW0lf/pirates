using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public RectTransform gameFieldScrollRect, panelRect;
    public ShipInfoList[] lists;
    public List<Panel> panels;
    public ShipsManager[] managers;

    [Header("Flags")]
    public GameObject additionFlag;

    [Header("Buttons")]
    public Button buyBtn;
    public Text buyBtnTxt, buyBtnTitleTxt;
    public Button shopBtn;
    public int shopBtnMinLvl = 3;
    public GameObject sellBtn;

    [Header("Windows")]
    public WindowNewSlot newSlotWindow;

    public Panel selectedPanel { get; private set; }
    public List<int> currentShips { get; private set; }

    private int selectedGameFieldNumber = -1;
    private bool switching = false;
    private Vector2 panelRectNewPos;
    private float switchSpeed = 3000f;
    private Animator buyBtnAnim;

    private readonly string[] romans = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    public static Inventory Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;

        currentShips = new List<int>();

        for (int i = 0; i < panels.Count && i < lists.Length; i++)
            panels[i].list = lists[i];
        if (panels.Count > 0)
            selectedPanel = panels[0];
        else Debug.LogWarning("The array of panels is empty, but this should not be");

        buyBtnAnim = buyBtn.GetComponent<Animator>();
    }

    private void Start()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            currentShips.Add(Island.Instance.GetParameter("CurrentShipNumber_" + panels[i].list.islandName, 0));
        }

        Load();

        //UpdateBuyButtonInfo();
        DisplayItems(new object[0]);
        LevelUpChanges(new object[0]);

        EventManager.Subscribe("ChangeMoney", UpdateBuyButtonInteractable);
        EventManager.Subscribe("ChangeMoney", UpdateFlagsState);
        EventManager.Subscribe("LevelUp", DisplayItems);
        EventManager.Subscribe("LevelUp", CheckNewSlot);
        EventManager.Subscribe("LevelUp", LevelUpChanges);
        EventManager.Subscribe("LevelUp", UpdateBuyButtonInfoOnLevelUp);
    }

    private void Update()
    {
        SwitchPanel();
    }

    private void LateUpdate()
    {
        CheckSelectedPanel();
    }

    private void CheckSelectedPanel()
    {
        int n = (gameFieldScrollRect.childCount - 1) - Mathf.Clamp((int)Mathf.Abs((gameFieldScrollRect.anchoredPosition.y + ((float)Screen.height / 2f))
            / gameFieldScrollRect.sizeDelta.y * gameFieldScrollRect.childCount), 0, gameFieldScrollRect.childCount - 1);

        if (n != selectedGameFieldNumber)
        {
            selectedGameFieldNumber = n;
            selectedPanel = panels[Mathf.Clamp(n, 0, panels.Count - 1)];

            BeginSwitchPanel(selectedGameFieldNumber);
            DisplayItems(new object[0]);
        }
    }

    private void BeginSwitchPanel(int number)
    {
        switching = true;

        bool btnActivate = (panels.IndexOf(selectedPanel) * 25 + shopBtnMinLvl) <= Island.Instance.Level;
        shopBtn.gameObject.SetActive(btnActivate);

        float newX = -(panelRect.sizeDelta.x / panels.Count * number);
        panelRectNewPos = new Vector2(newX, panelRect.anchoredPosition.y);
    }

    private void SwitchPanel()
    {
        if (switching)
        {
            panelRect.anchoredPosition = Vector2.MoveTowards(panelRect.anchoredPosition, panelRectNewPos, Time.deltaTime * switchSpeed);
            if (panelRect.anchoredPosition.x == panelRectNewPos.x)
            {
                switching = false;
                UpdateBuyButtonInfo();
            }
        }
    }

    private void LevelUpChanges(object[] args)
    {
        if (Island.Instance.Level >= shopBtnMinLvl)
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
        bool interactable = GetShipPrice(selectedPanel.list, currentShips[panels.IndexOf(selectedPanel)]) < Island.Instance.Money && !selectedPanel.IsFull
            && selectedPanel.shipsCount < selectedPanel.unlockedSlotsCount;
        buyBtn.interactable = interactable;
        UpdateBuyButtonInfo();
    }

    private void CheckNewSlot(object[] args)
    {
        for (int p = 0; p < panels.Count; p++)
        {
            for (int i = 0; i < panels[p].levels.Length; i++)
            {
                if (panels[p].levels[i] == Island.Instance.Level)
                {
                    newSlotWindow.Activate = true;
                    return;
                }
                if (panels[p].levels[i] > Island.Instance.Level) return;
            }
        }
    }


    private void UpdateFlagsState(object[] args)
    {
        if (!selectedPanel.IsFull)
        {
            if (Island.Instance.Level < 6)
            {
                if (Island.Instance.Money >= GetShipPrice(selectedPanel.list, currentShips[panels.IndexOf(selectedPanel)]))
                {
                    if (!buyBtnAnim.GetBool("Pulse")) buyBtnAnim.SetBool("Pulse", true);
                }
                else
                {
                    if (buyBtnAnim.GetBool("Pulse")) buyBtnAnim.SetBool("Pulse", false);
                }
            }

            int max = selectedPanel.list.ships.Count - 1;
            for (int i = 1; i < selectedPanel.list.ships.Count; i++)
            {
                if (CheckShipUnlocked(selectedPanel.list.islandNumber, Mathf.Clamp(i, 0, max))
                    && CheckShipUnlocked(selectedPanel.list.islandNumber, Mathf.Clamp(i + 2, 0, max))
                    && Island.Instance.Money >= GetShipPrice(selectedPanel.list, i))
                {
                    if (!additionFlag.activeSelf) additionFlag.SetActive(true);
                    return;
                }
            }
            if (additionFlag.activeSelf) additionFlag.SetActive(false);
        }
        else
        {
            if (additionFlag.activeSelf) additionFlag.SetActive(false);
            if (buyBtnAnim.GetBool("Pulse")) buyBtnAnim.SetBool("Pulse", false);
        }
    }

    public void Merge(CurrentItem a, CurrentItem b)
    {
        if (!a || !b || !a.item || !b.item || !selectedPanel.list.ships.Contains(a.item) || !selectedPanel.list.ships.Contains(b.item) || a.id == b.id) return;
        if (selectedPanel.list.ships.IndexOf(a.item) < (selectedPanel.list.ships.Count - 1))
        {
            ShipInfo item = a.item;
            int id = a.id, newIndex = Mathf.Clamp(selectedPanel.list.ships.IndexOf(item) + 1, 0, selectedPanel.list.ships.Count - 1);
            Remove(a.id);
            Remove(b.id);
            selectedPanel.items[id] = selectedPanel.list.ships[newIndex];
            selectedPanel.shipsCount++;
            managers[selectedGameFieldNumber].GenerateShips(newIndex, 1);

            EventManager.SendEvent("ShipMerged", selectedPanel.list.ships[newIndex].name);

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

    public void Add(int panelNumber, ShipInfo item, bool free)
    {
        if (!panels[panelNumber].IsFull && panels[panelNumber].unlockedSlotsCount > panels[panelNumber].shipsCount)
        {
            for (int i = 0; i < panels[panelNumber].transform.childCount; i++)
            {
                if (panels[panelNumber].items[i] == null)
                {
                    panels[panelNumber].items[i] = item;
                    panels[panelNumber].addedSlotIndex = i;
                    int islandNum = panels[panelNumber].list.islandNumber, shipNum = panels[panelNumber].list.ships.IndexOf(item);
                    panels[panelNumber].shipsCount++;
                    managers[panelNumber].GenerateShips(panels[panelNumber].list.ships.IndexOf(item), 1);

                    if (free)
                    {
                        SetShipCount(islandNum, shipNum, Mathf.Clamp(GetShipCount(islandNum, shipNum) + 1, 0, panels[panelNumber].transform.childCount));
                        if (GetShipAlltimeCount(islandNum, shipNum) == 0) EventManager.SendEvent("NewShip", item);
                        AddShipAlltimeCount(islandNum, shipNum);
                        AddShipUnlocked(islandNum, shipNum);
                    }

                    panels[panelNumber].DisplayItems(free);
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
            selectedPanel.DisplayItems(false);
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
            Island.Instance.ChangeMoney(item.item.price);

            EventManager.SendEvent("ShipSold", item.item.name);

            Remove(item.id);
        }
    }

    private void UpdateBuyButtonInfo()
    {
        int n = currentShips[panels.IndexOf(selectedPanel)];
        buyBtnTxt.text = GetShipPrice(selectedPanel.list, n).ToString();
        buyBtnTitleTxt.text = "Buy Ship " + romans[n];
    }

    private void UpdateBuyButtonInfoOnLevelUp(object[] args)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            int n = currentShips[i], next = Mathf.Clamp(n + 1, n, panels[i].list.ships.Count - 1), islandNumber = panels[i].list.islandNumber;

            if (CheckShipUnlocked(islandNumber, next))
            {
                BigDigit price = GetShipPrice(panels[i].list, n)
                    , nextPrice = GetShipPrice(panels[i].list, next);

                if (price * 1.3f > nextPrice)
                {
                    currentShips[i] = n + 1;
                    Island.Instance.SetParameter("CurrentShipNumber_" + panels[i].list.islandName, currentShips[i]);
                }
            }
        }

        UpdateBuyButtonInfo();
    }

    private void DisplayItems(object[] args)
    {
        foreach (Panel p in panels)
        {
            p.DisplayItems(false);
        }
    }

    private void Load()
    {
        for (int p = 0; p < panels.Count; p++)
        {
            for (int i = 0; i < panels[p].items.Length; i++)
            {
                for (int j = 0; j < GetShipCount(panels[p].list.islandNumber, i); j++)
                {
                    Add(panels[p].list.islandNumber - 1, panels[p].list.ships[i], false);
                }
            }
        }
    }

    public void SetShipCount(int islandNumber, int shipNumber, int value)
    {
        Island.Instance.SetParameter("ShipCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), value);
    }

    public int GetShipCount(int islandNumber, int shipNumber)
    {
        return Island.Instance.GetParameter("ShipCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    public int GetShipAlltimeCount(int islandNumber, int shipNumber)
    {
        return Island.Instance.GetParameter("ShipAlltimeCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    public void AddShipAlltimeCount(int islandNumber, int shipNumber)
    {
        Island.Instance.SetParameter("ShipAlltimeCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), GetShipAlltimeCount(islandNumber, shipNumber) + 1);
    }

    public int GetShipBoughtCount(int islandNumber, int shipNumber)
    {
        return Island.Instance.GetParameter("ShipBoughtCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0);
    }

    public void AddShipBoughtCount(int islandNumber, int shipNumber)
    {
        Island.Instance.SetParameter("ShipBoughtCount_" + islandNumber.ToString() + "_" + shipNumber.ToString(), GetShipBoughtCount(islandNumber, shipNumber) + 1);
    }

    public bool CheckShipUnlocked(int islandNumber, int shipNumber)
    {
        return Island.Instance.GetParameter("ShipUnlocked_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 0) != 0;
    }

    public void AddShipUnlocked(int islandNumber, int shipNumber)
    {
        Island.Instance.SetParameter("ShipUnlocked_" + islandNumber.ToString() + "_" + shipNumber.ToString(), 1);
    }

    public void BuyShip(int number)
    {
        int n = Mathf.Clamp(number, 0, selectedPanel.list.ships.Count - 1);
        int shipCount = GetShipCount(selectedPanel.list.islandNumber, n);
        if (Island.Instance.ChangeMoney(-GetShipPrice(selectedPanel.list, n))) //selectedPanel.list.ships[n].price * (shipCount + 1)))
        {
            Add(selectedPanel.list.islandNumber - 1, selectedPanel.list.ships[n], false);
            SetShipCount(selectedPanel.list.islandNumber, n, Mathf.Clamp(shipCount + 1, 0, selectedPanel.transform.childCount));
            if (GetShipAlltimeCount(selectedPanel.list.islandNumber, n) == 0) EventManager.SendEvent("NewShip", selectedPanel.list.ships[n]);
            AddShipAlltimeCount(selectedPanel.list.islandNumber, n);
            AddShipBoughtCount(selectedPanel.list.islandNumber, n);
            AddShipUnlocked(selectedPanel.list.islandNumber, n);
        }
        if (n == currentShips[panels.IndexOf(selectedPanel)]) UpdateBuyButtonInfo();
    }

    public void BuyCurrentNumberShip()
    {
        int curShipNum = currentShips[panels.IndexOf(selectedPanel)];
        BuyShip(curShipNum);
        EventManager.SendEvent("ShipBought", selectedPanel.list.ships[curShipNum].name, "Button");
    }

    public BigDigit GetShipPrice(ShipInfoList list, int id)
    {
        return list.ships[id].price * Mathf.Pow(list.ships[id].priceModifier, GetShipBoughtCount(list.islandNumber, id));
    }
}
