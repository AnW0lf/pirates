using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int id = -1;

    public Image icon;
    public Text titleTxt, priceTxt, raidTimeTxt, incomeTxt;
    public Button btn;

    public ShipInfo item { get; set; }
    public Inventory inventory { get; set; }
    private int islandNumber;
    private Island island;
    private bool unlocked;

    private void Start()
    {
        if (id < 0) id = transform.GetSiblingIndex() + 1;
        island = Island.Instance;
    }

    public void Open(Inventory inv)
    {
        unlocked = false;
        if (!island) island = Island.Instance;
        inventory = inv;
        islandNumber = inventory.list.islandNumber;
        item = inventory.list.ships[Mathf.Clamp(id, 0, inventory.list.ships.Count - 1)];

        EventManager.Subscribe("ChangeMoney", OnChangeBtnInteractable);
        EventManager.Subscribe("LevelUp", OnLevelUp);
        OnLevelUp(new object[0]);
    }

    public void Close()
    {
        EventManager.Unsubscribe("ChangeMoney", OnChangeBtnInteractable);
        EventManager.Unsubscribe("LevelUp", OnLevelUp);
    }

    private void OnChangeBtnInteractable(object[] args)
    {
        btn.interactable = unlocked && !inventory.IsFull
            && island.Money >= item.price * (inventory.GetShipAlltimeCount(islandNumber, Mathf.Clamp(id, 0, inventory.list.ships.Count - 1)) + 1);
    }

    public void OnChangeInfo()
    {
        if (unlocked)
        {
            priceTxt.text = (item.price * (inventory.GetShipAlltimeCount(islandNumber, Mathf.Clamp(id, 0, inventory.list.ships.Count - 1)) + 1)).ToString();
            raidTimeTxt.text = item.raidTime.ToString() + "s";
            incomeTxt.text = item.reward.ToString();
            icon.color = Color.white;
            OnChangeBtnInteractable(new object[0]);
            priceTxt.color = btn.interactable ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            priceTxt.text = "LOCKED";
            raidTimeTxt.text = "---s";
            incomeTxt.text = "---";
            icon.color = Color.black;
            priceTxt.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            btn.interactable = false;
        }
        titleTxt.text = item.name;
        icon.sprite = item.icon;
    }

    private void OnLevelUp(object[] args)
    {
        unlocked = island.Level >= item.unlockLevel;
        OnChangeInfo();
    }

    public void BuyShip()
    {
        if (inventory) inventory.BuyShip(id);
        OnChangeInfo();
    }
}
