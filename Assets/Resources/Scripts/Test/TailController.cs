using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailController : MonoBehaviour
{
    public Ship ship;

    private TrailRenderer tr;
    private Transform content;
    private TailScrollRect sr;

    Vector3 pos;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        content = GameObject.FindGameObjectWithTag("GameScrollView").GetComponent<UnityEngine.UI.ScrollRect>().content;
        sr = GameObject.FindGameObjectWithTag("GameScrollView").GetComponent<TailScrollRect>();
    }

    private void FixedUpdate()
    {
        if (!sr.Scrolled)
        {
            if (ship.InRaid()) tr.time = 0.4f;
            else tr.time = 0.8f;
        }
        else tr.time = 0f;
        pos = content.transform.position;
    }

}
