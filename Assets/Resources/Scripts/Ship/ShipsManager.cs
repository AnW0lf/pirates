using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public int islandNumber;
    public ShipInfoList list;
    public GameObject shipPrefab;
    public IslandController islandController;

    public void GenerateShips(int level, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ShipInfo item = list.ships[Mathf.Clamp(level, 0, list.ships.Count - 1)];
            ShipController ship = Instantiate(shipPrefab, transform).GetComponent<ShipController>();
            ship.SetShip(item, islandController, list.islandNumber);
            ship.Motor.target = transform;
            float dst = item.distance + UnityEngine.Random.Range(-80f, 80f);
            float angle = UnityEngine.Random.Range(0f, 360f);
            ship.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Cos(angle / 180 * Mathf.PI), Mathf.Sin(angle / 180 * Mathf.PI)) * dst;
            ship.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void DestroyShips(int level, int count)
    {
        int c = 0;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            ShipController sc = transform.GetChild(i).GetComponent<ShipController>();
            if (sc && sc.item.gradeLevel == level && !sc.Destroyed && c < count)
            {
                sc.Destroyed = true;
                Destroy(sc.gameObject);
                c++;
                if (c == count) return;
            }
        }
    }
}
