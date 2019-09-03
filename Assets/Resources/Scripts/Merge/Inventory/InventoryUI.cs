using UnityEngine;

public class InventoryUI : MonoBehaviour
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
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
            slot.inventory = inventory;

        UpdateUI();
    }

    void UpdateUI()
    {
        int unlockCount = Mathf.Clamp(island.Level - (Mathf.Clamp(islandNumber - 1, 0, 100)) * 25, 2, 10);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetLocked(i >= unlockCount);
            if (i < unlockCount && i < inventory.items.Count)
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
