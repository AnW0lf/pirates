using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IHasChanged
{
    public Inventory inventory;
    public Transform itemsParent;

    private InventorySlot[] slots;
    private Island island;
    private int islandNumber;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<Inventory>();
        if (inventory == null) inventory = GetComponentInParent<Inventory>();
        island = Island.Instance();
        islandNumber = inventory.islandNumber;
    }

    void Start()
    {
        inventory.onItemChangedCallback += HasChanged;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
            slot.inventory = inventory;

        HasChanged();
    }

    public void HasChanged()
    {
        int unlockCount = Mathf.Clamp(island.Level - (Mathf.Clamp(islandNumber - 1, 0, 100)) * 25, 2, 10);
        List<ShipInfo> ships = inventory.GetShipsList();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetLocked(i < unlockCount ? SlotState.UNLOCKED : SlotState.LOCKED);
            if (i < unlockCount)
            {
                if (slots[i].Item && ships.Contains(slots[i].Item))
                    ships.Remove(slots[i].Item);
                else if (slots[i].Item && !ships.Contains(slots[i].Item))
                    slots[i].ClearSlot();
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        if(ships.Count > 0)
        {
            for (int i = 0; i < slots.Length && ships.Count > 0; i++)
            {
                if (slots[i].State == SlotState.UNLOCKED)
                    slots[i].AddItem(ships[0]);
            }
        }

    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}
