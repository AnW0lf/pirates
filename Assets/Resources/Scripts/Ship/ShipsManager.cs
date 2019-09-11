using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public int islandNumber;
    public float distanceFromTarget = 400f;
    public GameObject[] shipsPrefabs;
    public int[] ships;

    private Island island;
    private int shipCount = 10;

    private void Awake()
    {
        island = Island.Instance;
        ships = new int[shipCount];
        for (int i = 0; i < ships.Length; i++)
        {
            island.InitParameter("ShipCount_" + islandNumber + "_" + i, 0);
            ships[i] = island.GetParameter("ShipCount_" + islandNumber + "_" + i, 0);
        }
    }

    private void Start()
    {
        //UpdateInfo();
    }

    public void UpdateInfo()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i] = island.GetParameter("ShipCount_" + islandNumber + "_" + i, 0);
            int count = 0;
            foreach (ShipController s in transform.GetComponentsInChildren<ShipController>())
                if (s.shipLevel == i) count++;
            if (count < ships[i])
            {
                GenerateShips(i, ships[i] - count);
            }
            else if (count > ships[i])
            {
                DestroyShips(i, count - ships[i]);
            }
        }
    }

    public void GenerateShips(int level, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ShipController ship = Instantiate(shipsPrefabs[level], transform).GetComponent<ShipController>();
            ship.shipLevel = level;
            ship.Motor.target = transform;
            float dst = distanceFromTarget + UnityEngine.Random.Range(-80f, 80f);
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
