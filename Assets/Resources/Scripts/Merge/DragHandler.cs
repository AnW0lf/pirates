﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public ShipInfo itemInfo;

    private Vector2 startPos, cursorStartPos;
    private static float k = 0f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (k == 0f) k = Camera.main.pixelHeight / Camera.main.orthographicSize / 2f;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPos = transform.position;
        cursorStartPos = eventData.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = startPos + (eventData.position - cursorStartPos) / k;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = Vector3.zero;
    }
}