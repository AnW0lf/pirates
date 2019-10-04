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
    private bool unlocked, available;

    private void Start()
    {
        if (id < 0) id = transform.GetSiblingIndex() + 1;
        island = Island.Instance;
    }

    public void Open(Inventory inv)
    {
        unlocked = false;
        available = false;
        if (!island) island = Island.Instance;
        inventory = inv;
        islandNumber = inventory.selectedPanel.list.islandNumber;
        item = inventory.selectedPanel.list.ships[Mathf.Clamp(id, 0, inventory.selectedPanel.list.ships.Count - 1)];

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
        btn.interactable = unlocked && available && !inventory.selectedPanel.IsFull
            && island.Money >= inventory.GetShipPrice(inventory.selectedPanel.list, id);
    }

    public void OnChangeInfo()
    {
        if (unlocked)
        {
            if (available)
            {
                priceTxt.text = inventory.GetShipPrice(inventory.selectedPanel.list, id).ToString();
                raidTimeTxt.text = item.raidTime.ToString() + "s";
                incomeTxt.text = item.reward.ToString();
                icon.color = Color.white;
                OnChangeBtnInteractable(new object[0]);
                priceTxt.color = btn.interactable ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                priceTxt.text = "Build\n" + inventory.selectedPanel.list.ships[Mathf.Clamp(id + 2, 0, inventory.selectedPanel.list.ships.Count - 1)].name;
                raidTimeTxt.text = item.raidTime.ToString() + "s";
                incomeTxt.text = item.reward.ToString();
                icon.color = Color.white;
                OnChangeBtnInteractable(new object[0]);
                priceTxt.color = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            }
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
        unlocked = inventory.CheckShipUnlocked(inventory.selectedPanel.list.islandNumber, id);
        available = inventory.CheckShipUnlocked(inventory.selectedPanel.list.islandNumber, Mathf.Clamp(id + 2, 0, inventory.selectedPanel.list.ships.Count - 1));
        OnChangeInfo();
    }

    public void BuyShip()
    {
        if (inventory) inventory.BuyShip(id);
        OnChangeInfo();
    }
}
