using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeMenuSlotsManager : MonoBehaviour
{
    private Island island;
    public ShipsManager ships;

    public Button buyShip, shop;

    private int filledSlots;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Start()
    {
        UpdateInfo();
        buyShip.onClick.AddListener(BuyShip);
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
                FillSlot();
            }
        }

        count = island.Level < 25 * (ships.islandNumber - 1) ? 0 : Mathf.Clamp(island.Level - 25 * (ships.islandNumber - 1), 2, 10);

        buyShip.interactable = filledSlots < count;

        print(filledSlots);
    }

    public void BuyShip()
    {
        if (ships == null || filledSlots >= 10) return;

        string parameter = "ShipCount_" + ships.islandNumber + "_0";
        island.SetParameter(parameter, island.GetParameter(parameter, 0) + 1);
        ships.UpdateInfo();
        UpdateInfo();
    }

    private void FillSlot()
    {
        foreach (MergeMenuSlot s in transform.GetComponentsInChildren<MergeMenuSlot>())
        {
            if (!s.Lock && !s.Fill)
            {
                s.Fill = true;
                filledSlots++;
                break;
            }
        }
    }
}
