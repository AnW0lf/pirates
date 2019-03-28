using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public Transform ships;
    private bool hasShips;

    public void BeginRaids()
    {
        hasShips = false;
        foreach(Ship ship in ships.GetComponentsInChildren<Ship>())
        {
            if (ship.isShipRotating())
            {
                hasShips = true;
            }
            ship.BeginRaidFromIslandClick();
        }

        if (hasShips && GetComponent<Animation>())
        {
            GetComponent<Animation>().Play();
        }
    }
}
