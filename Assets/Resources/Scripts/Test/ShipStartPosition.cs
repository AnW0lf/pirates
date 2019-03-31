using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStartPosition : MonoBehaviour
{
    public string shipName;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(shipName + "SetAngle", SetAngle);
        EventManager.Subscribe(shipName + "SetRise", SetRise);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(shipName + "SetAngle", SetAngle);
        EventManager.Unsubscribe(shipName + "SetRise", SetRise);
    }

    private void SetRise(object[] parameters)
    {
        rect.sizeDelta = Vector2.right * (parameters.Length > 0 ? (float)parameters[0] : 350f) + Vector2.up * 10f;
    }

    private void SetAngle(object[] parameters)
    {
        rect.localEulerAngles = Vector3.forward * (parameters.Length > 0 ? (float)parameters[0]: 0f);
    }
}
