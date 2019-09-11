using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public GameObject dragObj;
    public bool canDrag = false;


    private Vector2 startPos, cursorStartPos;
    private static float k = 0f;
    private Transform dragTransform;
    private Image dragImg, img;
    private CanvasGroup dragCanvasGroup;

    private void Awake()
    {
        if (k == 0f) k = Camera.main.pixelHeight / Camera.main.orthographicSize / 2f;
        dragTransform = dragObj.transform;
        img = transform.GetComponent<Image>();
        dragImg = dragObj.GetComponent<Image>();
        dragCanvasGroup = dragObj.GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        itemBeingDragged = gameObject;
        startPos = transform.position;
        cursorStartPos = eventData.position;
        dragCanvasGroup.blocksRaycasts = false;
        dragTransform.position = transform.position;
        dragImg.enabled = true;
        img.enabled = false;
        dragImg.sprite = GetComponent<Image>().sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        dragTransform.position = startPos + (eventData.position - cursorStartPos) / k;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
    }

    public void EndDrag()
    {
        itemBeingDragged = null;
        dragCanvasGroup.blocksRaycasts = true;
        dragImg.enabled = false;
        img.enabled = true;
    }
}
