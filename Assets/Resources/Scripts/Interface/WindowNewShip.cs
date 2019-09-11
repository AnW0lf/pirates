using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowNewShip : WindowBase
{
    [SerializeField] protected Text shipName;
    [SerializeField] protected Image shipIcon;
    [SerializeField] protected Transform[] piersArray;

    private Island island;

    private void Awake()
    {
        island = Island.Instance;
    }

    public override void Open(object[] args)
    {
        foreach (Transform piers in piersArray)
        {
            foreach (PierManager pier in piers.GetComponentsInChildren<PierManager>())
            {
                if (pier.minLvl == island.Level)
                {
                    base.Open(args);
                    shipName.text = pier.shipName;
                    shipIcon.sprite = pier.shipIcon;
                    break;
                }
            }
        }
        if (!Opened) Close();
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }
}
