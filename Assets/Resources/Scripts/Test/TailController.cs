using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailController : MonoBehaviour
{
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
        if (!sr.Scrolled) tr.time = 0.75f;
        else tr.time = 0f;
        pos = content.transform.position;
    }

}
