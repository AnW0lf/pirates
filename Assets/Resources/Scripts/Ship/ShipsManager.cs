using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public int islandNumber;
    public ShipInfoList list;
    public GameObject shipPrefab;
    public int[] ships;

    public void GenerateShips(int level, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ShipInfo item = list.ships[Mathf.Clamp(level, 0, list.ships.Count - 1)];
            ShipController ship = Instantiate(shipPrefab, transform).GetComponent<ShipController>();
            ship.shipLevel = level;
            ship.Motor.target = transform;
            ship.Motor.speed = item.speed;
            ship.Motor.duration = item.raidTime;
            ship.img.sprite = item.icon;
            float dst = item.distance + UnityEngine.Random.Range(-80f, 80f);
            float angle = UnityEngine.Random.Range(0f, 360f);
            ship.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Cos(angle / 180 * Mathf.PI), Mathf.Sin(angle / 180 * Mathf.PI)) * dst;
            ship.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void DestroyShips(int level, int count)
    {
        int c = count;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<ShipController>().shipLevel == level && c > 0)
            {
                Destroy(transform.GetChild(i).gameObject);
                c--;
                if (c == 0) return;
            }
        }
    }
}
