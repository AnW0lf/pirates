using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailController : MonoBehaviour
{
    private TrailRenderer tr;
    private Transform content;

    UnityEngine.UI.ScrollRect sr;

    Vector3 pos;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        content = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<UnityEngine.UI.ScrollRect>().content;
    }

    private void FixedUpdate()
    {
        if (content.position == pos) tr.time = 0.75f;
        if (content.position != pos) tr.time = 0f;
        pos = content.position;
    }

}
