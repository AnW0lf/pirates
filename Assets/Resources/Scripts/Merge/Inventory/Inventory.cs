using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    List<ShipInfo> items;

    private void Start()
    {
        items = new List<ShipInfo>();
    }
}
