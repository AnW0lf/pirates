using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Transform icon;
    public GameObject star, locked;
    public Text levelText;
    public Inventory inventory;
    [Header("Префаб корабля")]
    public GameObject shipPref;

    public SlotState State { get; set; }

    public ShipInfo Item { get; private set; }

    private void Awake()
    {
        if (inventory == null) inventory = GetComponentInParent<Inventory>();
    }

    private void Start()
    {
        UpdateInfo();
    }

    public void AddItem(ShipInfo newItem)
    {
        Item = newItem;
        UpdateInfo();
    }

    public void ClearSlot()
    {
        Item = null;
        UpdateInfo();
    }

    public void SetLocked(SlotState state)
    {
        State = state;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (Item && icon.childCount == 0)
        {
            GameObject o = Instantiate(shipPref, icon);
            o.GetComponent<Image>().sprite = Item.icon;
            o.GetComponent<DragHandler>().itemInfo = Item;
            icon.GetComponent<Slot>().itemInfo = Item;
            levelText.text = Item.gradeLevel.ToString();
        }
        else if (!Item && icon.childCount != 0)
            Destroy(icon.GetChild(0).gameObject);

        if (icon.childCount != 0)
            State = SlotState.FILLED;
        else Item = null;

        locked.SetActive(State == SlotState.LOCKED);
        star.SetActive(State == SlotState.FILLED);
    }
}

public enum SlotState { LOCKED, UNLOCKED, FILLED }
