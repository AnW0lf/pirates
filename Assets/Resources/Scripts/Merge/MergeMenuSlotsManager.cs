using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeMenuSlotsManager : MonoBehaviour
{
    public ShipsManager ships;
    public Button buyShip, shop;
    public Text buyShipPrice;
    public float priceMantissa;
    public int priceExponent;
    public Sprite[] shipsIcons;

    private Island island;
    private int filledSlots;

    private BigDigit Price { get { return new BigDigit(priceMantissa, priceExponent); } }
    private int BoughtShipsCount { get { return island.GetParameter("BoughtShipsCount_" + ships.islandNumber, 0); } set { island.SetParameter("BoughtShipsCount_" + ships.islandNumber, value); } }
    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        island.InitParameter("BoughtShipsCount_" + ships.islandNumber, 0);
        EventManager.Subscribe("ChangeMoney", UpdateInfo);
        UpdateInfo();
        buyShip.onClick.AddListener(BuyShip);
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<MergeMenuSlot>().SlotNumber = i + 1;
    }

    private void UpdateInfo(object[] args)
    {
        buyShip.interactable = island.Money >= Price * (BoughtShipsCount + 1);
    }

    public void UpdateInfo()
    {
        filledSlots = 0;

        int count = island.Level < 25 * (ships.islandNumber - 1) ? 0 : Mathf.Clamp(island.Level - 25 * (ships.islandNumber - 1), 2, 10);

        foreach (MergeMenuSlot s in transform.GetComponentsInChildren<MergeMenuSlot>())
        {
            s.Lock = count-- <= 0;
            s.Fill = false;
        }

        for (int i = ships.ships.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < ships.ships[i]; j++)
            {
                FillSlot(shipsIcons[i], i + 1);
            }
        }

        count = island.Level < 25 * (ships.islandNumber - 1) ? 0 : Mathf.Clamp(island.Level - 25 * (ships.islandNumber - 1), 2, 10);

        buyShip.interactable = filledSlots < count && island.Money >= Price * (BoughtShipsCount + 1);
        buyShipPrice.text = (Price * (BoughtShipsCount + 1)).ToString() + "[C]";
    }

    public void BuyShip()
    {
        if (ships == null || filledSlots >= 10) return;
        if (island.ChangeMoney(-(Price * (BoughtShipsCount + 1))))
        {
            BoughtShipsCount++;
            string parameter = "ShipCount_" + ships.islandNumber + "_0";
            island.SetParameter(parameter, island.GetParameter(parameter, 0) + 1);
            ships.UpdateInfo();
            UpdateInfo();
        }
    }

    private void FillSlot(Sprite sprite, int level)
    {
        foreach (MergeMenuSlot s in transform.GetComponentsInChildren<MergeMenuSlot>())
        {
            if (!s.Lock && !s.Fill)
            {
                s.Fill = true;
                s.SetIcon(sprite, level);
                filledSlots++;
                break;
            }
        }
    }
}
