using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUnlockWheel : WindowBase
{
    protected int minLevel = 2;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    public override void Open(object[] args)
    {
        if (island.Level == minLevel)
            base.Open(args);
        else Close();
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }
}
