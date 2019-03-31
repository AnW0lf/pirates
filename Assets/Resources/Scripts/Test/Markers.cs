using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markers : MonoBehaviour
{
    public string shipName;
    public Transform[] markers;

    private void OnEnable()
    {
        EventManager.Subscribe(shipName + "Marker", SetMarker);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(shipName + "Marker", SetMarker);
    }

    private void SetMarker(object[] parameters)
    {
        if (parameters.Length < 1) return;
        switch ((Pier.Marker)parameters[0])
        {
            case Pier.Marker.LOCK:
                markers[0].SetAsLastSibling();
                break;
            case Pier.Marker.CHECK:
                markers[1].SetAsLastSibling();
                break;
            case Pier.Marker.UPGRADE:
                markers[2].SetAsLastSibling();
                break;
            case Pier.Marker.MAXLEVEL:
                markers[3].SetAsLastSibling();
                break;
        }
    }
}
