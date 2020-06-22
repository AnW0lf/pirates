﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBonusLighting : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private LineRenderer line;
    [SerializeField] private ShipClick shipClick;
    private float distance = 5f;
    private BonusBehavior _bh = null;

    private static int disableLevel = 5;
    private static bool locked = false;

    private void Start()
    {
        locked = Island.Instance().Level >= disableLevel;
        EventManager.Subscribe("LevelUp", (args) => { locked = Island.Instance().Level >= disableLevel; });
        Color color = shipClick.color;
        color.a = 0.4f;
        line.startColor = color;
        line.endColor = color;
    }

    private bool Raycast(out BonusBehavior bh)
    {
        bh = null;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, distance, mask);

        if (hit)
        {
            bh = hit.transform.GetComponent<BonusBehavior>();
            if (bh != null) return true;
        }

        return false;
    }

    private void Update()
    {
        if (locked) Destroy(this);
        if (Raycast(out BonusBehavior bh))
        {
            if (_bh == null)
            {
                _bh = bh;
                _bh.Lighting = true;
                print("Ship turn light on");
            }
            else if (_bh != bh)
            {
                _bh.Lighting = false;
                _bh = bh;
                _bh.Lighting = true;
                print("Ship switch BH");
            }
        }
        else if (_bh != null)
        {
            _bh.Lighting = false;
            _bh = null;
            print("Ship forget");
        }

        if (_bh != null)
        {
            if (!line.enabled) line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, _bh.transform.position);
        }
        else if (line.enabled) line.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 start, end;

        start = transform.position;
        end = start - transform.up * distance;

        Gizmos.DrawLine(start, end);
    }
}
