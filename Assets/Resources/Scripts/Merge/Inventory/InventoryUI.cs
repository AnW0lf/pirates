using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemsParent;

    InventorySlot[] slots;
    void Start()
    {
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
            slot.inventory = inventory;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
