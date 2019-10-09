using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int id = -1;

    public Image icon, iconCoin;
    public Text titleTxt, priceTxt, raidTimeTxt, incomeTxt;
    public Button btn;

    public ShipInfo item { get; set; }
    public Inventory inventory { get; set; }
    private int islandNumber;
    private Island island;
    private bool unlocked, available;
    private float symWidth = 40f, maxWidth = 400f;

    public void Open(Inventory inv)
    {
        if (id < 0) id = transform.GetSiblingIndex();
        island = Island.Instance;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(BuyShip);

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
        OnChangeInfo();
    }

    public void OnChangeInfo()
    {
        if (unlocked)
        {
            if (available)
            {
                priceTxt.text = inventory.GetShipPrice(inventory.selectedPanel.list, id).ToString();
                icon.color = Color.white;

                iconCoin.gameObject.SetActive(true);
                raidTimeTxt.text = item.raidTime.ToString() + "<size=40>s</size>";
                incomeTxt.text = item.reward.ToString();
                priceTxt.color = btn.interactable ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                priceTxt.text = "Build\n" + inventory.selectedPanel.list.ships[Mathf.Clamp(id + 2, 0, inventory.selectedPanel.list.ships.Count - 1)].name;
                priceTxt.color = new Color(0.7f, 0.7f, 0.7f, 0.9f);

                iconCoin.gameObject.SetActive(false);
                raidTimeTxt.text = item.raidTime.ToString() + "<size=40>s</size>";
                incomeTxt.text = item.reward.ToString();
                icon.color = Color.white;
            }
        }
        else
        {
            priceTxt.text = "LOCKED";
            priceTxt.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            iconCoin.gameObject.SetActive(false);
            raidTimeTxt.text = "?";
            incomeTxt.text = "?";
            icon.color = Color.black;
            btn.interactable = false;
        }

        float newWidth = Mathf.Clamp(priceTxt.text.Length * symWidth, 0f, maxWidth);
        if (priceTxt.text.Contains(".")) newWidth -= symWidth / 2;
        priceTxt.rectTransform.sizeDelta = new Vector2(newWidth, priceTxt.rectTransform.sizeDelta.y);

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
