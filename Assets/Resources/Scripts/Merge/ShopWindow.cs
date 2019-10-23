using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    public ShopItem[] items;
    public Text titleTxt, txtNextLevel;

    public void Open(Inventory inv)
    {
        titleTxt.text = inv.selectedPanel.list.islandName.ToUpper();
        txtNextLevel.text = ((inv.panels.IndexOf(inv.selectedPanel) + 1) * 25).ToString();
        foreach (ShopItem item in items)
        {
            if (item)
                item.Open(inv);
        }
    }

    public void Close()
    {
        foreach (ShopItem item in items)
        {
            item.Close();
        }
    }
}
