using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public ShipsManager shipsManager;
    public ShipInfoList infoList;
    public int space = 10, islandNumber;
    public List<ShipInfo> items = new List<ShipInfo>();

    private Island island;

    private int UnlockCount { get { return Mathf.Clamp(island.Level - (Mathf.Clamp(islandNumber - 1, 0, 100)) * 25, 2, space); } }

    private void Awake()
    {
        island = Island.Instance();

        for(int i = 0; i < infoList.ships.Count; i++)
        {
            for(int j = 0; j < GetShipCount(i); j++)
            {
                items.Add(infoList.ships[i]);
            }
        }
    }

    public bool CanAdd()
    {
        return items.Count < UnlockCount && island.Money >= ActualPrice(0);
    }

    public BigDigit ActualPrice(int id)
    {
        return infoList.ships[id].startPrice * (GetShipCount(id) + 1);
    }

    private int GetShipCount(int id)
    {
        string parameter = "ShipCount_" + islandNumber + "_" + id;
        return island.GetParameter(parameter, 0);
    }

    private void SetShipCount(int id, int value)
    {
        string parameter = "ShipCount_" + islandNumber + "_" + id;
        island.SetParameter(parameter, value);
    }

    public bool Add(ShipInfo item)
    {
        if (!infoList.ships.Contains(item) || items.Count >= UnlockCount) return false;

        int shipId = infoList.ships.IndexOf(item);

        if (island.ChangeMoney(-ActualPrice(infoList.ships.IndexOf(item))))
        {
            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();

            SetShipCount(shipId, GetShipCount(shipId) + 1);

            shipsManager.UpdateInfo();

            return true;
        }

        return false;
    }

    public void AddFirst()
    {
        if (!infoList.ships.Contains(infoList.ships[0]) || items.Count >= UnlockCount) return;

        if (island.ChangeMoney(-ActualPrice(0)))
        {
            items.Add(infoList.ships[0]);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();

            SetShipCount(0, GetShipCount(0) + 1);

            shipsManager.UpdateInfo();
        }
    }

    public void Remove(ShipInfo item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
