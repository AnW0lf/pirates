using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipLook : MonoBehaviour
{
    public string shipName;

    private RectTransform rect;
    private Image image;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(shipName + "SetScale", SetScale);
        EventManager.Subscribe(shipName + "SetSprite", SetSprite);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(shipName + "SetScale", SetScale);
        EventManager.Unsubscribe(shipName + "SetSprite", SetSprite);
    }

    private void SetScale(object[] parameters)
    {
        if (parameters.Length > 0)
        {
            float scale = (float)parameters[0];
            rect.localScale = Vector3.right * scale + Vector3.up * scale * Mathf.Sign(rect.localScale.y) + Vector3.forward;
        }
    }

    private void SetSprite(object[] parameters)
    {
        if (parameters.Length > 0)
        {
            image.sprite = (Sprite)parameters[0];
        }
    }
}
