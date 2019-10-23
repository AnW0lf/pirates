using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUnlockWheel : WindowBase
{
    public bool Activate { get; set; }

    private void Start()
    {
        Opened = false;
        Activate = false;
    }

    public override void Open(object[] args)
    {
        if (Activate)
        {
            base.Open(args);
            Activate = false;
        }
    }

    public override void Close()
    {
        base.Close();
        InterfaceIerarchy ierarchy;
        if ((ierarchy = transform.GetComponentInParent<InterfaceIerarchy>())) ierarchy.Next();
    }
}
