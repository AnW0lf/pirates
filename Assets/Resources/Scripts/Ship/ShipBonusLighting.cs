using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBonusLighting : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private LineRenderer line;
    [SerializeField] private ShipClick shipClick;
    [SerializeField] private float red = 0.65f, blue = 0.5f, green = 0.25f, yellow = 0f;
    private float distance = 5f;
    private BonusBehavior _bh = null;

    private static int disableLevel = 7;
    private static bool locked = false;

    private void Start()
    {
        locked = Island.Instance().Level >= disableLevel;
        EventManager.Subscribe("LevelUp", (args) => { locked = Island.Instance().Level >= disableLevel; });
        Color color = shipClick.color;
        color.a = 8f / 255f;
        line.startColor = color;
        line.endColor = color;
    }

    private bool Raycast(out BonusBehavior bh)
    {
        bh = null;

        Vector3 original = transform.position, directiion;
        RaycastHit2D hit;

        directiion = (-transform.up * distance + transform.right * red * (shipClick.ship.Direction ? 1f : -1f)).normalized;

        hit = Physics2D.Raycast(original, directiion, distance, mask);

        if (hit)
        {
            bh = hit.transform.GetComponent<BonusBehavior>();
            if (bh != null) return true;
        }

        directiion = (-transform.up * distance + transform.right * blue * (shipClick.ship.Direction ? 1f : -1f)).normalized;

        hit = Physics2D.Raycast(original, directiion, distance, mask);

        if (hit)
        {
            bh = hit.transform.GetComponent<BonusBehavior>();
            if (bh != null) return true;
        }

        directiion = (-transform.up * distance + transform.right * green * (shipClick.ship.Direction ? 1f : -1f)).normalized;

        hit = Physics2D.Raycast(original, directiion, distance, mask);

        if (hit)
        {
            bh = hit.transform.GetComponent<BonusBehavior>();
            if (bh != null) return true;
        }

        directiion = (-transform.up * distance + transform.right * yellow * (shipClick.ship.Direction ? 1f : -1f)).normalized;

        hit = Physics2D.Raycast(original, directiion, distance, mask);

        if (hit)
        {
            bh = hit.transform.GetComponent<BonusBehavior>();
            if (bh != null) return true;
        }

        //directiion = (-transform.up * distance - transform.right * 0.25f).normalized;

        //hit = Physics2D.Raycast(original, directiion, distance, mask);

        //if (hit)
        //{
        //    bh = hit.transform.GetComponent<BonusBehavior>();
        //    if (bh != null) return true;
        //}

        return false;
    }

    private void Update()
    {
        if (locked) Destroy(this);
        if (shipClick.ship.IsRotate)
        {
            if (Raycast(out BonusBehavior bh))
            {
                if (_bh == null)
                {
                    _bh = bh;
                    //_bh.Lighting = true;
                }
                else if (_bh != bh)
                {
                    _bh.Lighting = false;
                    _bh = bh;
                    //_bh.Lighting = true;
                }
            }
            else if (_bh != null)
            {
                _bh.Lighting = false;
                _bh = null;
            }

            if (_bh != null)
            {
                if (!line.enabled) line.enabled = true;
                Vector3 start, end;
                start = line.transform.position;
                end = start - transform.up * Vector3.Distance(start, _bh.transform.position);
                line.SetPosition(0, start);
                line.SetPosition(1, end);
            }
            else if (line.enabled) line.enabled = false;
        }
        else if (line.enabled) line.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 start, end;

        start = transform.position;
        end = start - transform.up * distance + transform.right * red * (shipClick.ship.Direction ? 1f : -1f);

        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.blue;

        start = transform.position;
        end = start - transform.up * distance + transform.right * blue * (shipClick.ship.Direction ? 1f : -1f);

        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.green;

        start = transform.position;
        end = start - transform.up * distance + transform.right * green * (shipClick.ship.Direction ? 1f : -1f);

        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.yellow;

        start = transform.position;
        end = start - transform.up * distance + transform.right * yellow * (shipClick.ship.Direction ? 1f : -1f);

        Gizmos.DrawLine(start, end);

        //Gizmos.color = Color.cyan;

        //start = transform.position;
        //end = start + transform.right * distance * (shipClick.ship.Direction ? 1f : -1f);

        //Gizmos.DrawLine(start, end);
    }
}
