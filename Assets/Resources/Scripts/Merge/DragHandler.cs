using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static GameObject itemBeingDragged;
    public GameObject dragObj;
    public bool canDrag = false;


    private Vector2 startPos, cursorStartPos;
    private static float k = 0f;
    private Transform dragTransform;
    private Image dragImg, img;
    private CanvasGroup dragCanvasGroup;
    private GameObject starObj;

    private void Awake()
    {
        if (k == 0f) k = Camera.main.pixelHeight / Camera.main.orthographicSize / 2f;
        dragTransform = dragObj.transform;
        img = transform.GetComponent<Image>();
        dragImg = dragObj.GetComponent<Image>();
        dragCanvasGroup = dragObj.GetComponent<CanvasGroup>();
        starObj = transform.GetChild(0).gameObject;
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
        starObj.SetActive(false);
        dragImg.sprite = GetComponent<Image>().sprite;
        dragImg.rectTransform.sizeDelta = new Vector2(dragImg.rectTransform.sizeDelta.x, dragImg.rectTransform.sizeDelta.x * dragImg.sprite.texture.height / dragImg.sprite.texture.width);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        dragTransform.position = startPos + (eventData.position - cursorStartPos) / k;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(itemBeingDragged) EndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        CurrentItem item = GetComponentInParent<CurrentItem>();
        CurrentItem other = itemBeingDragged.GetComponentInParent<CurrentItem>();
        if (item && other && item.item && other.item && item.item == other.item)
        {
            item.inventory.Merge(item, other);
        }
        else item.inventory.Switch(item, other);
    }

    public void EndDrag()
    {
        itemBeingDragged = null;
        dragCanvasGroup.blocksRaycasts = true;
        dragImg.enabled = false;
        img.enabled = true;
        starObj.SetActive(true);
    }
}
