using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBonusLighting : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private float distance = 5f;
    private BonusBehavior _bh = null;

    private static int disableLevel = 2;
    private static bool locked = false;

    private void Start()
    {
        locked = Island.Instance().Level >= disableLevel;
        EventManager.Subscribe("LevelUp", (args) => { locked = Island.Instance().Level >= disableLevel; });
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
