using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    public Transform itemContainer;
    public Text titleTxt;

    public void Open(Inventory inv)
    {
        titleTxt.text = inv.list.islandName;
        foreach(ShopItem item in itemContainer.GetComponentsInChildren<ShopItem>())
        {
            item.Open(inv);
        }
    }

    public void Close()
    {
        foreach (ShopItem item in itemContainer.GetComponentsInChildren<ShopItem>())
        {
            item.Close();
        }
    }
}
