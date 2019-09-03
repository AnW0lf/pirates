using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image shipIcon, star, lockIcon;
    public Text levelText;
    public Inventory inventory;

    public bool Locked { get; private set; }

    private ShipInfo item;

    private void Awake()
    {
        Locked = true;
        if (inventory == null) inventory = GetComponentInParent<Inventory>();
    }

    private void Start()
    {
        UpdateInfo();
    }

    public void AddItem(ShipInfo newItem)
    {
        item = newItem;
        UpdateInfo();
    }

    public void ClearSlot()
    {
        item = null;
        UpdateInfo();
    }

    public void SetLocked(bool locked)
    {
        Locked = locked;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if(Locked)
        {
            shipIcon.enabled = false;
            lockIcon.enabled = true;
            star.enabled = false;

            levelText.enabled = false;
        }
        else if (item != null)
        {
            shipIcon.enabled = true;
            shipIcon.sprite = item.icon;
            lockIcon.enabled = false;
            star.enabled = true;

            levelText.enabled = true;
            levelText.text = item.gradeLevel.ToString();
        }
        else
        {
            shipIcon.enabled = false;
            lockIcon.enabled = false;
            star.enabled = false;

            levelText.enabled = false;
        }
    }
}
