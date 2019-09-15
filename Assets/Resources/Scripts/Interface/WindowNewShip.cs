using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowNewShip : WindowBase
{
    [SerializeField] protected Text shipName;
    [SerializeField] protected Image shipIcon;

    private void Start()
    {
        EventManager.Subscribe("NewShip", Open);
    }

    public override void Open(object[] args)
    {
        base.Open(args);
        ShipInfo item = (ShipInfo)args[0];
        shipName.text = item.name;
        shipIcon.sprite = item.icon;
        if (!Opened) Close();
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }
}
