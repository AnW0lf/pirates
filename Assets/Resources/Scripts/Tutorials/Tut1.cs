using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut1 : MonoBehaviour
{
    public GameObject ships;

    private Ship ship;

    private void OnEnable()
    {
        ship = ships.GetComponentInChildren<Ship>();
    }

    private void Update()
    {
        if (ship.InRaid())
            gameObject.transform.parent.gameObject.GetComponentInParent<Tutorial>().NextStage();
    }
}
