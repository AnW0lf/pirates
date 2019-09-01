using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewShipInfoList", menuName = "Ships/ShipInfoList")]
public class ShipInfoList : ScriptableObject
{
    public int islandNumber = 1;
    public List<ShipInfo> ships = null;
}
