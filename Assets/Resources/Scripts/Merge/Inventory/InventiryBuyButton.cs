using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventiryBuyButton : MonoBehaviour
{
    public Inventory inventory;
    public Text price;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        btn.interactable = inventory.CanAdd();
        string txt = inventory.ActualPrice(0).ToString() + "[C]";
        if (!price.text.Equals(txt)) price.text = txt;
    }
}
