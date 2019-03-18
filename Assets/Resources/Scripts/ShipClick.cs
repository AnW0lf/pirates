using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClick : MonoBehaviour
{
    public Ship ship;
    private bool visible = true;

    public bool IsVisible()
    {
        return visible;
    }

    private void OnMouseUpAsButton()
    {
        if (!ship.InRaid())
            ship.BeginRaid();
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }
}
