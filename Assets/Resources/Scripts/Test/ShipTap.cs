using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTap : MonoBehaviour
{
    public string shipName;

    public void Click()
    {
        EventManager.SendEvent(shipName + "Click");
    }
}
