using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotate : MonoBehaviour
{
    public bool isRotate = false, direction = false;
    public float speed = 20f;
    public string shipName;

    private float realSpeed;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        realSpeed = speed * (direction ? 1 : -1);
    }

    private void Start()
    {
        Subscribe();
    }

    private void FixedUpdate()
    {
        if (isRotate)
        {
            rect.localEulerAngles += Vector3.forward * realSpeed * Time.fixedDeltaTime;
        }
    }

    private void Subscribe()
    {
        EventManager.Subscribe(shipName + "SetAngleSpeed", SetSpeed);
        EventManager.Subscribe(shipName + "StopRotate", StopRotate);
        EventManager.Subscribe(shipName + "StartRotate", StartRotate);
    }

    private void SetSpeed(object[] parameters)
    {
        if (parameters.Length > 0)
        {
            speed = (float)parameters[0];
        }
    }

    private void StopRotate(object[] parameters)
    {
        isRotate = false;
    }

    private void StartRotate(object[] parameters)
    {
        isRotate = true;
        if(parameters.Length > 0)
        {
            direction = (bool)parameters[0];
        }
        realSpeed = speed * (direction ? 1 : -1);
    }
}
