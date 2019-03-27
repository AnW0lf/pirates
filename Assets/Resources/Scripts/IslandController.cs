using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    public Transform ships;

    public void BeginRaids()
    {
        foreach(Ship ship in ships.GetComponentsInChildren<Ship>())
        {
            ship.BeginRaid();
        }
    }
}
